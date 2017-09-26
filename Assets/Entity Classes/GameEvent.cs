using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : Entity {

	public float duration;
	EnviromentManager env;
	// Use this for initialization
	void Start () {
		env = EnviromentManager.instance;
	}
	
	// Update is called once per frame
	void Update () {
		duration -= Time.deltaTime;
		if (duration < 0) {
			env.endEvent ();
			Destroy (gameObject);
		}
	}
}
