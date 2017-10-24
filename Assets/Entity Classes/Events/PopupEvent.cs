using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupEvent : MonoBehaviour {

	// Use this for initialization
	protected void Start () {
		Time.timeScale = 0;
	}


	public void endEvent() {
		Time.timeScale = 1;
		Destroy (gameObject);
	}

	// Update is called once per frame
	void Update () {

	}
}
