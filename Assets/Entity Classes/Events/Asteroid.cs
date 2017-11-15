using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : Entity {
	public float speed;
	public float rotSpeed;
	public float endPosition;
	public float health;
	public GameObject destroy;
	public GameObject dodged;
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
	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.GetComponent<CannonRoundController> () != null) {
			health -= 20;
			if (health < 0) {
				GameObject d = Instantiate (destroy);
				d.transform.position = transform.position;
				d.transform.localScale = transform.localScale;
				Destroy (gameObject);
			}
			Destroy (coll.gameObject);
		}else if (coll.gameObject.GetComponent<Ship> () != null && coll.gameObject.GetComponent<Ship> () == Ship.playerShip) {
			if (Ship.playerShip.checkDodge ()) {
				Destroy (GetComponent<CircleCollider2D> ());
				GameObject d = Instantiate (dodged);
				d.transform.position = new Vector3 (transform.position.x, transform.position.y, -3f);
			} else {
				Ship.playerShip.causeDamage (coll.contacts [0].point, 15 * transform.localScale.x, 3 * transform.localScale.x);
				GameObject d = Instantiate (destroy);
				d.transform.position = transform.position;
				d.transform.localScale = transform.localScale;
				Destroy (gameObject);
			}
		}
	}
}
