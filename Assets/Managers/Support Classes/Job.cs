using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Job  {
	
	private static int highestID = 0;
	public static int NONE = 0;
	public static int HANDS = 1;
	public static int WELD = 2;
	// Unique ID
	public int Id;
	// 0-2
	public int priority = 2;
	// Place to go to to do job
	public Vector3 Location;
	// The location of the thing assigning the job
	public Vector3 AssignerLocation;
	// How long to spend doing the job
	public float duration;
	// Jobs that always need to be done, e.g. manning a station
	public bool permenant = false;
	// A human-readable description
	public string desc;
	// the number for the tool crew should use on this job
	public int tool = NONE;
	// The crew member the job is assigned to, or null if unassigned
	public Person assignedCrew;
	// the action to call when the job is received
	public Action onReceive;
	// the action to call when the job is started
	public Action onStart;
	// the action to call when the job is interrupted
	public Action onInterrupt;
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
