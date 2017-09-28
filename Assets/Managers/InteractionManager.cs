using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour {

	private static InteractionManager instance;

	CrewManager crewManager;
	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
		} else {
			Destroy (gameObject);
		}

	}

	void Start () {
		crewManager = CrewManager.instance;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp (0)) {
			RaycastHit2D hit = Physics2D.Raycast (new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y), Vector2.zero);
			if (crewManager.selectedCrew != null) {
				crewManager.selectedCrew.setSelect(false);
				crewManager.selectedCrew = null;
			}
			if (hit.collider != null) {
				Person p = hit.collider.gameObject.GetComponent<Person> ();
				if (p != null) {
					p.setSelect(true);
					crewManager.selectedCrew = p;
				}
			}
			
		} else if (Input.GetMouseButtonUp (1)) {
			Vector3 point = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			Vector3 loc = new Vector3 (Mathf.Round (point.x), Mathf.Round (point.y), 0f) - Ship.playerShip.transform.position;
			if (Ship.playerShip.map.isWithinBounds (loc)) {
				if ((Ship.playerShip.map.isInShip ((int)loc.x, (int)loc.y)) && (Ship.playerShip.map.isPassable ((int)loc.x, (int)loc.y))) {
					Job j = new Job ();
					j.Location = Ship.playerShip.map.onFloor (loc);
					j.AssignerLocation = j.Location;
					j.desc = "Manual move to";
					j.priority = 1;
					crewManager.addNewJob (j);
				}
			}
		}
	}
}
