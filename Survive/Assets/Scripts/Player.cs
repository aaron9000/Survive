using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	// Connections
	public SpriteRenderer sprite;	
	public GameObject bloodPrefab;
	public GameObject gibPrefab;
	public ParticleSystem sparkleParticle;

	// Internal State
	private float _parachuteLivetime = 0.0f;
	private float _width = 0.0f;
	private float _timeAlive = 0.0f;
	private float _anchorY = 0.0f;
	private bool _lastDirectionWasLeft = false;
	private Vector2 _velocity = Vector2.zero;
	private bool _hasEntered = false;
	
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
		sparkleParticle.Stop ();
		sparkleParticle.renderer.sortingLayerName = Constants.EffectsLayerName;
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

		// Animate into scene after spawn
		if (_hasEntered == false){
			float desiredHeight = Utility.GetBottomEdge() + Utility.GetHeight() * 0.8f;
			if (transform.position.y <= desiredHeight){
				_hasEntered = true;
				_anchorY = transform.position.y;
			}else{
				transform.Translate(new Vector3(0, -Time.deltaTime * 2.0f));
			}
			return;
		}

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
		float y =  _anchorY + (Mathf.Sin(_timeAlive * 1.25f) * _width * 0.6f);
		transform.position = new Vector3(transform.position.x, y, 0);
		sprite.transform.Rotate(Vector3.forward, Time.deltaTime * (idleRotateVelocity + spinBonus));
		_timeAlive += Time.deltaTime;

		// Tick down powerupm
		_parachuteLivetime -= Time.deltaTime;
		bool parachuteActive = ParachuteIsActive ();
		if (sparkleParticle.isStopped && parachuteActive){
			sparkleParticle.Play ();
		}else if (sparkleParticle.isPlaying && !parachuteActive){
			sparkleParticle.Stop ();
		}
	}

#endregion

#region Internal Helpers
	private void _bounceOfWall(float wallX){

		// Handle phyics
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
			_bloodEffect();
		}
	}
	private void _bloodEffect(){
		GameObject p = (GameObject)GameObject.Instantiate(bloodPrefab);
		p.transform.position = transform.position;
	}
	private void _gibEffect(){
		GameObject p = (GameObject)GameObject.Instantiate(gibPrefab);
		p.transform.position = transform.position;
	}
#endregion

#region Public Methods
	public Rect GetRect(){
		Rect fullSizeRect = Utility.RectForBounds(sprite.bounds);
		return Utility.ScaleRect(fullSizeRect, transform.localScale.x);
	}
	public void Die(){
		_gibEffect();
		GameObject.Destroy(this.gameObject);
	}
	public void DeployParachute(){
		_parachuteLivetime = 5.0f; 
	}
	public bool ParachuteIsActive(){
		return _parachuteLivetime > 0.0f;
	}
#endregion
}
