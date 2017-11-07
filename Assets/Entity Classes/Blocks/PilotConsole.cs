using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotConsole : MonoBehaviour {
	public Console console;
	public GameObject anim;
	// Use this for initialization
	void Start () {
		console = GetComponent<Console> ();
		Ship.playerShip.addPilotConsole (this);
	}
	
	// Update is called once per frame
	void Update () {
		anim.SetActive (console.manned);
	}
}
