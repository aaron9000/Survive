using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {

	// Connections
	public SpriteRenderer sprite;
	public Sprite spriteA;
	public Sprite spriteB;

	// Internal State
	private float _height = 0.0f;

	// Constants
	private const float SPEED = 1.0f;

	// Use this for initialization
	void Start () {
		_height = Utility.GetSpriteHeight(sprite);
		bool reversed = UnityEngine.Random.value < 0.5f;
		float scale = UnityEngine.Random.value * 0.5f + 0.5f;
		this.transform.localScale = new Vector3(reversed ? scale : -scale, 1.0f);
		if (UnityEngine.Random.value < 0.5f){
			sprite.sprite = spriteA;
		}else{
			sprite.sprite = spriteB;
		}
	}

	// Update is called once per frame
	void Update () {

		// Move upwards
		transform.Translate(new Vector3(0.0f, SPEED * Time.deltaTime * GameLogic.GetSpeedScale(), 0.0f));

		// Kill player on collision
		float scale = UnityEngine.Random.value * 0.5f + 0.5f;
		float top = (sprite.bounds.size.y * 0.5f * 0.9f * scale) + sprite.bounds.center.y;
		float x = sprite.bounds.center.x;
		float width = sprite.bounds.size.x * scale;
		Vector2 linePos = new Vector2(x, top);
		Rect playerRect = GameLogic.GetPlayerRect();
		if (Utility.HorizontalLineIntersect(linePos, width, playerRect)){
			GameLogic.KillPlayer();
		}

		// Destroy when offscreen
		float topEdge = Utility.GetTopEdge() + _height;
		if (transform.position.y > topEdge){
			GameObject.Destroy(this.gameObject);
		}
	}
}
