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
			Vector3 point = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			Vector3 loc = new Vector3 (Mathf.Round (point.x), Mathf.Round (point.y), 0f);
			if (Ship.playerShip.map.isWithinBounds (loc)) {
				if (Ship.playerShip.map.isPassible ((int)loc.x, (int)loc.y)) {
					Job j = new Job ();
					j.Location = Ship.playerShip.map.onFloor (loc);
					crewManager.addNewJob (j);
				}
			}
		}
	}
}
