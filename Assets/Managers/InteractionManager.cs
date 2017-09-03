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
		if (Input.GetMouseButtonUp(0)) {
			Vector3 point = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			Job j = new Job ();
			j.Location = new Vector3( Mathf.Round (point.x), -2.5f, 0f);
			crewManager.addNewJob (j);
		}
	}
}
