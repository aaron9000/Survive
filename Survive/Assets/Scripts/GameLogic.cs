using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour {

	// Connections
	public GameObject playerPrefab;
	public GameObject mainMenuPrefab;
	public GameObject replayMenuPrefab;
	public GameObject scoreBannerPrefab;
	public Font customFont;

	// Internal State
	private float _gameTime = 0.0f;
	private float _speedRatio = 1.0f;
	private float _lastSpeedScale = 1.0f;
	private float _distance = 0.0f;
	private Player _player;
	private GameState _gameState = GameState.Launch;
	private GameObject _currentMenu = null;
	private GUIStyle _style;


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
		_style = new GUIStyle ();
		_style.font = customFont;
		_style.fontSize = 56;
		_style.alignment = TextAnchor.MiddleCenter;
		_style.normal.textColor = Color.white;
	}
	
	// Update is called once per frame
	void Update () {

		_gameTime += Time.deltaTime;
		if (_player != null) {
			_distance += Time.deltaTime * _getSpeedScale () * 2.5f;
		}
	}

	void OnGUI(){
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
	}
	#endregion


#region Gamestate Helpers
	private string _getScoreText(){
		return ((int)(_distance)).ToString ();
	}
	private void _handleGameplay (){
		// Bring counter in first
		if (_translateMenuAndCheckPlacement () == false) {
			return;
		}

		// Show text
		Rect labelRect = new Rect (355, 42, 100, 64);
		GUI.Label (labelRect, _getScoreText(), _style);

		// Spawn player if one does not exist
		if (_player == null) {
			_spawnPlayer();
		}
	}
	private void _handleMenu (){
		// Animate menu in
		if (_translateMenuAndCheckPlacement () == false) {
			return;
		}

		if (_shouldContinue()) {
			_launchGame();
		}
	}
	private bool _shouldContinue(){
		return (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0));
	}
	private bool _translateMenuAndCheckPlacement(){
		bool menuIsInPlace = _currentMenu.transform.position.y <= Utility.GetTopEdge();
		if (menuIsInPlace == false) {
			_currentMenu.transform.Translate(new Vector3(0, Time.deltaTime * -6.0f, 0));
		}
		return menuIsInPlace;
	}
	private void _handleReplay (){
		// Animate counter in
		if (_translateMenuAndCheckPlacement () == false) {
			return;
		}

		// Show text
		Rect labelRect = new Rect (265, 420, 100, 64);
		GUI.Label (labelRect, _getScoreText(), _style);

		// If done, press enters game
		if (_shouldContinue()) {
			_launchGame();
		}
	}
	private void _handleLaunch(){

		// Move to menu
		_showMenu ();
	}
	private void _showReplay(){
		_gameState = GameState.Replay;
		_openMenu ();
	}
	private void _showMenu(){
		_gameState = GameState.Menu;
		_openMenu ();
	}
	private void _launchGame(){
		_gameState = GameState.Game;
		_gameTime = 0.0f;
		_distance = 0.0f;
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag (Constants.ObstacleTag);
		foreach (GameObject obstacle in obstacles) {
			GameObject.Destroy(obstacle);
		}
		_openMenu ();
	}
	private void _openMenu(){
		if (_currentMenu != null){
			GameObject.Destroy(_currentMenu);
			_currentMenu = null;
		}
		GameObject prefab = null;
		switch (_gameState){
			case GameState.Game:
				prefab = scoreBannerPrefab;
				break;
			case GameState.Menu:
				prefab = mainMenuPrefab;
				break;
			case GameState.Replay:
				prefab = replayMenuPrefab;
				break;
		}
		if (prefab == null) {
			return;
		}
		_currentMenu = (GameObject)GameObject.Instantiate (prefab);
		float menuHeight = _currentMenu.GetComponentInChildren<SpriteRenderer>().renderer.bounds.size.y;
		_currentMenu.transform.position = new Vector3 (Utility.GetMiddleX(), Utility.GetTopEdge() + menuHeight, 0);
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
		_showReplay ();
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
