using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {
	Ship playerShip;
	public GameObject barrel;
	// Use this for initialization
	void Start () {
		playerShip = Ship.playerShip;
		playerShip.addCannon (this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void target (Vector3 tar){
		Vector3 rel = tar - barrel.transform.position;
		float angle = (Mathf.Atan2 (rel.y, rel.x) / (2f * Mathf.PI)) * 360f;
		Debug.Log (angle);
		barrel.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
	}
}
