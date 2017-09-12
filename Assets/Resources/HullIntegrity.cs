using System;

// Hull Integrity represents the health of the ship. When hull integrity reaches zero, the ship has been destroyed.

namespace AssemblyCSharp
{
	public class HullIntegrity
	{
		int maximumHullIntegrity;
		int currentHullIntegrity;

		// When we use the default constructor, the hull integrity starts at its maximum potential hull integrity.
		public HullIntegrity (int maxHullIntegrity)
		{
			maximumHullIntegrity = maxHullIntegrity;
			currentHullIntegrity = maxHullIntegrity;
		}

		// This constructor is for when we want the hull integrity to not start at its potential maximum. 
		public HullIntegrity (int maxHullIntegrity, int startingHullIntegrity)
		{
			maximumHullIntegrity = maxHullIntegrity;
			currentHullIntegrity = startingHullIntegrity;
		}

		// This method is called when the hull loses integrity.
		void loseHullIntegrity (int hullIntegrityLost) 
		{
			if ((currentHullIntegrity -= hullIntegrityLost) < 0) {
				currentHullIntegrity = 0;
			} 

			else {
				currentHullIntegrity -= hullIntegrityLost;
			}
		}

		// This method is called when the hull gains integrity.
		void addHullIntegrity (int hullIntegrityAdded)
		{
			if ((currentHullIntegrity += hullIntegrityAdded) > maximumHullIntegrity) {
				currentHullIntegrity = maximumHullIntegrity;
			} 

			else {
				currentHullIntegrity += hullIntegrityAdded;
			}
		}

		// This method is called if the ship's hull integrity is upgraded.
		void upgradeHullIntegrity (int hullIntegrityUpgraded) 
		{
			maximumHullIntegrity += hullIntegrityUpgraded;
		}

	}
}

