using UnityEngine;
using System.Collections;

public class ObstacleSpawner : MonoBehaviour {

	// Connections
	public GameObject obstaclePrefab;
	public GameObject powerupPrefab;

	// Internal State
	private float _cooldown = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		_cooldown -= Time.deltaTime;
		if (GameLogic.ShouldSpawnObstacle() == false){
			return;
		}
		if (_cooldown <= 0){
			if (UnityEngine.Random.value > Constants.POWERUP_CHANCE){
				_spawnObject(obstaclePrefab);
			}else{
				_spawnObject(powerupPrefab);
			}
			_cooldown = GameLogic.GetObstacleDelay();
		}
	}
	private void _spawnObject(GameObject prefab){
		GameObject p = (GameObject)GameObject.Instantiate(prefab);
		Vector3 randPos = new Vector3(Utility.GetWidth() * 0.75f * (UnityEngine.Random.value - 0.5f), 0, 0);
		p.transform.position = this.transform.position + randPos;
	}
}
