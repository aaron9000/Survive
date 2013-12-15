using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	// Connections
	public SpriteRenderer sprite;	

	// Internal State
	private bool _parachuteActive = false;
	private float _parachuteLivetime = 0.0f;
	private float _width = 0.0f;
	private float _timeAlive = 0.0f;
	private float _anchorY = 0.0f;
	private bool _lastDirectionWasLeft = false;
	private Vector2 _velocity = Vector2.zero;

	// Constants
	private const float MAX_SPEED = 36.0f;
	private const float ACCEL = 56.0f;
	private const float AIR_FRICTION = 0.935f;

	// Use this for initialization
	void Start () {
		_width = Utility.GetSpriteWidth(sprite);	
		_anchorY = transform.position.y;
	}
	
	// Physics Updates
	void FixedUpdate(){

		// Velocity and airfriction
		_velocity *= AIR_FRICTION;
		transform.Translate(_velocity * Time.fixedDeltaTime);
	}

	// Update is called once per frame
	void Update () {
		
		// Movement
		if (Input.GetKey(KeyCode.A)){
			// Left
			_velocity = new Vector2(Mathf.Clamp(_velocity.x - ACCEL * Time.deltaTime, -MAX_SPEED, MAX_SPEED), 0);
			_lastDirectionWasLeft = true;
		}
		if (Input.GetKey(KeyCode.D)){
			// Right
			_velocity = new Vector2(Mathf.Clamp(_velocity.x + ACCEL * Time.deltaTime, -MAX_SPEED, MAX_SPEED), 0);
			_lastDirectionWasLeft = false;
		}	

		// Detect wall collision

		// Detect obstalce collision

		// Animation
		// Bobbing animation
		float spinBonus = _velocity.x * -50;
		float idleRotateVelocity = _lastDirectionWasLeft ? 25.0f : -25.0f;
		float y =  _anchorY + (Mathf.Sin(_timeAlive) * _width * 0.3f);
		transform.position = new Vector3(transform.position.x, y, 0);
		sprite.transform.Rotate(Vector3.forward, Time.deltaTime * (idleRotateVelocity + spinBonus));

		_timeAlive += Time.deltaTime;
	}
}
