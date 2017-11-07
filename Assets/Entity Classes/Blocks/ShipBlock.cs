using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBlock: Entity  {
	public int id;
	public bool passable = false;
	public Vector2 size = new Vector2(1,1);
	public float maxhealth = 100;
	public float health;

	public void Start() {
		health = maxhealth;
		changeHealth (0);
	}

	public void changeHealth(float amount) {
		health += amount;
		GetComponent<SpriteRenderer> ().color = new Color(1f, 1f, 1f, health / maxhealth);
	}
}
