using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Skills {WEAPONS, PILOTING, ENGINEERING, SCIENCE, NAVIGATION, PERSONALCOMBAT};
public enum Roles {PILOT, GUNNER, ENGINEER};

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
	private bool DoingJob{ get{ return state != State.IDLE; } }
	// a path is a queue of waypoints
	private Queue<Vector3> path;
	// the current waypoint
	private Vector3 target;

	public string crewName;
	public string age;
	public string profession;
	public int[] skillLevels;
	public List<string> firstNames = new List<string>();
	public List<string> lastNames = new List<string>();
	public List<string> professions = new List<string>();
	public List<string> listOfTraits = new List<string>();
	public Roles role;
	public int maxHealth;
	public int currentHealth;
	public List<string> traits = new List<string>();
	private CrewManager manager;
	SpriteRenderer Body;

	enum State {IDLE, MOVINGTOJOB, DOINGJOB};

	private State state = State.IDLE;


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

		string traitsCurrentLine; // For looping through possible traits.
		System.IO.StreamReader textListOfTraits = new System.IO.StreamReader("traits.txt");

		while ((traitsCurrentLine = textListOfTraits.ReadLine ()) != null) {
			listOfTraits.Add(traitsCurrentLine);
		}

		int randomTraitsIndex = UnityEngine.Random.Range (0, listOfTraits.Count);

		this.crewName = firstNames[randomFirstIndex] + " " + lastNames[randomLastIndex];
		this.profession = professions [randomProfIndex];
		this.age = UnityEngine.Random.Range (25, 70).ToString ();
		this.traits.Add (listOfTraits[randomTraitsIndex]);
		this.maxHealth = 100;
		this.currentHealth = 100;

		skillLevels = new int[6];

		if (this.profession == "Engineering Officer") {
			skillLevels[(int)Skills.PILOTING] = 2;
			skillLevels[(int)Skills.ENGINEERING] = 4;
			skillLevels[(int)Skills.SCIENCE] = 2;
			skillLevels[(int)Skills.NAVIGATION] = 1;
			skillLevels[(int)Skills.WEAPONS] = 2;
			skillLevels[(int)Skills.PERSONALCOMBAT] = 1;
			role = Roles.ENGINEER;
		}

		if (this.profession == "Pilot") {
			skillLevels[(int)Skills.PILOTING] = 4;
			skillLevels[(int)Skills.ENGINEERING] = 2;
			skillLevels[(int)Skills.SCIENCE] = 1;
			skillLevels[(int)Skills.NAVIGATION] = 3;
			skillLevels[(int)Skills.WEAPONS] = 2;
			skillLevels[(int)Skills.PERSONALCOMBAT] = 1;
			role = Roles.PILOT;
		}

		if (this.profession == "Software Engineer") {
			skillLevels[(int)Skills.PILOTING] = 2;
			skillLevels[(int)Skills.ENGINEERING] = 2;
			skillLevels[(int)Skills.SCIENCE] = 3;
			skillLevels[(int)Skills.NAVIGATION] = 3;
			skillLevels[(int)Skills.WEAPONS] = 1;
			skillLevels[(int)Skills.PERSONALCOMBAT] = 1;
			role = Roles.PILOT;
		}

		if (this.profession == "Weapons Specialist") {
			skillLevels[(int)Skills.PILOTING] = 1;
			skillLevels[(int)Skills.ENGINEERING] = 2;
			skillLevels[(int)Skills.SCIENCE] = 1;
			skillLevels[(int)Skills.NAVIGATION] = 1;
			skillLevels[(int)Skills.WEAPONS] = 4;
			skillLevels[(int)Skills.PERSONALCOMBAT] = 3;
			role = Roles.GUNNER;
		}

		if (this.profession == "Security Officer") {
			skillLevels[(int)Skills.PILOTING] = 1;
			skillLevels[(int)Skills.ENGINEERING] = 1;
			skillLevels[(int)Skills.SCIENCE] = 1;
			skillLevels[(int)Skills.NAVIGATION] = 1;
			skillLevels[(int)Skills.WEAPONS] = 3;
			skillLevels[(int)Skills.PERSONALCOMBAT] = 4;
			role = Roles.GUNNER;
		}

		if (this.profession == "Navigation Officer") {
			skillLevels[(int)Skills.PILOTING] = 3;
			skillLevels[(int)Skills.ENGINEERING] = 1;
			skillLevels[(int)Skills.SCIENCE] = 2;
			skillLevels[(int)Skills.NAVIGATION] = 4;
			skillLevels[(int)Skills.WEAPONS] = 2;
			skillLevels[(int)Skills.PERSONALCOMBAT] = 1;
			role = Roles.PILOT;
		}

		if (this.profession == "Captain") {
			skillLevels[(int)Skills.PILOTING] = 2;
			skillLevels[(int)Skills.ENGINEERING] = 2;
			skillLevels[(int)Skills.SCIENCE] = 2;
			skillLevels[(int)Skills.NAVIGATION] = 3;
			skillLevels[(int)Skills.WEAPONS] = 2;
			skillLevels[(int)Skills.PERSONALCOMBAT] = 2;
			role = Roles.ENGINEER;
		}

		if (this.profession == "Science Officer") {
			skillLevels[(int)Skills.PILOTING] = 1;
			skillLevels[(int)Skills.ENGINEERING] = 3;
			skillLevels[(int)Skills.SCIENCE] = 4;
			skillLevels[(int)Skills.NAVIGATION] = 2;
			skillLevels[(int)Skills.WEAPONS] = 1;
			skillLevels[(int)Skills.PERSONALCOMBAT] = 1;
			role = Roles.ENGINEER;
		}

		if (this.traits.Contains("Book Smart")) {
			this.skillLevels[(int)Skills.SCIENCE] += 1;
		}

		if (this.traits.Contains("Deadeye")) {
			this.skillLevels[(int)Skills.PERSONALCOMBAT] += 1;
		}

		if (this.traits.Contains ("Quick Loader")) {
			this.skillLevels [(int)Skills.WEAPONS] += 1;
		}

		if (this.traits.Contains ("Ace")) {
			this.skillLevels [(int)Skills.PILOTING] += 1;
		}

		if (this.traits.Contains ("Astrogation Expert")) {
			this.skillLevels [(int)Skills.NAVIGATION] += 1;
		}

		if (this.traits.Contains ("Mechanically Inclined")) {
			this.skillLevels [(int)Skills.ENGINEERING] += 1;
		}

		if (this.traits.Contains ("Hardy")) {
			this.maxHealth = 120;
			this.currentHealth = 120;
		}

		if (this.traits.Contains ("Fleet of Foot")) {
			this.speed *= 1.2;
		}
			
		// start idling
		target = Ship.playerShip.map.getNewWanderTarget (transform.localPosition);
	}


	void Update () {

		if (currentJob == null && state != State.IDLE) {
			state = State.IDLE;
		}
		if (state == State.IDLE) {
			// are we basically at our target?
			if (Mathf.Abs ((target - transform.localPosition).magnitude) < 2f * speed * Time.deltaTime) {
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
		} else if (state == State.MOVINGTOJOB) {
			// if we are basically at our target
			if (Mathf.Abs ((target - transform.localPosition).magnitude) < 2f * speed * Time.deltaTime) {
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
						currentJob.onStart.Invoke (skillLevels[(int)currentJob.requiredSkill].ToString());
					}
					state = State.DOINGJOB;

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
		} else if (state == State.DOINGJOB) {
			// if the job is not permenant
			if (!currentJob.permenant) {
				// decrease the time remaining
				float ratemod = 1;
				if (currentJob.requiredSkill != null) {
					ratemod = skillLevels [(int)currentJob.requiredSkill];
				}
				currentJob.duration -= (Time.deltaTime * ratemod);
				// if we're done
				if (currentJob.duration < 0) {
					if (currentJob.onComplete != null) {
						currentJob.onComplete.Invoke ();
					}
					manager.UnassignJob (this, currentJob);
					currentJob = null;
					state = State.IDLE;
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
		state = State.MOVINGTOJOB;
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
		if (state == State.IDLE) {
			return "Idle";
		} else if (state == State.MOVINGTOJOB) {
			return "Moving to job:\n  \"" + currentJob.Description() + "\"";
		} else if (state == State.DOINGJOB) {
			return "Doing job \n  \"" + currentJob.Description() + "\"";
		} else {
			return "N/A";
		}
	}

	public void improveSkill(string skill) {
		if (skill == "weapons") {
			if (this.skillLevels[(int)Skills.WEAPONS] < 5) {
				this.skillLevels[(int)Skills.WEAPONS] += 1;
			}
		}

		if (skill == "pilot") {
			if (this.skillLevels[(int)Skills.PILOTING] < 5) {
				this.skillLevels[(int)Skills.PILOTING] += 1;
			}
		}

		if (skill == "engineer") {
			if (this.skillLevels[(int)Skills.ENGINEERING] < 5) {
				this.skillLevels[(int)Skills.ENGINEERING] += 1;
			}
		}

		if (skill == "science") {
			if (this.skillLevels[(int)Skills.SCIENCE] < 5) {
				this.skillLevels[(int)Skills.SCIENCE] += 1;
			}
		}

		if (skill == "navigation") {
			if (this.skillLevels[(int)Skills.NAVIGATION] < 5) {
				this.skillLevels[(int)Skills.NAVIGATION] += 1;
			}
		}

		if (skill == "personalCombat") {
			if (this.skillLevels[(int)Skills.PERSONALCOMBAT] < 5) {
				this.skillLevels[(int)Skills.PERSONALCOMBAT] += 1;
			}
		}
	}

}
