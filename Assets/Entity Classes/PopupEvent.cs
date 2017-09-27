using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupEvent : GameEvent {

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}

	protected void OnEnable() {
		eventType = GameEvent.POPUP;
	}

	// Update is called once per frame
	void Update () {

	}
}
