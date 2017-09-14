using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Job  {
	
	private static int highestID = 0;

	public int Id;
	public Vector3 Location;
	public float duration;
	public string desc;
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
