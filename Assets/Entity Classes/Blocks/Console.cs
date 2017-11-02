using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Console : Entity {
	public Vector2 interactionSpot;
	public float maintenanceChance;
	public float maintenanceDuraction;
	public string humanReadableName;
	public int maintenancePriority = 1;
	private CrewManager crewManager;
	public bool broken = false;
	public bool manned = false;
	public bool mannable = false;
	// Use this for initialization
	void Start () {
		crewManager = CrewManager.instance;
		if (mannable) {
			Job j = new Job ();
			j.Location = new Vector2(transform.localPosition.x, transform.localPosition.y) + interactionSpot;
			j.AssignerLocation = transform.localPosition;
			j.permenant = true;
			j.desc = "Man " + humanReadableName + " at";
			j.priority = 2;
			j.onStart = NowManned;
			j.onInterrupt = NowUnmanned;
			j.tool = Job.HANDS;
			crewManager.addNewJob (j);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Random.value < maintenanceChance && !broken) {
			Job j = new Job();
			j.Location = new Vector2(transform.localPosition.x, transform.localPosition.y) + interactionSpot;
			j.AssignerLocation = transform.localPosition;
			j.duration = maintenanceDuraction;
			j.desc = "Perform maintenance on " + humanReadableName + " at";
			j.onComplete = MaintenanceComplete;
			j.priority = maintenancePriority;
			j.tool = Job.HANDS;
			crewManager.addNewJob(j);
			broken = true;
		}
	}

	void MaintenanceComplete() {
		broken = false;
	}

	void NowManned() {
		manned = true;
	}
	void NowUnmanned() {
		manned = false;
	}
}
