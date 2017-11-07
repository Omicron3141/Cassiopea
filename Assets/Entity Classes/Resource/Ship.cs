using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ship: SpaceObject  {
	public static Ship playerShip;
	public bool isPlayerShip = false;
	public GameObject[] Blocks;
	public List<ShipBlock> shipBlocks;
	public float bestDodgeChance = 0.5f;
	//the Id of the ladder block for pathfinding
	int ladderID = 7;

	public ShipMap map;
	public Oxygen oxygen = new Oxygen();
	public Food food = new Food(100);
	public Metal metal = new Metal(100);
	public Fuel fuel = new Fuel(100);
	public Water water = new Water(100);
	public Text oxygenDisplay;
	public Text metalDisplay;
	public Text foodDisplay;
	public Text fuelDisplay;
	//public Text waterDisplay;

	public List<Cannon> cannons;
	public List<Engine> engines;
	public List<PilotConsole> pilotconsoles;


	void Awake () {
		if (playerShip == null && isPlayerShip) {
			playerShip = this;
		}
		map = new ShipMap (70, 30);
		cannons = new List<Cannon> ();
		ConstructShip ("TestShip");
		//map.printMap ();
	}


	void Update () {
		updateOxygenDisplay ();
		updateFoodDisplay ();
		updateMetalDisplay ();
		updateFuelDisplay ();
		//updateWaterDisplay ();
	}


	void ConstructShip(string filename) {
		TextAsset file = Resources.Load (filename) as TextAsset;
		string[] lines = file.text.Split ("\n" [0]);
		int height = lines.Length;
		int y = height;
		shipBlocks = new List<ShipBlock> ();
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
									float xoff = ix - x - block.GetComponent<ShipBlock> ().impassibleoffset.x;
									float yoff = iy - y - block.GetComponent<ShipBlock> ().impassibleoffset.y;
									if ((xoff < block.GetComponent<ShipBlock> ().impassibleextent.x) && (xoff >= 0) && (yoff < block.GetComponent<ShipBlock> ().impassibleextent.y) && (yoff >= 0)) {
										map.setThing (ix, iy, ShipMap.impassible);
									} else {
										map.setThing (ix, iy, ShipMap.passable);
									}
								} else {
									map.setThing (ix, iy, ShipMap.impassible);
								}
							}
						}
						GameObject thisBlock = Instantiate (block);
						thisBlock.transform.SetParent (transform);
						thisBlock.transform.localPosition = new Vector3 (x * 1f, y * 1f, 1f*0.00001f*i);
						shipBlocks.Add(thisBlock.GetComponent<ShipBlock>());
					}
				}
				x++;
			}
			y--;
		}
		EdgeCollider2D collider = gameObject.AddComponent<EdgeCollider2D>();
		collider.points = map.generateTrace ().ToArray ();
	}

	private void updateOxygenDisplay() {
		string text = this.oxygen.percentage.ToString () + "%";
		oxygenDisplay.text = text;
	}

	private void updateFoodDisplay() {
		string text = this.food.currentFood.ToString () + " / " + this.food.maximumFoodStorage.ToString ();
		foodDisplay.text = text;
	}

	private void updateMetalDisplay() {
		string text = this.metal.amount.ToString () + " / " + this.metal.maxAmount.ToString ();
		metalDisplay.text = text;
	}

	private void updateFuelDisplay() {
		string text = this.fuel.amount.ToString () + " / " + this.fuel.maximumFuelStorage.ToString ();
		fuelDisplay.text = text;
	}

	//private void updateWaterDisplay() {
		//string text = this.water.amount.ToString () + " / " + this.water.maxAmount.ToString ();
		//waterDisplay.text = text;
	//}

	public bool isWithinShip(Vector3 pos) {
		return map.isWithinBounds (pos);
	}

	public void addCannon(Cannon c){
		cannons.Add (c);
	}

	public void addEngine(Engine e){
		engines.Add (e);
	}

	public void addPilotConsole(PilotConsole p){
		pilotconsoles.Add (p);
	}

	public void target(Vector3 target) {
		for (int i = 0; i < cannons.Count; i++) {
			cannons [i].target (target);
		}
	}

	public void fire() {
		for (int i = 0; i < cannons.Count; i++) {
			cannons [i].fire();
		}
	}

	public void causeDamage (Vector2 location, float damage, float radius) {
		location = new Vector2 ((int)location.x, (int)location.y);
		foreach (ShipBlock b in shipBlocks) {
			float dist = 0;
			Vector2 pos = new Vector2 (b.gameObject.transform.position.x, b.gameObject.transform.position.y);
			if (b.size.sqrMagnitude > 1) {
				dist = (pos + b.size / 2 - location).magnitude;
				dist -= b.size.magnitude / 2;
			} else {
				dist = (pos - location).magnitude;
			}
			if (dist < radius) {
				b.changeHealth (-1 * damage * (radius - dist) / radius);
			}
		}
	}

	public bool checkDodge () {
		float chance = bestDodgeChance;
		foreach (Engine e in engines) {
			if (e.cons.broken) {
				chance -= 0.1f;
			}
		}
		foreach (PilotConsole p in pilotconsoles) {
			if (!p.console.manned) {
				chance -= 0.3f;
			}
		}
		return (UnityEngine.Random.value < chance);
	}
}