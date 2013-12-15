using UnityEngine;
using System.Collections;

public class Powerup : MonoBehaviour {

	// Connections
	public SpriteRenderer sprite;
	public SpriteRenderer glow;
	public GameObject sparklePrefab;

	// Internal State
	private float _height = 0.0f;

	// Constants
	private const float SPEED = 1.0f;

	// Use this for initialization
	void Start () {
		_height = Utility.GetSpriteHeight(sprite);
	}
	
	// Update is called once per frame
	void Update () {

		// Move upwards
		transform.Translate(new Vector3(0.0f, SPEED * Time.deltaTime * GameLogic.GetSpeedScale(), 0.0f));

		// Collide with player
		Rect rect = Utility.RectForBounds(sprite.bounds);
		Rect playerRect = GameLogic.GetPlayerRect();
		if (Utility.Intersect(rect, playerRect)){
			GameLogic.AwardPowerup();
			_sparkleEffect();
			GameObject.Destroy(this.gameObject);
		}

		// Destroy when offscreen
		float topEdge = Utility.GetTopEdge() + _height;
		if (transform.position.y > topEdge){
			GameObject.Destroy(this.gameObject);
		}

		// Animate
		float scale = (Mathf.Sin(Time.time) * 0.1f + 0.9f) * 1.75f;
		glow.transform.Rotate(Vector3.forward, Time.deltaTime * 15.0f);
		glow.transform.localScale = new Vector3(scale, scale, scale);
	}

	private void _sparkleEffect(){
		GameObject p = (GameObject)GameObject.Instantiate(sparklePrefab);
		p.transform.position = transform.position;
	}
}
