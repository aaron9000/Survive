using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour {

	// Connections
	public GameObject player;

	// Internal State
	private bool _playerDead = false;
	private float _gameTime = 0.0f;

	// Singleton Instance
	private static GameLogic _instance;

	// Constants
	private const float MIN_SPEED_SCALE = 2.0f;
	private const float MAX_SPEED_SCALE = 5.0f;
	private const float MIN_SPAWN_DELAY = 0.4f;
	private const float MAX_SPAWN_DELAY = 1.6f;
	private const float RAMP_UP_TIME = 60.0f;

#region Unity Lifecycle
	void Awake (){
		_instance = this;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		_gameTime += Time.deltaTime;
	}
#endregion

#region Internal Helpers
	protected Rect _getPlayerRect(){
		if (player == null){
			return new Rect(10000,10000,0,0);
		}
		return player.GetComponent<Player>().GetRect();
	}
	protected void _killPlayer(){
		if (player == null){
			return;
		}
		player.GetComponent<Player>().Die();
		player = null;
	}
	protected void _awardPowerup(){
		if (player == null){
			return;
		}
		player.GetComponent<Player>().DeployParachute();
	}
	protected float _getSpeedScale(){
		float delta = MAX_SPEED_SCALE - MIN_SPEED_SCALE;
		float speedScale = Mathf.Clamp01(_gameTime / RAMP_UP_TIME) * delta + MIN_SPEED_SCALE;
		if (player != null){
			if (player.GetComponent<Player>().ParachuteIsActive()){
				speedScale *= 0.5f;
			}
		}
		return speedScale;
	}
	protected float _getObstacleDelay(){
		float delta = MAX_SPAWN_DELAY - MIN_SPAWN_DELAY;
		float spawnDelay = (1.0f - Mathf.Clamp01(_gameTime / RAMP_UP_TIME)) * delta + MIN_SPAWN_DELAY;
		return spawnDelay;
	}
#endregion

#region Static Methods
	public static Rect GetPlayerRect() {
		return _instance._getPlayerRect();
	}
	public static void KillPlayer(){
		_instance._killPlayer();
	}
	public static void AwardPowerup(){
		_instance._awardPowerup();
	}
	public static float GetSpeedScale(){	
		return _instance._getSpeedScale();
	}
	public static float GetObstacleDelay(){
		return _instance._getObstacleDelay();
	}
#endregion
}
