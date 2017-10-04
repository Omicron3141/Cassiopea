using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodSpoils : MonoBehaviour {

	public Text eventText;
	public Button eventButton;

	// Use this for initialization
	void Start () {
		Time.timeScale = 0;
		eventText.text = "One of your crew members has found spoiled food in our storage. " +
			"Apparently it had been improperly preserved. Hopefully we can get along without it.";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void endEvent() {
		Time.timeScale = 1;
		Ship.playerShip.food.currentFood -= 5;
		Destroy (gameObject);
	}

	void OnEnable() {
		eventButton.onClick.AddListener(endEvent);
	}


}
