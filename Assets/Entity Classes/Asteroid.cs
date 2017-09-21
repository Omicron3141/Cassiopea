using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : Entity {
	public float speed;
	public float rotSpeed;
	// Use this for initialization
	void Start () {
		rotSpeed *= (int)(Mathf.Round (Random.value * 2 - 1));
	}
	
	// Update is called once per frame
	void Update () {
		transform.position -= new Vector3 (speed * Time.deltaTime, 0f, 0f);
		transform.Rotate (0f, 0f, rotSpeed * Time.deltaTime);
	}
}
