using UnityEngine;
using System.Collections;

public class ScrollingBackground : MonoBehaviour {

	// Connections
	public SpriteRenderer top;	
	public SpriteRenderer bottom;	
	public float scrollSpeed = 1.0f;

	// Internal State
	private float _height = 1.0f;	

#region Unity Lifecycle

	// Use this for initialization
	void Start () {
		_height = top.sprite.texture.height;
		_scrollRenderers(-scrollSpeed * UnityEngine.Random.value);
	}
	
	// Update is called once per frame
	void Update () {
		_scrollRenderers(-scrollSpeed * Time.deltaTime);
	}
#endregion

#region Helpers
	private void _scrollRenderers(float amount){
		_scroll(top, amount);
		_scroll(bottom, amount);
	}
	private void _scroll(SpriteRenderer renderer, float amount){
		renderer.transform.position += new Vector3(0, amount, 0);
		if (renderer.transform.position.y < -_height){
			renderer.transform.position += new Vector3(0, _height, 0);					
		}
	}
#endregion
}
