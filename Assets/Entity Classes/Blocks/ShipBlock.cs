using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBlock: Entity  {
	public int id;
	public bool passable = false;
	public Vector2 size = new Vector2(1,1);
	public float maxhealth = 100;
	public float health;
	private Job fixjob = null;
	CrewManager cm;

	public void Start() {
		cm = CrewManager.instance;
		health = maxhealth;
		changeHealth (0);
	}

	public void changeHealth(float amount) {
		health += amount;
		GetComponent<SpriteRenderer> ().color = new Color(1f, 1f, 1f, health / maxhealth);
		if (fixjob == null) {
			fixjob = new Job ();
			Debug.Log ("a " + transform.localPosition);
			fixjob.priority = 1;
			fixjob.Location = Ship.playerShip.map.nearestInsideFloorSpot (transform.localPosition);
			Debug.Log ("r "+fixjob.Location);
			fixjob.AssignerLocation = transform.localPosition;
			fixjob.desc = "Fix block at";
			fixjob.onComplete = repairComplete;
			cm.addNewJob (fixjob);		
		}
	}

	public void repairComplete() {
		health = maxhealth;
		GetComponent<SpriteRenderer> ().color = new Color(1f, 1f, 1f, health / maxhealth);
		fixjob = null;
	}
}
