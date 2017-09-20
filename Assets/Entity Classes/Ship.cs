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
		map = new ShipMap (70, 30);
		ConstructShip ("TestShip");
		//map.printMap ();
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
				if (s!=null && s!=""){
					int i = Convert.ToInt32 (s);
					if (i >= 0) {
						GameObject block = Blocks [i];
						Vector2 size = block.GetComponent<ShipBlock> ().size;
						for (int ix = x; ix < x+size.x; ix++) {
							for (int iy = y; iy < y+size.y; iy++) {
								if (i == ladderID) {
									map.setThing (ix, iy, ShipMap.ladder);
								} else if (block.GetComponent<ShipBlock> ().passable) {
									map.setThing (ix, iy, ShipMap.passable);
								} else {
									map.setThing (ix, iy, ShipMap.impassible);
								}
							}
						}
						GameObject thisBlock = Instantiate (block);
						thisBlock.transform.SetParent (transform);
						thisBlock.transform.position = new Vector3 (x * 1f, y * 1f, -1f*0.00001f*i);
					}
				}
				x++;
			}
			y--;
		}	
	}
}
