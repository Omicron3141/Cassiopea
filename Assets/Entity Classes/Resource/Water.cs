using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The ship's water supply is one of the resources that players of Cassiopeia need to keep track of.
// The ship's water supply is displayed by a counter at the top of the screen.
// Water is primarily consumed by crew members and hydroponic farms.

public class Water {

	public int amount;
	public int maxAmount;

	// This is the default constructor for water. By default, the player starts with the maximum possible amount of water.
	public Water(int _maxAmount) {
		maxAmount = _maxAmount;
	}

	// This constructor allows the ship to start with a non-maximal amount of water.
	public Water(int _amount, int _maxAmount) {
		if (_amount > _maxAmount) {
			_amount = _maxAmount;
		} 

		else {
			amount = _amount;
		}

		maxAmount = _maxAmount;
	}
		
	public void addWater(int waterAdded) {
		if ((this.amount + waterAdded) > this.maxAmount) {
			this.amount = this.maxAmount;
		} 

		else {
			this.amount += waterAdded;
		}
	}

	public void removeWater(int waterRemoved) {
		if ((this.amount - waterRemoved) < 0) {
			this.amount = 0;
		} 

		else {
			this.amount -= waterRemoved;
		}
	}

	public void upgradeMaximumWater(int storageAdded) {
		this.maxAmount += storageAdded;
	}
}
