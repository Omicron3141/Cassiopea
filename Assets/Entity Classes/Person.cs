using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person: Entity  {
	string Name;
	public float speed;
	public Job currentJob;
	public bool doingJob;
	private Job CurrentJob{ get{ return currentJob; } }
	private bool DoingJob{ get{ return doingJob; } }
	private Queue<Vector3> path;
	private Vector3 target;

	private CrewManager manager;
	SpriteRenderer Body;

	void Start () {
		manager = CrewManager.instance;
		manager.AddCrewMember (this);
		Body = transform.Find ("Body").GetComponent<SpriteRenderer>();
	}


	void Update () {
		if (doingJob) {
			if (Mathf.Abs((target - transform.position).magnitude) < speed * Time.deltaTime) {
				transform.position = target;
				if (path.Count == 0) {
					doingJob = false;
					manager.UnassignJob (this, currentJob);
					currentJob = null;
				} else {
					target = path.Dequeue ();
				}

			} else {
				float Xdirection;
				if (target.x == transform.position.x) {
					Xdirection = 0;
				} else {
					Xdirection = (target.x - transform.position.x)/Mathf.Abs(target.x - transform.position.x);
				}

				float Ydirection;
				if (target.y == transform.position.y) {
					Ydirection = 0;
				} else {
					Ydirection = (target.y - transform.position.y)/Mathf.Abs(target.y - transform.position.y);
				}

				Body.flipX = (Xdirection > 0);
				transform.Translate (Xdirection * speed * Time.deltaTime, Ydirection * speed * Time.deltaTime, 0f);
			}
		}
	}

	public void assignJob(Job job) {
		currentJob = job;
		currentJob.Location.z = transform.position.z;
		path = Ship.playerShip.map.pathfind (transform.position, currentJob.Location);
		target = path.Dequeue ();
		doingJob = true;
	}
}
