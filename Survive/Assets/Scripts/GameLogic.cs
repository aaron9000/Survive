using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour {

	// Connections
	public GameObject playerPrefab;
	public GameObject mainMenuPrefab;
	public GameObject replayMenuPrefab;
	public GameObject scoreBannerPrefab;

	// Internal State
	private bool _playerDead = false;
	private float _gameTime = 0.0f;
	private float _speedRatio = 1.0f;
	private float _lastSpeedScale = 1.0f;
	private Player _player;
	private GameState _gameState = GameState.Launch;

	// Singleton Instance
	private static GameLogic _instance;

	// Constants
	private const float MIN_SPEED_SCALE = 2.5f;
	private const float MAX_SPEED_SCALE = 5.0f;
	private const float MIN_SPAWN_DELAY = 0.5f;
	private const float MAX_SPAWN_DELAY = 1.5f;
	private const float RAMP_UP_TIME = 60.0f;
	private const float POWERUP_SLOW_FACTOR = 0.5f;

	public enum GameState{
		Menu = 0,
		Game = 1,
		Replay = 2,
		Launch = 3
	}

#region Unity Lifecycle
	void Awake (){
		_instance = this;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		// Dealing with various gamestates
		switch (_gameState) {
			case GameState.Launch:
				_handleLaunch();
				break;
			case GameState.Game:
				_handleGameplay();
				break;
			case GameState.Menu:
				_handleMenu();
				break;
			case GameState.Replay:
				_handleReplay();
				break;
		}

		_gameTime += Time.deltaTime;
	}
#endregion


#region Gamestate Helpers
	private void _handleGameplay (){
		// Spawn player if one does not exist
		if (_player == null) {
			_spawnPlayer();
		}
	}
	private void _handleMenu (){
		// Animate menu in
		bool menuIsInPlace = true;
		if (menuIsInPlace == false) {
			// Animate in
			
			return;
		}

		// If done, tapping enters game
		if (Input.GetKey(KeyCode.Space)){
			_launchGame();
		}
	}
	private void _handleReplay (){
		// Animate counter in
		bool menuIsInPlace = true;
		if (menuIsInPlace == false) {
			// Animate in

			return;
		}

		// If done, press enters game
		if (Input.GetKey(KeyCode.Space)){
			_launchGame();
		}
	}
	private void _handleLaunch(){

		// Move to menu
		_gameState = GameState.Menu;
	}
	private void _launchGame(){
		_gameState = GameState.Game;
		_gameTime = 0.0f;
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag (Constants.ObstacleTag);
		foreach (GameObject obstacle in obstacles) {
			GameObject.Destroy(obstacle);
		}
	}
#endregion

#region Internal Helpers
	protected void _spawnPlayer(){
		GameObject p = (GameObject)GameObject.Instantiate(playerPrefab);
		p.transform.position = new Vector3(Utility.GetMiddleX(), Utility.GetBottomEdge() + (Utility.GetHeight() * 1.2f), 0);
		_player = p.GetComponentInChildren<Player>();
	}
	protected Rect _getPlayerRect(){
		if (_player == null){
			return new Rect(10000, 10000, 0, 0);
		}
		return _player.GetRect();
	}
	protected void _killPlayer(){
		if (_player == null){
			return;
		}
		_player.Die();
		_player = null;
		_gameState = GameState.Replay;
	}
	protected void _awardPowerup(){
		if (_player == null){
			return;
		}
		_player.DeployParachute();
	}
	protected float _getSpeedScale(){
		float speedScale = _lastSpeedScale;
		if (_gameState == GameState.Game){
			float delta = MAX_SPEED_SCALE - MIN_SPEED_SCALE;
			bool powerupActive = false;
			speedScale = Mathf.Clamp01(_gameTime / RAMP_UP_TIME) * delta + MIN_SPEED_SCALE;
			if (_player != null){
				if (_player.ParachuteIsActive()){
					powerupActive = true;
				}
			}
			if (powerupActive){
				_speedRatio -= Time.deltaTime * 0.01f;
			}else{
				_speedRatio += Time.deltaTime * 0.01f;
			}
			_speedRatio = Mathf.Clamp(_speedRatio, POWERUP_SLOW_FACTOR, 1.0f);
			speedScale *= _speedRatio;
		}else{
			speedScale -= Time.deltaTime * 0.01f;
		}
		speedScale = Mathf.Clamp (speedScale, MIN_SPEED_SCALE, MAX_SPEED_SCALE);
		_lastSpeedScale = speedScale;
		return speedScale;
	}
	protected float _getObstacleDelay(){
		float delta = MAX_SPAWN_DELAY - MIN_SPAWN_DELAY;
		float spawnDelay = (1.0f - Mathf.Clamp01(_gameTime / RAMP_UP_TIME)) * delta + MIN_SPAWN_DELAY;
		return spawnDelay;
	}
	protected bool _shouldSpawnObstacle(){
		return (bool)(_gameState == GameState.Game);
	}
#endregion

#region Static Methods
	public static void KillPlayer(){
		_instance._killPlayer();
	}
	public static void AwardPowerup(){
		_instance._awardPowerup();
	}
	public static Rect GetPlayerRect() {
		return _instance._getPlayerRect();
	}
	public static float GetSpeedScale(){	
		return _instance._getSpeedScale();
	}
	public static float GetObstacleDelay(){
		return _instance._getObstacleDelay();
	}
	public static bool ShouldSpawnObstacle(){
		return _instance._shouldSpawnObstacle();
	}
#endregion
}
