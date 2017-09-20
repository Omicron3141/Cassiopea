using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMap{
	int[,] map;
	int width;
	int height;

	public const int ladder = 2;
	public const int passable = 0;
	public const int impassible = 1;

	public ShipMap(int w, int h) {
		width = w;
		height = h;
		map = new int[width, height];
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				map [x, y] = -1;
			}
		}
	}

	public int thingAt(int x, int y) {
		if ((x < width) && (y < height) && (x>=0) && (y>=0)) {
			return map [x, y];
		} else {
			//Debug.LogError ("Tried to access out-of-bounds map index at "+x+","+y);
			return -1;
		}
	}
	public void setThing(int x, int y, int thing) {
		if ((x < width) && (y < height) && (x>=0) && (y>=0)) {
			map [x, y] = thing;
		} else {
			Debug.LogError ("Tried to access out-of-bounds map index at "+x+","+y);
		}
	}
	public bool isWithinBounds(Vector3 pos) {
		return (((int)pos.x < width) && ((int)pos.y < height));
	}

	public bool isInShip (int x, int y) {
		return (thingAt (x, y) >= 0);
	}

	public bool isPassible (int x, int y) {
		return (thingAt (x, y) == passable);
	}

	public bool isLadder (int x, int y) {
		return (thingAt (x, y) == ladder);
	}

	// takes in a point and follows it down to the floor below that place
	public Vector3 onFloor(Vector3 pos) {
		float x = pos.x;
		float y = pos.y;
		while ((isPassible((int)x, (int)(y-1))) && (isInShip((int)x, (int)(y-1)))){
			y--;
		}
		return new Vector3 (x, y, pos.z);
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

	public Vector3 getNewWanderTarget (Vector3 startpos) {
		int startx = (int)startpos.x;
		int starty = (int)startpos.y;
		int direction = (int)(Mathf.Round (Random.value) * 2 - 1);
		int distance = Random.Range (3, 4);
		for (int i = 1; i < distance; i++) {
			if (!isPassible (startx + direction * i, starty)) {
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
}
