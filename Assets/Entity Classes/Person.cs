using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person: Entity  {
	string Name;

	private bool selected = false;

	public float speed;
	public Job currentJob;
	private Job CurrentJob{ get{ return currentJob; } }
	private bool DoingJob{ get{ return state != IDLE; } }
	private Queue<Vector3> path;
	private Vector3 target;
	public string crewName;
	public string age;
	public string profession;


	private CrewManager manager;
	SpriteRenderer Body;


	private int state = 0;

	private static int IDLE = 0;
	private static int MOVINGTOJOB = 1;
	private static int DOINGJOB = 2;

	void Start () {
		manager = CrewManager.instance;
		manager.AddCrewMember (this);
		Body = transform.Find ("Body").GetComponent<SpriteRenderer>();
		this.crewName = "John Smith";
		this.profession = "Engineer";
		this.age = "30";
	}


	void Update () {
		if (state == MOVINGTOJOB) {
			if (Mathf.Abs ((target - transform.position).magnitude) < speed * Time.deltaTime) {
				transform.position = target;
				if (path.Count == 0) {
					if (currentJob.duration > 0) {
						transform.Find ("Hands").gameObject.SetActive (true);
					}
					state = DOINGJOB;

				} else {
					target = path.Dequeue ();
				}

			} else {
				float Xdirection;
				if (target.x == transform.position.x) {
					Xdirection = 0;
				} else {
					Xdirection = (target.x - transform.position.x) / Mathf.Abs (target.x - transform.position.x);
				}

				float Ydirection;
				if (target.y == transform.position.y) {
					Ydirection = 0;
				} else {
					Ydirection = (target.y - transform.position.y) / Mathf.Abs (target.y - transform.position.y);
				}
				float facing = -1f;
				if (Xdirection < 0) {
					facing = 1f;
				}
				transform.localScale = new Vector3 (facing, 1f, 1f);
				transform.Translate (Xdirection * speed * Time.deltaTime, Ydirection * speed * Time.deltaTime, 0f);
			}
		} else if (state == DOINGJOB) {
			currentJob.duration -= Time.deltaTime;
			if (currentJob.duration < 0) {
				manager.UnassignJob (this, currentJob);
				if (currentJob.onComplete != null) {
					currentJob.onComplete.Invoke ();
				}
				currentJob = null;
				state = IDLE;
				transform.Find ("Hands").gameObject.SetActive (false);

			}
		}
	}

	public void assignJob(Job job) {
		currentJob = job;
		currentJob.Location.z = transform.position.z;
		path = Ship.playerShip.map.pathfind (transform.position, currentJob.Location);
		target = path.Dequeue ();
		state = MOVINGTOJOB;
	}

	public bool getSelected() {
		return selected;
	}

	public void setSelect (bool sel) {
		selected = sel;
		transform.Find ("Select").GetComponent<SpriteRenderer> ().enabled = sel;
	}

}
