using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotConsole : MonoBehaviour {
	Console console;
	public GameObject anim;
	// Use this for initialization
	void Start () {
		console = GetComponent<Console> ();
	}
	
	// Update is called once per frame
	void Update () {
		anim.SetActive (console.manned);
	}
}
