using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {
	Ship playerShip;
	public GameObject barrel;
	float rottarget;
	public float rotspeed;
	public float minangle = 0;
	public float maxangle = 180;
	public float blockedmedian = 270;
	// Use this for initialization
	void Start () {
		playerShip = Ship.playerShip;
		playerShip.addCannon (this);
		rottarget = barrel.transform.rotation.eulerAngles.z;
	}
	
	// Update is called once per frame
	void Update () {
		float distancetogo = rottarget - barrel.transform.rotation.eulerAngles.z;
		if (Mathf.Abs(distancetogo) > 180) {
			distancetogo -= 360;
		}
		if (Mathf.Abs (distancetogo) < rotspeed * Time.deltaTime) {
			barrel.transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, rottarget));
		} else {
			barrel.transform.Rotate (new Vector3 (0f, 0f, rotspeed * Time.deltaTime * distancetogo / Mathf.Abs (distancetogo)));
		}
	}

	public void target (Vector3 tar){
		Vector3 rel = tar - barrel.transform.position;
		rottarget = (Mathf.Atan2 (rel.y, rel.x) / (2f * Mathf.PI)) * 360f;
		rottarget = clampAngle(rottarget, minangle, maxangle, blockedmedian);
	}

	public static float clampAngle(float angle, float min, float max, float blockedmed) {
		if(((Mathf.Abs(angle-blockedmed) < 90) || (Mathf.Abs(angle-blockedmed) > 270)) && ((Mathf.Abs(angle-min) < 90) || (Mathf.Abs(angle-min) > 270))) {
			return min;
		}else if(((Mathf.Abs(angle-blockedmed) < 90) || (Mathf.Abs(angle-blockedmed) > 270)) && ((Mathf.Abs(angle-max) < 90) || (Mathf.Abs(angle-max) > 270))) {
			return max;
		}
		return angle;
	}

}
