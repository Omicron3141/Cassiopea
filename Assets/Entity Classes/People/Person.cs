using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person: Entity  {
	string Name;

	private bool selected = false;

	// walk speed
	public float speed;
	// walk speed while idle
	public float idleSpeed;
	// the current job being done, or null
	public Job currentJob;
	// A paramater to get the current job
	private Job CurrentJob{ get{ return currentJob; } }
	// access state
	private bool DoingJob{ get{ return state != IDLE; } }
	// a path is a queue of waypoints
	private Queue<Vector3> path;
	// the current waypoint
	private Vector3 target;

	public string crewName;
	public string age;
	public string profession;
	public int weaponsLevel;
	public int pilotLevel;
	public int engineerLevel;
	public int scienceLevel;
	public int navigationLevel;
	public int personalCombatLevel;
	public List<string> firstNames = new List<string>();
	public List<string> lastNames = new List<string> ();
	public List<string> professions = new List<string> ();

	private CrewManager manager;
	SpriteRenderer Body;


	private int state = 0;

	private static int IDLE = 0;
	private static int MOVINGTOJOB = 1;
	private static int DOINGJOB = 2;

	void Start () {
		// get the crew manager
		manager = CrewManager.instance;
		// add yourself to the crew list
		manager.AddCrewMember (this);
		// find your body
		Body = transform.Find ("Body").GetComponent<SpriteRenderer>();

		string currentLine; // For looping through the first names.
		System.IO.StreamReader listOfFirstNames = new System.IO.StreamReader("first_names.txt");

		while ((currentLine = listOfFirstNames.ReadLine ()) != null) {
			firstNames.Add(currentLine);
		}

		int randomFirstIndex = UnityEngine.Random.Range (0, firstNames.Count);

		string nextCurrentLine; // For looping through the last names.
		System.IO.StreamReader listOfLastNames = new System.IO.StreamReader("last_names.txt");

		while ((nextCurrentLine = listOfLastNames.ReadLine ()) != null) {
			lastNames.Add(nextCurrentLine);
		}

		int randomLastIndex = UnityEngine.Random.Range (0, lastNames.Count);

		string profCurrentLine; // For looping through the professions.
		System.IO.StreamReader listOfProfessions = new System.IO.StreamReader("professions.txt");

		while ((profCurrentLine = listOfProfessions.ReadLine ()) != null) {
			professions.Add(profCurrentLine);
		}

		int randomProfIndex = UnityEngine.Random.Range (0, professions.Count);

		this.crewName = firstNames[randomFirstIndex] + " " + lastNames[randomLastIndex];
		this.profession = professions [randomProfIndex];
		this.age = UnityEngine.Random.Range (25, 70).ToString ();

		if (this.profession == "Engineering Officer") {
			this.pilotLevel = 2;
			this.engineerLevel = 4;
			this.scienceLevel = 2;
			this.navigationLevel = 1;
			this.weaponsLevel = 2;
			this.personalCombatLevel = 1;
		}

		if (this.profession == "Pilot") {
			this.pilotLevel = 4;
			this.engineerLevel = 2;
			this.scienceLevel = 1;
			this.navigationLevel = 3;
			this.weaponsLevel = 2;
			this.personalCombatLevel = 1;
		}

		if (this.profession == "Software Engineer") {
			this.pilotLevel = 2;
			this.engineerLevel = 2;
			this.scienceLevel = 3;
			this.navigationLevel = 3;
			this.weaponsLevel = 1;
			this.personalCombatLevel = 1;
		}

		if (this.profession == "Weapons Specialist") {
			this.pilotLevel = 1;
			this.engineerLevel = 2;
			this.scienceLevel = 1;
			this.navigationLevel = 1;
			this.weaponsLevel = 4;
			this.personalCombatLevel = 3;
		}

		if (this.profession == "Security Officer") {
			this.pilotLevel = 1;
			this.engineerLevel = 1;
			this.scienceLevel = 1;
			this.navigationLevel = 1;
			this.weaponsLevel = 3;
			this.personalCombatLevel = 4;
		}

		if (this.profession == "Navigation Officer") {
			this.pilotLevel = 3;
			this.engineerLevel = 1;
			this.scienceLevel = 2;
			this.navigationLevel = 4;
			this.weaponsLevel = 2;
			this.personalCombatLevel = 1;
		}

		if (this.profession == "Captain") {
			this.pilotLevel = 2;
			this.engineerLevel = 2;
			this.scienceLevel = 2;
			this.navigationLevel = 3;
			this.weaponsLevel = 2;
			this.personalCombatLevel = 2;
		}

		if (this.profession == "Science Officer") {
			this.pilotLevel = 1;
			this.engineerLevel = 3;
			this.scienceLevel = 4;
			this.navigationLevel = 2;
			this.weaponsLevel = 1;
			this.personalCombatLevel = 1;
		}

		// start idling
		target = Ship.playerShip.map.getNewWanderTarget (transform.localPosition);
	}


	void Update () {
		if (currentJob == null) {
			state = IDLE;
		}
		if (state == IDLE) {
			// are we basically at our target?
			if (Mathf.Abs ((target - transform.localPosition).magnitude) < speed * Time.deltaTime) {
				// move to our target
				transform.localPosition = new Vector3(target.x, target.y, 0f);
				// get a new wander point
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
			// if we are basically at our target
			if (Mathf.Abs ((target - transform.localPosition).magnitude) < speed * Time.deltaTime) {
				// go to the target
				transform.localPosition = new Vector3(target.x, target.y, 0f);
				// was this our last waypoint
				if (path.Count == 0) {
					// start doing the job
					if (currentJob.duration > 0 || currentJob.permenant) {
						float facing = -1f;
						if (currentJob.AssignerLocation.x - currentJob.Location.x < 0) {
							facing = 1f;
						}
						transform.localScale = new Vector3 (facing, 1f, 1f);
						if (currentJob.tool == Job.HANDS) {
							transform.Find ("Hands").gameObject.SetActive (true);
						}else if (currentJob.tool == Job.WELD) {
							transform.Find ("Welder").gameObject.SetActive (true);
						}
					}
					if (currentJob.onStart != null) {
						currentJob.onStart.Invoke ();
					}
					state = DOINGJOB;

				} else {
					// get the next waypoint
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
			// if the job is not permenant
			if (!currentJob.permenant) {
				// decrease the time remaining
				currentJob.duration -= Time.deltaTime;
				// if we're done
				if (currentJob.duration < 0) {
					if (currentJob.onComplete != null) {
						currentJob.onComplete.Invoke ();
					}
					manager.UnassignJob (this, currentJob);
					currentJob = null;
					state = IDLE;
					transform.Find ("Hands").gameObject.SetActive (false);
					transform.Find ("Welder").gameObject.SetActive (false);

				}
			}
		}
	}

	public void assignJob(Job job) {
		if (currentJob != null && currentJob.onInterrupt != null) {
			currentJob.onInterrupt.Invoke ();
		}
		currentJob = job;
		currentJob.Location.z = transform.localPosition.z;
		path = Ship.playerShip.map.pathfind (transform.localPosition, currentJob.Location);
		target = path.Dequeue ();
		state = MOVINGTOJOB;
		transform.Find ("Hands").gameObject.SetActive (false);
		transform.Find ("Welder").gameObject.SetActive (false);
		if (currentJob.onReceive != null) {
			currentJob.onReceive.Invoke ();
		}

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

	public void improveSkill(string skill) {
		if (skill == "weapons") {
			if (this.weaponsLevel < 5) {
				this.weaponsLevel += 1;
			}
		}

		if (skill == "pilot") {
			if (this.pilotLevel < 5) {
				this.pilotLevel += 1;
			}
		}

		if (skill == "engineer") {
			if (this.engineerLevel < 5) {
				this.engineerLevel += 1;
			}
		}

		if (skill == "science") {
			if (this.scienceLevel < 5) {
				this.scienceLevel += 1;
			}
		}

		if (skill == "navigation") {
			if (this.navigationLevel < 5) {
				this.navigationLevel += 1;
			}
		}

		if (skill == "personalCombat") {
			if (this.personalCombatLevel < 5) {
				this.personalCombatLevel += 1;
			}
		}
	}

}
