using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : Entity {

	public static int PASSIVE = 0;
	public static int POPUP = 1;
	public int eventType = -1;
	protected EnviromentManager env;
	// Use this for initialization
	virtual protected void Start () {
		env = EnviromentManager.instance;
	}
	
	// Update is called once per frame
	void Update () {

	}
}
