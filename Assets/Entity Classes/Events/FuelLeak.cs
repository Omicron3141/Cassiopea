using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelLeak : MonoBehaviour {

	public Text eventText;
	public Button eventButton;

	// Use this for initialization
	void Start () {
		Time.timeScale = 0;
		eventText.text = "Following an odd smell, a crew member realized that a batch of " +
			"fuel was leaking. We won’t be able to salvage the fuel, but at least they " +
			"prevented a potential catastrophe by fixing the leak.";

	}

	// Update is called once per frame
	void Update () {

	}

	void endEvent() {
		Time.timeScale = 1;
		Ship.playerShip.fuel.amount -= 5;
		Destroy(gameObject);
	}

	void OnEnable() {
		eventButton.onClick.AddListener(endEvent);
	}

}
