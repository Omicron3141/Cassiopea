using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnviromentManager: MonoBehaviour {

	[System.Serializable]
	public class EventMap
	{
		public string tag;
		public GameObject eventObject;
	}

	[System.Serializable]
	public class EventList
	{
		public Event[] events;
	}

	[System.Serializable]
	public class Event
	{
		public string tag;
		public string title;
		public string text;
		public float weight;
		public Btn[] buttons;
	}

	[System.Serializable]
	public class Btn
	{
		public string text;
		public string function;
		public string[] paramaters;
	}


	public static EnviromentManager instance;
	public GameObject popup;
	public List<Event> possibleEvents;
	private Event lastevent;
	private float totalWeight;
	public EventMap[] passiveEvents;
	public float eventProbability;
	public PassiveEvent currentEffect;

	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
		} else {
			Destroy (gameObject);
		}
		possibleEvents = new List<Event> ();
		loadEvents ();
	}


	// Update is called once per frame
	void Update () {
		if (currentEffect == null) {
			if (Random.value < eventProbability * Time.deltaTime) {
				createEvent (selectEvent());
			}
		}
	}

	public void loadEvents() {
		totalWeight = 0;
		Object[] jsons = Resources.LoadAll ("Events", typeof(TextAsset));
		foreach (Object asset in jsons) {
			TextAsset json = (TextAsset)asset;
			EventList evs = JsonUtility.FromJson<EventList> (json.text);
			foreach (Event ev in evs.events) {
				possibleEvents.Add (ev);
				totalWeight += ev.weight;
			}
		}
	}


	Event selectEvent(){
		float actualTotalWeight = totalWeight;
		if (lastevent != null) {
			actualTotalWeight -= lastevent.weight;
		}
		float value = Random.value * actualTotalWeight;
		foreach (Event ev in possibleEvents) {
			value -= ev.weight;
			if (value <= 0 && (lastevent == null || ev != lastevent)) {
				lastevent = ev;
				return ev;
			}
		}
		Debug.Log ("null event");
		return null;
	}

	void createEvent(Event ev) {
		GameObject pop = Instantiate (popup);
		Transform panel = pop.transform.Find ("Canvas").Find ("Panel");
		panel.Find("Title").GetComponent<Text> ().text = ev.title;
		panel.Find("Description").GetComponent<Text> ().text = ev.text;
		GameObject[] buttons = {
			panel.Find ("Button1").gameObject,
			panel.Find ("Button2").gameObject,
			panel.Find ("Button3").gameObject
		};
		if (ev.buttons.Length < 3) {
			buttons[2].SetActive (false);
			if (ev.buttons.Length < 2) {
				buttons[1].SetActive (false);
			}
		}
		for (int i = 0; i < ev.buttons.Length; i++) {
			GameObject button = buttons [i];
			button.transform.Find ("Text").GetComponent<Text> ().text = ev.buttons [i].text;
			Button.ButtonClickedEvent btnev = new Button.ButtonClickedEvent();
			btnev.AddListener (pop.GetComponent<PopupEvent> ().endEvent);
			string func = ev.buttons [i].function;
			string[] param = ev.buttons[i].paramaters;
			btnev.AddListener (() => eventFunction(func, param));
			button.GetComponent<Button> ().onClick = btnev;
		}

	}

	void eventFunction(string function, string[] paramaters){
		switch (function) {
			case "changeResource":
				changeResource (paramaters [0], int.Parse (paramaters [1]));
				break;
			case "loadEvent":
				loadEvent (paramaters [0]);
				break;
			case "beginPassiveEvent":
				beginPassiveEvent (paramaters [0]);
				break;
			case "randomSelect":
				int i1 = int.Parse(paramaters [1]);
				string[] option1 = new string[i1];
				string[] option2 = new string[paramaters.Length - i1 - 4];
				for (int j = 0; j < option1.Length; j++) {
					option1 [j] = paramaters [3 + j];
				}
				for (int j = 0; j < option2.Length; j++) {
					option2 [j] = paramaters [4 + i1 + j];
				}
				randomSelect (float.Parse(paramaters [0]), paramaters[2], option1, paramaters[3+i1], option2);
				break;
			case "And":
				int i2 = int.Parse(paramaters [0]);
				string[] action1 = new string[i2];
				string[] action2 = new string[paramaters.Length - i2 - 3];
				for (int j = 0; j < action1.Length; j++) {
					action1 [j] = paramaters [2 + j];
				}
				for (int j = 0; j < action2.Length; j++) {
					action2 [j] = paramaters [3 + i2 + j];
				}
				concatFunction (paramaters[1], action1, paramaters[2+i2], action2);
				break;
		}
	}

	// Modifies the ship resources. Usage:
	// "changeResource x y" where x is the resource to change ("food", "fuel", "oxygen", "metal", or "water") and y is the integer amount to change by.
	void changeResource (string resource, int amount) {
		if (resource == "food") {
			if ((Ship.playerShip.food.currentFood + amount) > Ship.playerShip.food.maximumFoodStorage) {
				Ship.playerShip.food.currentFood = Ship.playerShip.food.maximumFoodStorage;
			} else if ((Ship.playerShip.food.currentFood + amount) < 0) {
				Ship.playerShip.food.currentFood = 0;
			} else{
				Ship.playerShip.food.currentFood += amount;
			}
		} 

		else if (resource == "fuel") {
			if ((Ship.playerShip.fuel.amount + amount) > Ship.playerShip.fuel.maximumFuelStorage) {
				Ship.playerShip.fuel.amount = Ship.playerShip.fuel.maximumFuelStorage;
			} else if ((Ship.playerShip.fuel.amount + amount) < 0) {
				Ship.playerShip.fuel.amount = 0;
			} else{
				Ship.playerShip.fuel.amount += amount;
			}
		} 

		else if (resource == "oxygen") {
			if ((Ship.playerShip.oxygen.percentage + amount) > 100) {
				Ship.playerShip.oxygen.percentage = 100;
			} else if ((Ship.playerShip.oxygen.percentage + amount) < 0) {
				Ship.playerShip.oxygen.percentage = 0;
			} else{
				Ship.playerShip.oxygen.percentage += amount;
			}
		}

		else if (resource == "metal") {
			if ((Ship.playerShip.metal.amount + amount) > Ship.playerShip.metal.maxAmount) {
				Ship.playerShip.metal.amount = Ship.playerShip.metal.maxAmount;
			} else if ((Ship.playerShip.metal.amount + amount) < 0) {
				Ship.playerShip.metal.amount = 0;
			} else{
				Ship.playerShip.metal.amount += amount;
			}
		}

		else if (resource == "water") {
			if ((Ship.playerShip.water.amount + amount) > Ship.playerShip.water.maxAmount) {
				Ship.playerShip.water.amount = Ship.playerShip.water.maxAmount;
			} 

			else if ((Ship.playerShip.water.amount + amount) < 0) {
				Ship.playerShip.water.amount = 0;
			} 

			else{
				Ship.playerShip.water.amount += amount;
			}
		}
	}

	// Modifies the ship's maximum resource storage. Usage:
	// "changeResourceStorage x y" where x is the resource to change ("food", "fuel", "metal", or "water") and y is the integer amount to change by.
	void changeResourceStorage (string resource, int amount) {
		if (resource == "food") {
			if ((Ship.playerShip.food.maximumFoodStorage + amount) < 0) {
				Ship.playerShip.food.maximumFoodStorage = 0;
			} 

			else {
				Ship.playerShip.food.maximumFoodStorage += amount;
			}
		} 

		else if (resource == "fuel") {
			if ((Ship.playerShip.fuel.maximumFuelStorage + amount) < 0) {
				Ship.playerShip.fuel.maximumFuelStorage = 0;
			} 

			else {
				Ship.playerShip.fuel.maximumFuelStorage += amount;
			}
		} 

		else if (resource == "metal") {
			if ((Ship.playerShip.metal.maxAmount + amount) < 0) {
				Ship.playerShip.metal.maxAmount = 0;
			} 

			else {
				Ship.playerShip.metal.maxAmount += amount;
			}
		}

		else if (resource == "water") {
			if ((Ship.playerShip.water.maxAmount + amount) < 0) {
				Ship.playerShip.water.maxAmount = 0;
			} 

			else {
				Ship.playerShip.water.maxAmount += amount;
			}
		}
	}

	// begins a new popup event. Usage:
	// "loadevent s" where s is the string tag of an event (e.g. "et-0")
	void loadEvent(string ev) {
		foreach (Event env in possibleEvents) {
			if (env.tag == ev) {
				createEvent (env);
			}
		}
	}

	// begins a new passive event. Usage:
	// "beginPassiveEvent s" where s is the string key of an event (e.g. "asteroidfield")
	void beginPassiveEvent(string ev) {
		foreach (EventMap map in passiveEvents) {
			if (map.tag == ev) {
				GameObject passev = Instantiate (map.eventObject);
				passev.transform.position = new Vector3 (0f, 0f, 0f);
				currentEffect = passev.GetComponent<PassiveEvent>();
			}
		}
	}


	// randomly selects between two other events. Usage:
	// "randomSelect p i 111 222" where p is a float probability of selecting the first option, i is the number of paramaters for the first option,
	// 									111 is the command for the first option, and 222 is the command for the second option
	void randomSelect(float prob, string func1, string[] params1, string func2, string[] params2 ) {
		if (Random.value < prob) {
			eventFunction (func1, params1);
		} else {
			eventFunction (func2, params2);
		}
	}

	// does one event and then the other. Usage:
	// "And i 111 222" where i is the number of paramaters for the first action,
	// 						 111 is the command for the first action, and 222 is the command for the second action
	void concatFunction(string func1, string[] params1, string func2, string[] params2 ) {
		eventFunction (func1, params1);
		eventFunction (func2, params2);
	}


	public void endEvent(){
		currentEffect = null;
	}

}
