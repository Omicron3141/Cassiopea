using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonRoundController : MonoBehaviour {
	public float speed;
	public float range;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (speed * Time.deltaTime, 0f, 0f, Space.Self);
		range -= speed * Time.deltaTime;
		if (range < 0) {
			Destroy (gameObject);
		}
	}
		
}
