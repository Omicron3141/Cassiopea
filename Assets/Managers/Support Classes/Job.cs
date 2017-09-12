using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Job  {
	
	private static int highestID = 0;

	public int Id;
	public Vector3 Location;
	bool assigned = false;

	public Job() {
		Id = highestID;
		highestID += 1;
	}
}
