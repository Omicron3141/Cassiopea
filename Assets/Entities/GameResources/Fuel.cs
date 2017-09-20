using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Fuel is one of the resources the player needs to keep track of in Cassiopeia.
// Fuel is used to move a ship between locations, and it can potentially be lost
// and gained via events.

namespace AssemblyCSharp
{
	public class Fuel
	{
		public int amount;
		public int maximumFuelStorage;

		// This is a default constructor that starts the ship with full fuel.
		public Fuel (int maxFuelStorage)
		{
			maximumFuelStorage = maxFuelStorage;
			amount = 0;
		}

		// This is a contrusctor that can specify the amount of fuel that the ship starts with.
		public Fuel (int startingAmount, int maxFuelStorage)
		{
			maximumFuelStorage = maxFuelStorage;
			amount = startingAmount;
		}

		// This function is to be called when the ship uses fuel while flying.
		void useFuel () 
		{ 
			if (this.amount == 0) {
				
			} 

			else {
				this.amount -= 1;
			}

		}

		// This is how fuel is added to the ship's supply.
		void addFuel (int fuelToBeAdded)
		{
			if ((fuelToBeAdded + this.amount) > maximumFuelStorage) {
				this.amount = this.maximumFuelStorage;
			} 

			else {
				this.amount += fuelToBeAdded;
			}
		}

		// This is how fuel is removed via event.
		void removeFuel (int fuelToBeRemoved)
		{
			if ((this.amount -= fuelToBeRemoved) < 0) {
				this.amount = 0;
			} 

			else {
				this.amount -= fuelToBeRemoved;
			}

		}

		// This is how fuel storage is upgraded.
		void upgradeFuelStorage (int amountToBeUpgraded) 
		{
			this.maximumFuelStorage += amountToBeUpgraded;
		}
	}
}

