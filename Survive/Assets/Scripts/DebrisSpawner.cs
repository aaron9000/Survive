using UnityEngine;
using System.Collections;

public class DebrisSpawner : MonoBehaviour {

	// Connections
	public GameObject debrisPrefab;

	// Internal State
	private float _cooldown = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		_cooldown -= Time.deltaTime;
		if (_cooldown <= 0){
			GameObject debris = (GameObject)GameObject.Instantiate(debrisPrefab);
			Vector3 randPos = 0.3f * Utility.GetWidth() * Utility.NormalizedRadialSpread();
			debris.transform.position = this.transform.position + randPos;
			_cooldown = 1.0f;
		}
	}
}
