using UnityEngine;
using System.Collections;

public class ParticleEffect : MonoBehaviour {

	// Internal State
	private float _start;
	private float _delay;
	private ParticleSystem _particle;
        
	#region Unity Lifecycle
	void Start () {
		_particle = this.GetComponent<ParticleSystem>();
		_start = _particle.startLifetime;
		_delay = _particle.startLifetime * 2.0f;        
	}
	void Update () {

		// Destroy self after the effect is done. Animate light intensity
		_delay -= Time.deltaTime;
		if (_delay < 0.0f) {
			GameObject.Destroy(this.gameObject);
			return;
		}
		float ratio = _delay / _start;
	}
	#endregion
}
