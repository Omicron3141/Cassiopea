using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveEvent : GameEvent {
	public float duration;
	public float destroyDelay;
	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}

	protected void OnEnable() {
		eventType = GameEvent.PASSIVE;
	}
	
	// Update is called once per frame
	void Update () {
		duration -= Time.deltaTime;
		if (duration < 0) {
			env.endEvent ();
			Destroy (gameObject, destroyDelay);
		}
	}
}
