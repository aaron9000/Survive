using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour {

	// Connections
	public GameObject player;

	// Singleton Instance
	private static GameLogic _instance;

#region Unity Lifecycle
	void Awake (){
		_instance = this;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
#endregion

#region Internal Helpers
	protected Rect _getPlayerRect(){
		if (player == null){
			return new Rect(0,0,0,0);
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
#endregion
}
