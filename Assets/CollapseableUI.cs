using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollapseableUI : MonoBehaviour {
	public Vector3 fullsize;
	public Vector3 smallsize;
	private bool collapsed = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void swap() {
		collapsed = !collapsed;
		if (collapsed) {
			GetComponent<RectTransform> ().sizeDelta = smallsize;
		} else {
			GetComponent<RectTransform> ().sizeDelta = fullsize;
		}
	}
}
