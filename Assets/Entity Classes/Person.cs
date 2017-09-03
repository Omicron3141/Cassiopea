﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person: Entity  {
	string Name;
	public float speed;
	public Job currentJob;
	public bool doingJob;
	private Job CurrentJob{ get{ return currentJob; } }
	private bool DoingJob{ get{ return doingJob; } }

	private CrewManager manager;

	void Start () {
		manager = CrewManager.instance;
		manager.AddCrewMember (this);
	}


	void Update () {
		if (doingJob) {
			if (Mathf.Abs((currentJob.Location - transform.position).magnitude) < speed * Time.deltaTime) {
				transform.position = currentJob.Location;
				doingJob = false;
				manager.UnassignJob (this, currentJob);
				currentJob = null;

			} else {
				float Xdirection = (currentJob.Location.x - transform.position.x)/Mathf.Abs(currentJob.Location.x - transform.position.x);
				transform.Translate (Xdirection * speed * Time.deltaTime, 0f, 0f);
			}
		}
	}

	public void assignJob(Job job) {
		currentJob = job;
		doingJob = true;
	}
}
