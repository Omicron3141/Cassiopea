using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnviromentManager: MonoBehaviour {

	public static EnviromentManager instance;
	public GameObject[] events;
	public float eventProbability;
	public float meanDuration;
	public float variationDuration;
	private GameEvent currentEvent;

	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
		} else {
			Destroy (gameObject);
		}
	}

	
	// Update is called once per frame
	void Update () {
		if (currentEvent == null) {
			if (Random.value < eventProbability * Time.deltaTime) {
				GameObject ev = Instantiate(events [Random.Range (0, events.Length)]);
				Debug.Log ("Starting Event: " + ev.ToString ());
				currentEvent = ev.GetComponent<GameEvent> ();
				if (currentEvent.eventType == GameEvent.PASSIVE) {
					PassiveEvent passiveEvent = (PassiveEvent)currentEvent;
					ev.transform.position = new Vector3 (100f, 0f, 0f);
					passiveEvent.duration = meanDuration + (Random.value * 2 - 1) * variationDuration;
				} else if (currentEvent.eventType == GameEvent.POPUP) {
					PopupEvent popupEvent = (PopupEvent)currentEvent;
				}
			}
		}
	}


	public void endEvent(){
		currentEvent = null;
	}

}
