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
	private const float MAX_SPEED = 25.0f;
	private const float ACCEL = 45.0f;
	private const float AIR_FRICTION = 0.93f;
	private const float PLAYER_SCALE = 0.5f;
	private const float BOUNCE_SPEED = 3.0f;

#region Unity Lifecycle
	// Use this for initialization
	void Start () {
		_width = Utility.GetSpriteWidth(sprite) * PLAYER_SCALE;	
		_anchorY = transform.position.y;
		transform.localScale = new Vector3(PLAYER_SCALE, PLAYER_SCALE, 1.0f);
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
		Vector3 pos = transform.position;
		float leftEdge = Utility.GetLeftEdge() + _width * 0.5f;
		float rightEdge = Utility.GetRightEdge() - _width * 0.5f;
		_bounceOfWall(leftEdge);
		_bounceOfWall(rightEdge);

		// Animation
		float spinBonus = _velocity.x * -50;
		float idleRotateVelocity = _lastDirectionWasLeft ? 25.0f : -25.0f;
		float y =  _anchorY + (Mathf.Sin(_timeAlive) * _width * 0.35f);
		transform.position = new Vector3(transform.position.x, y, 0);
		sprite.transform.Rotate(Vector3.forward, Time.deltaTime * (idleRotateVelocity + spinBonus));
		_timeAlive += Time.deltaTime;
	}

#endregion

#region Internal Helpers
	private void _bounceOfWall(float wallX){
		Vector3 pos = transform.position;
		float middle = Utility.GetMiddleX();
		if ((pos.x > wallX && wallX > middle) || (pos.x < wallX && wallX < middle)){
			transform.position = new Vector3(wallX, pos.y, pos.z);
			_velocity *= -0.35f;
			if (_velocity.x > 0){
				_velocity += new Vector2(BOUNCE_SPEED, 0);
			}else{
				_velocity += new Vector2(-BOUNCE_SPEED, 0);
			}
		}
	}
#endregion

#region Public Methods
	public Rect GetRect(){
		Rect fullSizeRect = Utility.RectForBounds(sprite.bounds);
		return Utility.ScaleRect(fullSizeRect, transform.localScale.x);
	}
	public void Die(){
		GameObject.Destroy(this.gameObject);
	}
	public void DeployParachute(){

	}
#endregion
}
