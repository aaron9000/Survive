using UnityEngine;
using System.Collections;

public class Utility{
	public static void DumpScreenInfo(){
		Debug.Log("screen width = " + Screen.width);
		Debug.Log("screen height = " + Screen.height);
		Debug.Log("width = " + Utility.GetWidth());
		Debug.Log("height = " + Utility.GetHeight());
		Debug.Log("left = " + Utility.GetLeftEdge());
		Debug.Log("right = " + Utility.GetRightEdge());
		Debug.Log("top = " + Utility.GetTopEdge());
		Debug.Log("bottom = " + Utility.GetBottomEdge());
		Debug.Log("screenRatio = " + Utility.GetScreenRatio());
	}
	public static float GetLeftEdge(){
		return GetWidth() * -0.5f;
	}	
	public static float GetRightEdge(){
		return GetWidth() * 0.5f;
	}
	public static float GetBottomEdge(){
		return GetHeight() * -0.5f;
	}
	public static float GetTopEdge(){
		return GetHeight() * 0.5f;
	}
	public static float GetHeight(){
		return Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y * 2.0f;
	}
	public static float GetWidth(){
		return Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x * 2.0f;
	}
	public static float GetSpriteHeight(SpriteRenderer sprite){
		float scaleRatio = sprite.sprite.texture.height / Constants.ScreenHeight;
		Debug.Log(sprite.sprite.texture.height);
		return scaleRatio * GetHeight();
	}
	public static float GetSpriteWidth(SpriteRenderer sprite){
		float scaleRatio = sprite.sprite.texture.width / Constants.ScreenHeight;
		return scaleRatio * GetWidth();
	}
	public static float GetScreenRatio(){
		return Screen.height / Constants.ScreenHeight;
	}
	public static Vector3 NormalizedRadialSpread(){
		float angle = UnityEngine.Random.value * Mathf.PI * 2.0f;	
		float radius = Mathf.Sqrt(UnityEngine.Random.value);
		Vector2 vec2 = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
		return new Vector3(vec2.x * radius, vec2.y * radius, 0);
	}
}
