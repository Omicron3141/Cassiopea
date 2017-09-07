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
	}

	public int thingAt(int x, int y) {
		if ((x < width) && (y < height)) {
			return map [x, y];
		} else {
			Debug.LogError ("Tried to access out-of-bounds map index at "+x+","+y);
			return -1;
		}
	}
	public void setThing(int x, int y, int thing) {
		if ((x < width) && (y < height)) {
			map [x, y] = thing;
		} else {
			Debug.LogError ("Tried to access out-of-bounds map index at "+x+","+y);
		}
	}
	public bool isWithinBounds(Vector3 pos) {
		return (((int)pos.x < width) && ((int)pos.y < height));
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
		while (isPassible((int)x, (int)(y-1))){
			y--;
		}
		return new Vector3 (x, y, pos.z);
	}

	public Queue<Vector3> pathfind(Vector3 startpos, Vector3 endpos) {
		Queue<Vector3> path = new Queue<Vector3>();

		Vector3 nearestLadderInDirectionOfTravel = nearestLadder (startpos, (int)(endpos.x - startpos.x));
		Vector3 nearestLadderInOppositeDirectionOfTravel = nearestLadder (startpos, (int)(startpos.x - endpos.x));
		Vector3 ladderbase;
		if ((nearestLadderInDirectionOfTravel - startpos).magnitude < (nearestLadderInOppositeDirectionOfTravel - startpos).magnitude) {
			ladderbase = nearestLadderInDirectionOfTravel;
		} else {
			ladderbase = nearestLadderInOppositeDirectionOfTravel;
		}
		path.Enqueue (ladderbase);
		Vector3 laddertop = new Vector3 (ladderbase.x, endpos.y, ladderbase.z);
		path.Enqueue (laddertop);
		path.Enqueue (endpos);
		return path;

	}

	private Vector3 nearestLadder(Vector3 startpos, int direction) {
		int x = (int)startpos.x;
		int y = (int)startpos.y;
		while ((thingAt (x, y) != ladder) && (x < width) && (x>0)) {
			x += direction / Mathf.Abs (direction);
		}
		return new Vector3 (x*1f, y*1f, startpos.z);
	}
}
