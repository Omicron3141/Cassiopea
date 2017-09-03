using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewManager: MonoBehaviour {

	public static CrewManager instance;

	List<Person> UnassignedCrewMembers = new List<Person> ();
	List<Person> AssignedCrewMembers = new List<Person> ();

	Queue<Job> UnassignedJobs = new Queue<Job> ();
	List<Job> AssignedJobs = new List<Job> ();

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
		}

	}

	public void AddCrewMember(Person p) {
		UnassignedCrewMembers.Add(p);
	}


	public void UnassignJob(Person p, Job j){
		AssignedJobs.Remove(j);
		AssignedCrewMembers.Remove (p);
		UnassignedCrewMembers.Add (p);
	}

	public void addNewJob(Job j){
		UnassignedJobs.Enqueue(j);
	}

}
