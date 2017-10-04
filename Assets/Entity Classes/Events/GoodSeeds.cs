using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoodSeeds : MonoBehaviour {

	public Text eventText;
	public Button eventButton;

	// Use this for initialization
	void Start () {
		Time.timeScale = 0;
		eventText.text = "It looks like the seeds we brought for our " +
			"hydroponics have been unexpectedly effective. The extra food will be welcome.";
	}

	// Update is called once per frame
	void Update () {

	}

	void endEvent() {
		Time.timeScale = 1;

		if ((Ship.playerShip.food.currentFood + 5) > Ship.playerShip.food.maximumFoodStorage) {
			Ship.playerShip.food.currentFood = Ship.playerShip.food.maximumFoodStorage;
		} 

		else {
			Ship.playerShip.food.currentFood += 5;
		}

		Destroy (gameObject);
	}

	void OnEnable() {
		eventButton.onClick.AddListener(endEvent);
	}


}
