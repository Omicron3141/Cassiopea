using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidField : MonoBehaviour {
	public float height = 0;
	public float length = 0;
	public float spawnDuration = 0.3f;
	public float spawnDurationRandomization = 0.15f;
	public float spawntimer = 0f;
	public float speed;
	public float rotspeed;
	public float spawnBuffer = 3f;
	public GameObject[] asteroids;
	PassiveEvent ev;
	private bool createAsteroids = true;
	private float lastSpawnedY = 0f;
	public float destroyDelay;
	// Use this for initialization
	void Start () {
		ev = GetComponent<PassiveEvent> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (spawntimer < 0 && createAsteroids) {
			GameObject astr = Instantiate (asteroids [Random.Range (0, asteroids.Length)]);
			float newY = transform.position.y + (Random.value - 0.5f) * height;
			int i = 10;
			while (Mathf.Abs (newY - lastSpawnedY) < spawnBuffer && i>0) {
				newY = transform.position.y + (Random.value - 0.5f) * height;
				i--;
			}
			lastSpawnedY = newY;
			astr.transform.position = new Vector3 (transform.position.x, newY, 0.1f);
			float scale = Random.value + 0.5f;
			astr.transform.localScale = new Vector3 (scale, scale, 1f);
			astr.GetComponent<Asteroid> ().health = scale * 20;
			astr.GetComponent<Asteroid> ().endPosition = -100;
			astr.GetComponent<Asteroid> ().speed = speed + Random.value * 2;
			astr.GetComponent<Asteroid> ().rotSpeed = rotspeed * (Random.value + 0.5f) * (Random.Range (0, 2) * 2f - 1f);
			spawntimer = spawnDuration + (spawnDurationRandomization * (Random.value - 0.5f));
		} else {
			spawntimer -= Time.deltaTime;
		}
		if (ev.duration < destroyDelay) {
			ParticleSystem.EmissionModule sys = transform.Find("Particle System").GetComponent<ParticleSystem> ().emission;
			sys.enabled = false;
			createAsteroids = false;
		}
	}
}
