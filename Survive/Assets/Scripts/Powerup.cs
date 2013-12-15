using UnityEngine;
using System.Collections;

public class Powerup : MonoBehaviour {

	// Connections
	public SpriteRenderer sprite;

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
		transform.Translate(new Vector3(0.0f, SPEED * Time.deltaTime, 0.0f));

		// Collide with player
		Rect rect = Utility.RectForBounds(sprite.bounds);
		Rect playerRect = GameLogic.GetPlayerRect();
		if (Utility.Intersect(rect, playerRect)){
			GameLogic.AwardPowerup();
			GameObject.Destroy(this.gameObject);
		}

		// Destroy when offscreen
		float topEdge = Utility.GetTopEdge() + _height;
		if (transform.position.y > topEdge){
			GameObject.Destroy(this.gameObject);
		}
	}
}
