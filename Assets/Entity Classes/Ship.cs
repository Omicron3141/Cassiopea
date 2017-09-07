using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Ship: SpaceObject  {
	public static Ship playerShip;
	public bool isPlayerShip = false;
	public GameObject[] Blocks;
	//the Id of the ladder block for pathfinding
	int ladderID = 7;

	public ShipMap map;

	void Awake () {
		if (playerShip == null && isPlayerShip) {
			playerShip = this;
		}
		map = new ShipMap (60, 30);
		ConstructShip ("TestShip");
	}


	void Update () {

	}


	void ConstructShip(string filename) {
		TextAsset file = Resources.Load (filename) as TextAsset;
		string[] lines = file.text.Split ("\n" [0]);
		int height = lines.Length;
		int y = height;
		foreach (string l in lines) {
			string line = l.Trim ();
			string[] columns = line.Split ("," [0]);
			int x = 0;
			foreach (string s in columns) {
				if (s!=null){
					int i = Convert.ToInt32 (s);
					if (i >= 0) {
						if (i == ladderID) {
							map.setThing (x, y, ShipMap.ladder);
						} else if (Blocks [i].GetComponent<Block> ().passable) {
							map.setThing (x, y, ShipMap.passable);
						} else {
							map.setThing (x, y, ShipMap.impassible);
						}
						GameObject block = Instantiate (Blocks [i]);
						block.transform.SetParent (transform);
						block.transform.position = new Vector3 (x * 1f, y * 1f, 0f);
					}
				}
				x++;
			}
			y--;
		}	
	}
}
