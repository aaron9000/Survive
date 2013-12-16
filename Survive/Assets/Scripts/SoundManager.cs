using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public enum SoundDef
	{
		Splat,
		Powerup,
		Click,
		Lose
	}

	// Connections
	public AudioClip splatSound;
	public AudioClip clickSound;
	public AudioClip powerupSound;
	public AudioClip loseSound;

	// Singleton
	private static SoundManager _instance;

	#region Unity Lifecycle
	void Awake () {
		_instance = this;
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	#endregion

	#region Singleton Methods
	protected void _playSound(SoundDef sound){
		AudioClip clip = null;
		switch (sound)
		{
			case SoundDef.Click:
				clip = clickSound;
				break;
			case SoundDef.Splat:
				clip = splatSound;
				break;
			case SoundDef.Lose:
				clip = loseSound;
				break;
			case SoundDef.Powerup:
				clip = powerupSound;
				break;
		}
		if (clip == null) {
			return;
		}
		AudioSource.PlayClipAtPoint (clip, Vector3.zero);
	}
	public static void PlaySound(SoundDef sound){
		_instance._playSound (sound);
	}

	#endregion
}
