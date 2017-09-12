using System;

// The ship's oxygen supply is one of the resources that players of Cassiopeia need to keep track of.
// The ship's oxygen supply is displayed as a percentage on a counter that will change color depending on how
// the current oxygen level will affect crew members.

namespace AssemblyCSharp 
{
	public class Oxygen
	{
		int percentage;

		// This is the default constructor. By default, a ship starts with 100% oxygen.
		public Oxygen()
		{
			percentage = 100;
		}

		// This constructor is for special situations when we want the ship to start with
		// less than 100% oxygen.
		public Oxygen(int startingPercentage)
		{
			percentage = startingPercentage;
		}

		// This is how we tick down oxygen via regular usage.
		void useOxygen()
		{
			percentage -= 1;
		}

		// This is how we remove oxygen lost due to an event.
		void loseOxygen(int percentLost) 
		{
			percentage -= percentLost;
		}

		// This is how we add oxygen via an event.
		void addOxygen(int percentAdded)
		{
			percentage += percentAdded;
		}
	}
}