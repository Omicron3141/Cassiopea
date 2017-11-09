using System.Collections;
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
	public float fireinterval = 2f;
	bool validtarget = false;
	bool ontarget = false;
	Console console;
	public GameObject anim;

	public GameObject bullet;
	public float barrellength = 10f;
	Vector3 barrelpos;
	float firetimer;
	// Use this for initialization
	void Start () {
		playerShip = Ship.playerShip;
		playerShip.addCannon (this);
		rottarget = barrel.transform.rotation.eulerAngles.z;
		firetimer = 0f;
		console = GetComponent<Console> ();
		barrelpos = barrel.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		anim.SetActive (console.manned);
		if (console.manned && (Time.deltaTime > 0)) {
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
		if (barrel.transform.position != barrelpos) {
			if ((barrel.transform.position - barrelpos).magnitude < 10f*Time.deltaTime) {
				barrel.transform.position = barrelpos;
			} else {
				barrel.transform.Translate (Time.deltaTime, 0f, 0f, Space.Self);
			}
		}
	}

	public void target (Vector3 tar){
		Vector3 rel = tar - barrel.transform.position;
		rottarget = (Mathf.Atan2 (rel.y, rel.x) / (2f * Mathf.PI)) * 360f;
		validtarget = true;
		float me = magDist (rottarget, blockedmedian);
		float mi = magDist (rottarget, minangle);
		float ma = magDist (rottarget, maxangle);
		if (isBetween(minangle, blockedmedian, rottarget)) {
			rottarget = minangle;
			validtarget = false;
		} else if (isBetween(maxangle, blockedmedian, rottarget)) {
			rottarget = maxangle;
			validtarget = false;
		}
	}

	private float magDist(float a1, float a2) {
		if (a1 < 0) {
			a1 += 360;
		}
		if (a2 < 0) {
			a2 += 360;
		}
		float r = Mathf.Abs (a1 - a2);
		while (r > 360) {
			r -= 360;
		}
		if (r > 180) {
			r = 360 - r;
		}
		return r;
	}

	private bool isBetween(float a1, float a2, float target) {
		float endpointdist = magDist (a1, a2);
		return ((magDist (a1, target) <= endpointdist) && (magDist (a2, target) <= endpointdist));
	}

	public void fire(){
		if (console.manned && ontarget && validtarget && firetimer < 0f) {
			GameObject b = Instantiate (bullet);
			b.transform.rotation = barrel.transform.rotation;
			b.transform.position = firepoint.transform.position;
			firetimer = fireinterval / console.mannedskill;
			barrel.transform.position = barrelpos;
			barrel.transform.Translate (-0.5f, 0f, 0f, Space.Self);
		}
	}

}
