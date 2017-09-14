using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Console : Entity {
	public Vector2 interactionSpot;
	public float maintenanceChance;
	public float maintenanceDuraction;
	public string humanReadableName;
	private CrewManager crewManager;
	public bool broken = false;
	// Use this for initialization
	void Start () {
		crewManager = CrewManager.instance;
	}
	
	// Update is called once per frame
	void Update () {
		if (Random.value < maintenanceChance && !broken) {
			Job j = new Job();
			j.Location = new Vector2(transform.position.x, transform.position.y) + interactionSpot;
			j.duration = maintenanceDuraction;
			j.desc = "Perform maintenance on " + humanReadableName;
			j.onComplete = MaintenanceComplete;
			crewManager.addNewJob(j);
			broken = true;
		}
	}

	void MaintenanceComplete() {
		broken = false;
	}
}
