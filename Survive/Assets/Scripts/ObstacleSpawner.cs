using UnityEngine;
using System.Collections;

public class ObstacleSpawner : MonoBehaviour {

	// Connections
	public GameObject obstaclePrefab;

	// Internal State
	private float _cooldown = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		_cooldown -= Time.deltaTime;
		if (_cooldown <= 0){
			GameObject obstacle = (GameObject)GameObject.Instantiate(obstaclePrefab);
			Vector3 randPos = 0.3f * Utility.GetWidth() * Utility.NormalizedRadialSpread();
			obstacle.transform.position = this.transform.position + randPos;
			_cooldown = 3.0f;
		}
	}
}
