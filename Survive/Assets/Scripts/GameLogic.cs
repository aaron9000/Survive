using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour {

	// Connections
	public GameObject player;

	private static GameLogic _instance;

	void Awake (){
		_instance = this;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

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
	public static Rect GetPlayerRect() {
		return _instance._getPlayerRect();
	}
	public static void KillPlayer(){
		_instance._killPlayer();
	}
}
