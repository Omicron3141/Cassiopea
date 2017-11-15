﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrewManager: MonoBehaviour {

	public static CrewManager instance;

	// The number of possible priorities
	public const int priorities = 3;

	// Crew
	private List<Person> UnassignedCrewMembers = new List<Person> ();
	private List<Person> AssignedCrewMembers = new List<Person> ();

	// List of length priorities, where each entry is a queue of jobs
	private List<List<Job>> UnassignedJobs = new List<List<Job>> ();

	private List<List<Job>> AssignedJobs = new List<List<Job>> ();

	// currently selected crew member
	public Person selectedCrew;

	public Text jobsUI;
	public Text traitsUI;

	// Use this for initialization
	void Awake () {
		// handles singleton
		if (instance == null) {
			instance = this;
		} else {
			Destroy (gameObject);
		}

		// Create the empty lists and queues for jobs
		for (int i = 0; i < priorities; i++) {
			UnassignedJobs.Add(new List<Job> ());
			AssignedJobs.Add(new List<Job> ());
		}

	}

	
	// Update is called once per frame
	void Update () {
		//assignJobsByJobs();
		assignJobsByCrew();

		updateTraitsUI ();

	}

	// try to find the best job for each crew member
	void assignJobsByCrew() {
		for (int c = 0; c < UnassignedCrewMembers.Count; c++) {
			Person crew = UnassignedCrewMembers [c];
			Vector3 crewloc = crew.gameObject.transform.localPosition;
			float mindist = Mathf.Infinity;
			Job minjob = null;
			int priority = 0;
			while (priority < priorities && UnassignedJobs [priority].Count == 0) {
				priority++;
			}
			if (UnassignedJobs [priority].Count > 0) {
				foreach (Job j in UnassignedJobs[priority]) {
					if ((j.Location - crewloc).magnitude < mindist) {
						mindist = (j.Location - crewloc).magnitude;
						minjob = j;
					}
				}
				UnassignedJobs [priority].Remove(minjob);
				crew.assignJob (minjob);
				minjob.assignedCrew = crew;
				AssignedCrewMembers.Add (crew);
				UnassignedCrewMembers.Remove (crew);
				AssignedJobs[priority].Add (minjob);
			}
		}
		// see if already assigned crew have something better to do
		for (int c = 0; c < AssignedCrewMembers.Count; c++) {
			Person crew = AssignedCrewMembers [c];
			Vector3 crewloc = crew.gameObject.transform.localPosition;
			float mindist = Mathf.Infinity;
			Job minjob = null;
			int priority = 0;
			while (priority < crew.currentJob.priority-1 && UnassignedJobs [priority].Count == 0) {
				priority++;
			}
			if (priority >= 0 && UnassignedJobs [priority].Count > 0) {
				foreach (Job j in UnassignedJobs[priority]) {
					if ((j.Location - crewloc).magnitude < mindist) {
						mindist = (j.Location - crewloc).magnitude;
						minjob = j;
					}
				}
				Job oldJob = crew.currentJob;
				AssignedJobs [oldJob.priority].Remove(oldJob);
				oldJob.assignedCrew = null;
				UnassignedJobs [priority].Remove(minjob);
				crew.assignJob (minjob);
				minjob.assignedCrew = crew;
				AssignedJobs [priority].Add (minjob);
				UnassignedJobs [oldJob.priority].Add (oldJob);
			}
		}
	}

	// try to find the best crew member for each job.
	void assignJobsByJobs() {
		// Go through each priority level
		for (int priority = 0; priority < priorities; priority++) {
			// If there is an unassigned job of this priority, try to assign
			if (UnassignedJobs [priority].Count > 0) {
				// Are there any free crew
				if (UnassignedCrewMembers.Count > 0) {
					// Assign it to that crew
					Job currentJob = UnassignedJobs [priority][0];
					UnassignedJobs [priority].RemoveAt (0);
					UnassignedCrewMembers [0].assignJob (currentJob);
					currentJob.assignedCrew = UnassignedCrewMembers [0];
					AssignedCrewMembers.Add (UnassignedCrewMembers [0]);
					UnassignedCrewMembers.RemoveAt (0);
					AssignedJobs[priority].Add (currentJob);
					// there are no available crew, check if this is the lowest priority
				} else if (priority < priorities-1) {
					// if this is not the lowest-priority list, that is, if there are lower priority lists.
					// search through all lower-priority lists, starting with the lowest priority
					for (int priorityToStealFrom = priorities - 1; priorityToStealFrom > priority; priorityToStealFrom--) {
						// if there's a job we can steal from
						if (AssignedJobs [priorityToStealFrom].Count > 0) {
							if (UnassignedJobs [priority].Count > 0) {

								Job oldJob = AssignedJobs [priorityToStealFrom] [0];
								AssignedJobs [priorityToStealFrom].RemoveAt (0);
								Person p = oldJob.assignedCrew;
								oldJob.assignedCrew = null;
								Job newJob = UnassignedJobs [priority][0];
								UnassignedJobs [priority].RemoveAt (0);
								p.assignJob (newJob);
								newJob.assignedCrew = p;
								AssignedJobs [priority].Add (newJob);
								UnassignedJobs [priorityToStealFrom].Add (oldJob);
								updateJobsUI ();
							}
						}
					}
				}
			}
		}
	}

	// Add a new crew member to the lists
	public void AddCrewMember(Person p) {
		UnassignedCrewMembers.Add(p);
	}

	// Deletes the job, and makes the crew member unassigned again
	public void UnassignJob(Person p, Job j){
		AssignedJobs[j.priority].Remove(j);
		p.currentJob = null;
		AssignedCrewMembers.Remove (p);
		UnassignedCrewMembers.Add (p);
		updateJobsUI ();
	}

	// Add a new job to the unassigned queues
	public void addNewJob(Job j){
		UnassignedJobs[j.priority].Add(j);
		updateJobsUI ();
	}

	// recall an assigned or unassigned job
	public void recallJob(Job j) {
		if (!UnassignedJobs [j.priority].Remove (j)) {
			UnassignJob (j.assignedCrew, j);
		}
	}

	public List<Person> getCrewMembers() {
		List<Person> CrewList = this.AssignedCrewMembers;
		CrewList.AddRange(this.UnassignedCrewMembers);
		return CrewList;
	}
		
	private void updateJobsUI() {
		string text = "";
		for (int i = 0; i < priorities; i++) {
			foreach (Job j in AssignedJobs[i]) {
				text += " * " + j.Description () + "\n";
			}
			foreach (Job j in UnassignedJobs[i]) {
				text += "   " + j.Description () + "\n";
			}
		}
		jobsUI.text = text;
	}

	private void updateTraitsUI() {
		string text = "";

		if (selectedCrew != null) {
			text += " Name: " + selectedCrew.crewName + "\n";
			text += " Age: " + selectedCrew.age + "\n";
			text += " Profession: " + selectedCrew.profession + "\n";
			text += " Pilot Level: " + selectedCrew.pilotLevel + "\n";
			text += " Navigation Level: " + selectedCrew.navigationLevel + "\n";
			text += " Engineer Level: " + selectedCrew.engineerLevel + "\n";
			text += " Science Level: " + selectedCrew.scienceLevel + "\n";
			text += " Weapons Level: " + selectedCrew.weaponsLevel + "\n";
			text += " Personal Combat Level: " + selectedCrew.personalCombatLevel + "\n";
			text += " Currently: " + selectedCrew.jobDesc() + "\n";
		} 

		else {
			text += "No crew selected." + "\n";
		}

		traitsUI.text = text;
	}

}
