using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : Entity {
	public float speed;
	public float rotSpeed;
	public float endPosition;
	public float health;
	public GameObject destroy;
	// Use this for initialization
	void Start () {
		rotSpeed *= (int)(Mathf.Round (Random.value * 2 - 1));
	}
	
	// Update is called once per frame
	void Update () {
		transform.position -= new Vector3 (speed * Time.deltaTime, 0f, 0f);
		transform.Rotate (0f, 0f, rotSpeed * Time.deltaTime);
		if (transform.position.x < endPosition) {
			Destroy (gameObject);
		}
	}

	// hit by something
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.GetComponent<CannonRoundController> () != null) {
			health -= 20;
			if (health < 0) {
				GameObject d = Instantiate (destroy);
				d.transform.position = transform.position;
				d.transform.localScale = transform.localScale;
				Destroy (gameObject);
			}
			Destroy (other.gameObject);
		}
	}
}
