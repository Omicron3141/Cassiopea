using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrewManager: MonoBehaviour {

	public static CrewManager instance;

	private List<Person> UnassignedCrewMembers = new List<Person> ();
	private List<Person> AssignedCrewMembers = new List<Person> ();

	private Queue<Job> UnassignedJobs = new Queue<Job> ();
	private List<Job> AssignedJobs = new List<Job> ();

	public Person selectedCrew;

	public Text jobsUI;
	public Text traitsUI;

	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
		} else {
			Destroy (gameObject);
		}

	}

	
	// Update is called once per frame
	void Update () {

		if (UnassignedJobs.Count > 0 && UnassignedCrewMembers.Count > 0) {
			Job currentJob = UnassignedJobs.Dequeue ();
			UnassignedCrewMembers [0].assignJob (currentJob);
			AssignedCrewMembers.Add (UnassignedCrewMembers [0]);
			UnassignedCrewMembers.RemoveAt (0);
			AssignedJobs.Add (currentJob);
			updateJobsUI ();
		}

		updateTraitsUI ();

	}

	public void AddCrewMember(Person p) {
		UnassignedCrewMembers.Add(p);
	}


	public void UnassignJob(Person p, Job j){
		AssignedJobs.Remove(j);
		AssignedCrewMembers.Remove (p);
		UnassignedCrewMembers.Add (p);
		updateJobsUI ();
	}

	public void addNewJob(Job j){
		UnassignedJobs.Enqueue(j);
		updateJobsUI ();
	}

	private void updateJobsUI() {
		string text = "";
		foreach (Job j in AssignedJobs) {
			text += " * "+j.Description()+"\n";
		}
		foreach (Job j in UnassignedJobs) {
			text += "   "+j.Description()+"\n";
		}
		jobsUI.text = text;
	}

	private void updateTraitsUI() {
		string text = "";

		if (selectedCrew != null) {
			text += " Name: " + selectedCrew.crewName + "\n";
			text += " Age: " + selectedCrew.age + "\n";
			text += " Profession: " + selectedCrew.profession + "\n";
			text += " Currently: " + selectedCrew.jobDesc() + "\n";
		} 

		else {
			text += "No crew selected." + "\n";
		}

		traitsUI.text = text;
	}

}
