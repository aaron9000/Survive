using UnityEngine;
using System.Collections;

public class FallingDebris : MonoBehaviour {

	// Connections
	public SpriteRenderer sprite;	

	// Internal State
	private float _normalizedDepth = 1.0f;	
	private float _fallSpeed = 0.0f;
	private float _top = 0.0f;
	private float _rotationVelocity = 0.0f;

	// Constants
	private const float MIN_SCALE = 0.2f;
	private const float MAX_SCALE = 0.8f;
	private const float MIN_FALL_SPEED = 0.3f;
	private const float MAX_FALL_SPEED = 0.8f;

#region Unity Lifecycle
	// Use this for initialization
	void Start () {

		// Determine depth and speed
		_normalizedDepth = UnityEngine.Random.value;
		_fallSpeed = Mathf.Lerp(MIN_FALL_SPEED, MAX_FALL_SPEED, _normalizedDepth);
		_top = Utility.GetTopEdge() + Utility.GetSpriteHeight(sprite);
		_rotationVelocity = (UnityEngine.Random.value - 0.5f) * 40;

		// Adjust size of sprite
		bool reversed = UnityEngine.Random.value < 0.5f;
		float scale = Mathf.Lerp(MIN_SCALE, MAX_SCALE, _normalizedDepth);
		sprite.transform.localScale = new Vector3(reversed ? scale : -scale, scale, 1.0f);
		sprite.sortingOrder = (int)(_normalizedDepth * 1000);
	}
	
	// Update is called once per frame
	void Update () {
		
		// Fall and rotate
		transform.position += new Vector3(0, _fallSpeed * Time.deltaTime, 0);
		transform.Rotate(Vector3.forward, Time.deltaTime * _rotationVelocity);

		// Destroy self?
		if (transform.position.y > _top){
			GameObject.Destroy(this.gameObject);
		}
	}
#endregion

}
