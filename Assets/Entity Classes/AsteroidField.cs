﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidField : MonoBehaviour {
	public float height = 0;
	public float length = 0;
	public float spawnChance = 0.3f;
	public float speed;
	public float rotspeed;
	public GameObject[] asteroids;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Random.value < spawnChance * Time.deltaTime) {
			GameObject astr = Instantiate(asteroids [Random.Range (0, asteroids.Length)]);
			astr.transform.position = new Vector3 (transform.position.x, transform.position.y + (Random.value - 0.5f) * height, 0.1f);
			float scale = Random.value + 0.5f;
			astr.transform.localScale = new Vector3(scale, scale, 1f);
			astr.GetComponent<Asteroid> ().endPosition = -100;
			astr.GetComponent<Asteroid> ().speed = speed + Random.value * 2;
			astr.GetComponent<Asteroid> ().rotSpeed = rotspeed * (Random.value - 0.5f);
		}
	}
}
