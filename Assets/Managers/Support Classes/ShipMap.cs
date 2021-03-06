﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMap{
	int[,] map;
	int width;
	int height;
	// the actual width and height taken up by stuff
	int actualW;
	int actualH;
	public const int ladder = 2;
	public const int passable = 0;
	public const int impassible = 1;

	public ShipMap(int w, int h) {
		width = w;
		height = h;
		actualH = 0;
		actualW = 0;
		map = new int[width, height];
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				map [x, y] = -1;
			}
		}
	}


	// get the thing at
	public int thingAt(int x, int y) {
		if ((x < width) && (y < height) && (x>=0) && (y>=0)) {
			return map [x, y];
		} else {
			//Debug.LogError ("Tried to access out-of-bounds map index at "+x+","+y);
			return -1;
		}
	}

	// set the thing at
	public void setThing(int x, int y, int thing) {
		if ((x < width) && (y < height) && (x>=0) && (y>=0)) {
			map [x, y] = thing;
			if (x > actualW) {
				actualW = x;
			}
			if (y > actualH) {
				actualH = y;
			}
		} else {
			Debug.LogError ("Tried to access out-of-bounds map index at "+x+","+y);
		}
	}

	// are we within the bounds we're keeping track of
	public bool isWithinBounds(Vector3 pos) {
		return (((int)pos.x < actualW) && ((int)pos.y < actualH));
	}

	// is this inside the ship proper
	public bool isInShip (int x, int y) {
		return (thingAt (x, y) >= 0);
	}

	// is this inside the ship proper
	public bool isInShip (Vector2 pos) {
		return isInShip ((int)pos.x, (int)pos.y);
	}

	// can crew people walk here
	public bool isPassable (int x, int y) {
		return (thingAt (x, y) == passable);
	}
	// can crew people walk here
	public bool isPassable (Vector2 pos) {
		return isPassable ((int)pos.x, (int)pos.y);
	}

	// is the thing here a ladder
	public bool isLadder (int x, int y) {
		return (thingAt (x, y) == ladder);
	}

	// can crew people walk here
	public bool isLadder (Vector2 pos) {
		return isLadder ((int)pos.x, (int)pos.y);
	}

	// takes in a point and follows it down to the floor below that place
	public Vector3 onFloor(Vector3 pos) {
		float x = pos.x;
		float y = pos.y;
		while ((isPassable((int)x, (int)(y-1))) && (isInShip((int)x, (int)(y-1)))){
			y--;
		}
		return new Vector3 (x, y, pos.z);
	}

	public Vector3 onFloor (Vector2 pos) {
		return onFloor (new Vector3 (pos.x, pos.y, 0f));
	}


	// Finds a path from the specified startpoint to endpoints. Returns a queue of waypoints
	public Queue<Vector3> pathfind(Vector3 startpos, Vector3 endpos) {
		Queue<Vector3> path = new Queue<Vector3>();

		// If the target is on another level
		if (endpos.y == startpos.y) {
			path.Enqueue (endpos);
		} else {
			int direction = (int)(endpos.x - startpos.x);
			if (direction == 0 ){
				direction = 1;
				// if we are right under our target, assume a direction of travel
			}
			Vector3 nearestLadderInDirectionOfTravel = nearestLadder (startpos, direction);
			Vector3 nearestLadderInOppositeDirectionOfTravel = nearestLadder (startpos, -1*direction);
			Vector3 ladderbase;
			if (nearestLadderInOppositeDirectionOfTravel == Vector3.zero) {
				// If there is no ladder in the other direction
				ladderbase = nearestLadderInDirectionOfTravel;
			} else if (nearestLadderInDirectionOfTravel == Vector3.zero) {
				// If there is no ladder in the direction of travel
				ladderbase = nearestLadderInOppositeDirectionOfTravel;
			} else if ((nearestLadderInDirectionOfTravel != Vector3.zero) && (Mathf.Abs(startpos.x - nearestLadderInDirectionOfTravel.x) < Mathf.Abs(startpos.x - endpos.x))){
				// if there is a ladder in the direction of travel and it is closer than the endpoint
				ladderbase = nearestLadderInDirectionOfTravel;
			} else if (pathLength(startpos, nearestLadderInDirectionOfTravel, endpos) < pathLength(startpos, nearestLadderInOppositeDirectionOfTravel, endpos)){
				ladderbase = nearestLadderInDirectionOfTravel;
			} else {
				ladderbase = nearestLadderInOppositeDirectionOfTravel;
			}
			path.Enqueue (ladderbase);
			Vector3 laddertop = new Vector3 (ladderbase.x, endpos.y, ladderbase.z);
			path.Enqueue (laddertop);
			path.Enqueue (endpos);
		}
		return path;

	}

	// return a random place near to the specified position and on the same level
	public Vector3 getNewWanderTarget (Vector3 startpos) {
		int startx = (int)startpos.x;
		int starty = (int)startpos.y;
		int direction = (int)(Mathf.Round (Random.value) * 2 - 1);
		int distance = Random.Range (3, 4);
		for (int i = 1; i < distance; i++) {
			if (!isPassable (startx + direction * i, starty)) {
				distance = i-1;
			}
		}
		//Debug.Log (distance);
		return new Vector3(startx + direction*distance, starty, startpos.z);
	}

	// Finds the length of a path (excluding vertical distance) from statpos to endpos passing through a particular ladder point
	private float pathLength(Vector3 startpos, Vector3 ladderPos, Vector3 endpos) {
		float dist = 0f;
		dist += Mathf.Abs (startpos.x - ladderPos.x);
		dist += Mathf.Abs (ladderPos.x - endpos.x);
		return dist;
	}

	// Returns to position of the nearest ladder in the given x-direction, or Vector3.zero if no ladder exists
	private Vector3 nearestLadder(Vector3 startpos, int direction) {
		int x = (int)startpos.x;
		int y = (int)startpos.y;
		while ((x < width) && (x > 0)) {
			if (thingAt (x, y) != ladder) {
				x += direction / Mathf.Abs (direction);
			} else {
				return new Vector3 (x*1f, y*1f, startpos.z);
			}
		}
		return Vector3.zero;
	}


	public void printMap() {
		string map = "";
		for (int y = height-1; y >= 0; y--) {
			for (int x = 0; x < width; x++) {
				int t = thingAt(x,y);
				if (t == -1) {
					map += "*";
				}else if (t == passable) {
					map += "~";
				}else if (t == impassible) {
					map += "#";
				}else if (t == ladder) {
					map += "=";
				}
			}
			map += "\n";
		}
		Debug.Log(map);
	}

	public List<Vector2> generateTrace() {
		Vector2 direction = Vector2.up;
		Vector2 position = Vector2.zero;
		List<Vector2> list = new List<Vector2> ();
		while (!isInShip (position)) {
			position = position + Vector2.up + Vector2.right;
		}
		list.Add (position);
		int i = 150;
		do {
			if (isInShip (position + direction)) {
				if (!isInShip (position + direction + Left (direction))) {
					position = position + direction;
					list.Add (position);
				} else {
					position = position + direction;
					list.Add (position);
					direction = Left (direction);
				}
			} else {
				direction = Left (Left (Left (direction)));
			}
			//Debug.Log (position + " " + direction);
			i-=1;
		} while (position != list [0] && i>0);
		return list;
	}

	private Vector2 Left(Vector2 dir) {
		if (dir == Vector2.up) {
			return Vector2.left;
		} else if (dir == Vector2.left) {
			return Vector2.down;
		} else if (dir == Vector2.down) {
			return Vector2.right;
		} else {
			return Vector2.up;
		}
	}

	public Vector3 nearestInsideFloorSpot (Vector3 p) {
		Vector3 pos = new Vector3 (p.x, p.y, 0f);
		Vector2 center = new Vector2 (width / 2, height / 2);
		float xdir = Mathf.Sign (center.x - pos.x);
		int timeout = 20;
		while (pos.x != center.x && timeout > 0) {
			timeout -= 1;
			if (isPassable (pos)) {
				return onFloor (pos);
			}else if (isPassable (pos + Vector3.up)) {
				return onFloor (pos + Vector3.up);
			}else if (isPassable (pos + Vector3.down)) {
				return onFloor (pos + Vector3.down);
			}

			pos += new Vector3 (xdir, 0f, 0f);

		}
		if (isPassable (pos)) {
			return onFloor (pos);
		}else if (isPassable (pos + Vector3.up)) {
			return onFloor (pos + Vector3.up);
		}else if (isPassable (pos + Vector3.down)) {
			return onFloor (pos + Vector3.down);
		}
		return onFloor (pos);
	}
}
