using UnityEngine;
using System.Collections;

public class FallingDebris : MonoBehaviour {

	// Connections
	public SpriteRenderer sprite;	

	// Internal State
	private float _normalizedDepth = 1.0f;	
	private float _fallSpeed = 0.0f;
	private float _bottom = 0.0f;

	// Constants
	private const float MIN_SCALE = 0.25f;
	private const float MAX_SCALE = 1.0f;
	private const float MIN_FALL_SPEED = 2.0f;
	private const float MAX_FALL_SPEED = 5.0f;

#region Unity Lifecycle
	// Use this for initialization
	void Start () {

		// Determine depth and speed
		_normalizedDepth = UnityEngine.Random.value;
		_fallSpeed = Mathf.Lerp(MIN_FALL_SPEED, MAX_FALL_SPEED, _normalizedDepth);
		_bottom = Utility.GetBottomEdge() - Utility.GetSpriteHeight(sprite);

		// Adjust size of sprite
		float scale = Mathf.Lerp(MIN_SCALE, MAX_SCALE, _normalizedDepth);
		sprite.transform.localScale = new Vector3(scale, scale, 1.0f);
	}
	
	// Update is called once per frame
	void Update () {
		
		// Fall and rotate
		transform.position += new Vector3(0, -_fallSpeed * Time.deltaTime, 0);
		transform.Rotate(Vector3.forward, Time.deltaTime * 100.0f);

		// Destroy self?
		if (transform.position.y < _bottom){
			GameObject.Destroy(this.gameObject);
		}
	}
#endregion

#region Helpers
	
#endregion
}
