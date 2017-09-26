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
				currentEvent = ev.GetComponent<GameEvent> ();
				ev.transform.position = new Vector3 (100f, 0f, 0f);
				currentEvent.duration = meanDuration + (Random.value * 2 - 1) * variationDuration;
			}
		}
	}


	public void endEvent(){
		currentEvent = null;
	}

}
