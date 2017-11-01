using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The ship's metal supply is one of the resources that players of Cassiopeia need to keep track for.
// The ship's metal supply is displayed as a counter at the top of the screen.
// Metal is primarily used to repair the ship and make upgrades to it.

public class Metal 
{	
	public int amount;
	public int maxAmount;

	// This is the default constructor for metal. By default, the player starts with their maximum amount of metal.
	public Metal(int _maxAmount) 
	{
		maxAmount = _maxAmount;
		amount = _maxAmount;
	}

	// This constructor allows the ship to start with a specified amount of metal.
	public Metal(int _amount, int _maxAmount) 
	{
		if (_amount > _maxAmount) {
			amount = _maxAmount;
		} 

		else {
			amount = _amount;
		}

		maxAmount = _maxAmount;
	}

	public void addMetal(int metalAdded) {
		if ((this.amount + metalAdded) > this.maxAmount) {
			this.amount = this.maxAmount;
		} 

		else {
			this.amount += metalAdded;
		}
	}

	public void removeMetal(int metalRemoved) {
		if ((this.amount - metalRemoved) < 0) {
			this.amount = 0;
		} 

		else {
			this.amount -= metalRemoved;
		}
	}

	public void upgradeMetalStorage(int storageAdded) {
		this.maxAmount += storageAdded;
	}
}
