using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnviromentManager: MonoBehaviour {

	public static EnviromentManager instance;

	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
		} else {
			Destroy (gameObject);
		}
	}

	
	// Update is called once per frame
	void Update () {
		
	}

}
