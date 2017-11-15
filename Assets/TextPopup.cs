using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPopup : MonoBehaviour {
	public float duration=1f;
	public float speed=10f;
	float remainingduration;
	Color c = Color.white;
	// Use this for initialization
	void Start () {
		remainingduration = duration;
	}
	
	// Update is called once per frame
	void Update () {
		remainingduration -= Time.deltaTime;
		transform.Translate (0f, Time.deltaTime * speed, 0f);
		c.a = remainingduration/duration;
		GetComponent<SpriteRenderer> ().color = c;
		if (remainingduration < 0) {
			Destroy (gameObject);
		}
	}
}
