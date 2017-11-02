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
	public Vector2 impassibleoffset = Vector2.zero;
	public Vector2 impassibleextent = Vector2.zero;
	CrewManager cm;

	public void Start() {
		cm = CrewManager.instance;
		health = maxhealth;
	}

	public void changeHealth(float amount) {
		health += amount;
		GetComponent<SpriteRenderer> ().color = new Color(1f, 1f, 1f, health / maxhealth);
		if (fixjob == null) {
			fixjob = new Job ();
			fixjob.priority = 1;
			fixjob.Location = Ship.playerShip.map.nearestInsideFloorSpot (transform.localPosition);
			fixjob.AssignerLocation = transform.localPosition;
			fixjob.desc = "Fix block at";
			fixjob.onComplete = repairComplete;
			fixjob.duration = (1 - health / maxhealth) * size.x * size.y;
			fixjob.tool = Job.WELD;
			cm.addNewJob (fixjob);		
		} else {
			fixjob.duration = (1 - health / maxhealth) * size.x * size.y;
		}
	}

	public void repairComplete() {
		health = maxhealth;
		GetComponent<SpriteRenderer> ().color = new Color(1f, 1f, 1f, health / maxhealth);
		fixjob = null;
	}
}
