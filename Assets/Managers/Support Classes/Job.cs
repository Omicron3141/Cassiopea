using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Job  {
	
	private static int highestID = 0;
	// Unique ID
	public int Id;
	// 0-2
	public int priority = 2;
	// Place to go to to do job
	public Vector3 Location;
	// How long to spend doing the job
	public float duration;
	// Jobs that always need to be done, e.g. manning a station
	public bool permenant = false;
	// A human-readable description
	public string desc;
	// The crew member the job is assigned to, or null if unassigned
	public Person assignedCrew;
	// the action to call when the job is done
	public Action onComplete;
	bool assigned = false;

	public Job() {
		Id = highestID;
		highestID += 1;
	}

	public string Description() {
		return desc + " (" + Location.x + "," + Location.y + ")";
	}
}
