using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person: Entity  {
	string Name;

	private bool selected = false;

	public float speed;
	public float idleSpeed;
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
		target = Ship.playerShip.map.getNewWanderTarget (transform.localPosition);
	}


	void Update () {
		if (state == IDLE) {
			if (Mathf.Abs ((target - transform.localPosition).magnitude) < speed * Time.deltaTime) {
				transform.localPosition = target;
				target = Ship.playerShip.map.getNewWanderTarget (transform.localPosition);
			} else {
				float Xdirection;
				if (target.x == transform.localPosition.x) {
					Xdirection = 0;
				} else {
					Xdirection = (target.x - transform.localPosition.x) / Mathf.Abs (target.x - transform.localPosition.x);
				}

				float Ydirection;
				if (target.y == transform.localPosition.y) {
					Ydirection = 0;
				} else {
					Ydirection = (target.y - transform.localPosition.y) / Mathf.Abs (target.y - transform.localPosition.y);
				}
				float facing = -1f;
				if (Xdirection < 0) {
					facing = 1f;
				}
				transform.localScale = new Vector3 (facing, 1f, 1f);
				transform.Translate (Xdirection * idleSpeed * Time.deltaTime, Ydirection * idleSpeed * Time.deltaTime, 0f);
			}
		} else if (state == MOVINGTOJOB) {
			if (Mathf.Abs ((target - transform.localPosition).magnitude) < speed * Time.deltaTime) {
				transform.localPosition = target;
				if (path.Count == 0) {
					if (currentJob.duration > 0 || currentJob.permenant) {
						transform.Find ("Hands").gameObject.SetActive (true);
					}
					state = DOINGJOB;

				} else {
					target = path.Dequeue ();
				}

			} else {
				float Xdirection;
				if (target.x == transform.localPosition.x) {
					Xdirection = 0;
				} else {
					Xdirection = (target.x - transform.localPosition.x) / Mathf.Abs (target.x - transform.localPosition.x);
				}

				float Ydirection;
				if (target.y == transform.localPosition.y) {
					Ydirection = 0;
				} else {
					Ydirection = (target.y - transform.localPosition.y) / Mathf.Abs (target.y - transform.localPosition.y);
				}
				float facing = -1f;
				if (Xdirection < 0) {
					facing = 1f;
				}
				transform.localScale = new Vector3 (facing, 1f, 1f);
				transform.Translate (Xdirection * speed * Time.deltaTime, Ydirection * speed * Time.deltaTime, 0f);
			}
		} else if (state == DOINGJOB) {
			if (!currentJob.permenant) {
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
	}

	public void assignJob(Job job) {
		currentJob = job;
		currentJob.Location.z = transform.localPosition.z;
		path = Ship.playerShip.map.pathfind (transform.localPosition, currentJob.Location);
		target = path.Dequeue ();
		state = MOVINGTOJOB;
		transform.Find ("Hands").gameObject.SetActive (false);

	}

	public bool getSelected() {
		return selected;
	}

	public void setSelect (bool sel) {
		selected = sel;
		transform.Find ("Select").GetComponent<SpriteRenderer> ().enabled = sel;
	}

	public string jobDesc() {
		if (state == IDLE) {
			return "Idle";
		} else if (state == MOVINGTOJOB) {
			return "Moving to job:\n \"" + currentJob.Description() + "\"";
		} else if (state == DOINGJOB) {
			return "Doing job \n\"" + currentJob.Description() + "\"";
		} else {
			return "N/A";
		}
	}

}
