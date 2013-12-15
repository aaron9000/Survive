using UnityEngine;
using System.Collections;

public class DebrisSpawner : MonoBehaviour {

	// Connections
	public GameObject debrisPrefab;

	// Internal State
	private float _cooldown = 0.0f;

	// Constants
	private const float START_COUNT = 10;
	private const float SPAWN_COOLDOWN = 0.5f;

	// Use this for initialization
	void Start () {
		// Create a bunch at the start to fill screen
		_fillScreen();
	}
	
	// Update is called once per frame
	void Update () {

		_cooldown -= Time.deltaTime;
		if (_cooldown <= 0){
			_createDebris(false);
			_cooldown = SPAWN_COOLDOWN;
		}
	}

	private void _createDebris(bool fillingScreen){
		GameObject debris = (GameObject)GameObject.Instantiate(debrisPrefab);
		Vector3 randPos = 0.3f * Utility.GetWidth() * Utility.NormalizedRadialSpread();
		if (fillingScreen == false){
			debris.transform.position = this.transform.position + randPos;
		}else{
			var randY = Utility.GetBottomEdge() + Utility.GetHeight() * UnityEngine.Random.value;
			debris.transform.position = new Vector3(Utility.GetMiddleX(), randY, 0) + randPos;
		}
	}

	private void _fillScreen(){
		for (int i = 0; i < START_COUNT; i++){
			_createDebris(true);
		}
	}
}
