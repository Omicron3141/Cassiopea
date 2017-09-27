using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	public class Food
	{
		public int maximumFoodStorage;
		public int currentFood;

		// The default constructor sets the current food equal to the maximum food.
		public Food (int maxFoodStorage)
		{
			maximumFoodStorage = maxFoodStorage;
			currentFood = maxFoodStorage;
		}

		// This constructor allows you to set the values of the current food and maximum
		// food storage separately.
		public Food (int maxFoodStorage, int startingFood)
		{
			maximumFoodStorage = maxFoodStorage;
			currentFood = startingFood;
		}

		// This is how we add to the current food.
		void addFood (int foodAdded) 
		{
			if ((currentFood + foodAdded) > maximumFoodStorage) {
				currentFood = maximumFoodStorage;
			} 

			else {
				currentFood += foodAdded;
			}
		}

		// This is how we remove food for events.
		void removeFood (int foodRemoved) {
			if ((currentFood - foodRemoved) < 0) {
				currentFood = 0;
			} 

			else {
				currentFood -= foodRemoved;
			}
		}

		// This is how we use food for feeding the crew.
		void useFood () {

			if (currentFood == 0) {
				
			} 

			else {
				currentFood -= 1;
			}
		}

		// This is how we upgrade the maximum amount of food the ship can store.
		void upgradeMaxFood (int foodStorageUpgrade) {
			maximumFoodStorage += foodStorageUpgrade;
		}

	}

