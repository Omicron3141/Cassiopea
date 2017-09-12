using System;

// Fuel is one of the resources the player needs to keep track of in Cassiopeia.
// Fuel is used to move a ship between locations, and it can potentially be lost
// and gained via events.

namespace AssemblyCSharp
{
	public class Fuel
	{
		public int Amount;

		// This is a default constructor that starts the ship with zero fuel.
		public Fuel ()
		{
			Amount = 0;
		}

		// This is a contrusctor that can specify the amount of fuel that the ship starts with.
		public Fuel (int startingAmount)
		{
			Amount = startingAmount;
		}

		// This function is to be called when the ship uses fuel while flying.
		void useFuel () 
		{ 
			if (this.Amount == 0) {
				
			} 

			else {
				this.Amount -= 1;
			}

		}

		// This is how fuel is added to the ship's supply.
		void addFuel (int fuelToBeAdded)
		{
			this.Amount += fuelToBeAdded;
		}

		// This is how fuel is removed via event.
		void removeFuel (int fuelToBeRemoved)
		{
			if ((this.Amount -= fuelToBeRemoved) < 0) {
				this.Amount = 0;
			} 

			else {
				this.Amount -= fuelToBeRemoved;
			}

		}
	}
}

