using UnityEngine;
using System.Collections;

public class ScrollingSpikes : MonoBehaviour {

	// Connections
	public SpriteRenderer top;	
	public SpriteRenderer bottom;	

	// Internal State
	private float _height;

	// Constants
	const float scrollSpeed = 1.0f;

#region Unity Lifecycle

	// Use this for initialization
	void Start () {
		_height = top.sprite.texture.height;
	}
	
	// Update is called once per frame
	void Update () {
		_scroll(top);
		_scroll(bottom);
	}
#endregion

#region Helpers

	private void _scroll(SpriteRenderer renderer){
		renderer.transform.position += new Vector3(0, -scrollSpeed * Time.deltaTime, 0);
		if (renderer.transform.position.y < -_height){
			renderer.transform.position += new Vector3(0, _height, 0);					
		}
	}
#endregion
}
