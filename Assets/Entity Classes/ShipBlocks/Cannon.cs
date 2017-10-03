﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {
	Ship playerShip;
	public GameObject barrel;
	public GameObject firepoint;
	float rottarget;
	public float rotspeed;
	public float minangle = 0;
	public float maxangle = 180;
	public float blockedmedian = 270;
	public float fireinterval = 1f;
	bool validtarget = false;
	bool ontarget = false;
	Console console;

	public GameObject bullet;
	public float barrellength = 10f;
	float firetimer;
	// Use this for initialization
	void Start () {
		playerShip = Ship.playerShip;
		playerShip.addCannon (this);
		rottarget = barrel.transform.rotation.eulerAngles.z;
		firetimer = 0f;
		console = GetComponent<Console> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (console.manned) {
			float distancetogo = rottarget - barrel.transform.rotation.eulerAngles.z;
			if (distancetogo > 180) {
				distancetogo -= 360;
			} else if (distancetogo < -180) {
				distancetogo += 360;
			}  
			// go in the correct direction if either will do
			if (Mathf.Abs (distancetogo) == 180) {
				barrel.transform.Rotate (new Vector3 (0f, 0f, rotspeed * Time.deltaTime * Mathf.Sign (distancetogo) * Mathf.Sign (blockedmedian - 180)));
				ontarget = false;
			} else if (Mathf.Abs (distancetogo) < rotspeed * Time.deltaTime) {
				barrel.transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, rottarget));
				ontarget = true;
			} else {
				barrel.transform.Rotate (new Vector3 (0f, 0f, rotspeed * Time.deltaTime * distancetogo / Mathf.Abs (distancetogo)));
				ontarget = false;
			}
			firetimer -= Time.deltaTime;
		}
	}

	public void target (Vector3 tar){
		Vector3 rel = tar - barrel.transform.position;
		rottarget = (Mathf.Atan2 (rel.y, rel.x) / (2f * Mathf.PI)) * 360f;
		validtarget = true;
		float me = magDist (rottarget, blockedmedian);
		float mi = magDist (rottarget, minangle);
		float ma = magDist (rottarget, maxangle);
		if(((me < 90) || (me > 270)) && ((mi < 90) || (mi > 270))) {
			rottarget = minangle;
			validtarget = false;
		}else if(((me < 90) || (me > 270)) && ((ma < 90) || (ma > 270))) {
			rottarget = maxangle;
			validtarget = false;

		}
	}

	private float magDist(float a1, float a2) {
		float r = Mathf.Abs (a1 - a2);
		while (r > 360) {
			r -= 360;
		}
		return r;
	}

	public void fire(){
		if (console.manned && ontarget && validtarget && firetimer < 0f) {
			GameObject b = Instantiate (bullet);
			b.transform.rotation = barrel.transform.rotation;
			b.transform.position = firepoint.transform.position;
			firetimer = fireinterval;
		}
	}

}
