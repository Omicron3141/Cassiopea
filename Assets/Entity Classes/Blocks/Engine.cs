using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour {
	public float normalthrust;
	public float brokenthrust;
	ParticleSystem.EmissionModule sys;
	public Console cons;
	// Use this for initialization
	void Start () {
		sys = transform.Find("Plume").GetComponent<ParticleSystem> ().emission;
		cons = GetComponent<Console> ();
		Ship.playerShip.addEngine (this);
	}
	
	// Update is called once per frame
	void Update () {
		if (cons.broken) {
			ParticleSystem.MinMaxCurve curve = sys.rateOverTime;
			curve.constant = brokenthrust;
			sys.rateOverTime = curve;
		} else {
			ParticleSystem.MinMaxCurve curve = sys.rateOverTime;
			curve.constant = normalthrust;
			sys.rateOverTime = curve;
		}
	}
}
