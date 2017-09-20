using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Job  {
	
	private static int highestID = 0;

	public int Id;
	public int priority = 2;
	public Vector3 Location;
	public float duration;
	public bool permenant = false;
	public string desc;
	public Person assignedCrew;
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
