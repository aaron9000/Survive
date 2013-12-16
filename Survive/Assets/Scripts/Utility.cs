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
	public static float GetMiddleX(){
		return (GetLeftEdge() + GetWidth() * 0.5f);
	}
	public static float GetSpriteHeight(SpriteRenderer sprite){
		float scaleRatio = sprite.sprite.texture.height / Constants.SCREEN_HEIGHT;
		return scaleRatio * GetHeight();
	}
	public static float GetSpriteWidth(SpriteRenderer sprite){
		float scaleRatio = sprite.sprite.texture.width / Constants.SCREEN_HEIGHT;
		return scaleRatio * GetWidth();
	}
	public static float GetScreenRatio(){
		return Screen.height / Constants.SCREEN_HEIGHT;
	}
	public static Vector3 NormalizedRadialSpread(){
		float angle = UnityEngine.Random.value * Mathf.PI * 2.0f;	
		float radius = Mathf.Sqrt(UnityEngine.Random.value);
		Vector2 vec2 = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
		return new Vector3(vec2.x * radius, vec2.y * radius, 0);
	}
	public static bool HorizontalLineIntersect(Vector2 linePos, float lineWidth, Rect rect){
		Rect lineRect = new Rect(linePos.x - lineWidth * 0.5f, linePos.y + 0.01f, lineWidth, 0.02f);
		return Intersect(lineRect, rect);
	}
	public static bool Intersect(Rect a, Rect b) {
            FlipNegative(ref a);
            FlipNegative(ref b);
            bool c1 = a.xMin < b.xMax;
            bool c2 = a.xMax > b.xMin;
            bool c3 = a.yMin < b.yMax;
            bool c4 = a.yMax > b.yMin;
            return c1 && c2 && c3 && c4;
	}
    private static void FlipNegative(ref Rect r) {
	    if (r.width < 0)
	        r.x -= ( r.width *= -1 );
	
	    if (r.height < 0)
			r.y -= (r.height *= -1);
	}
	public static Rect RectForBounds(Bounds bounds){
		return new Rect(bounds.center.x - bounds.size.x *  0.5f, bounds.center.y - bounds.size.y *  0.5f, bounds.size.x, bounds.size.y);
	}
	public static Rect ScaleRect(Rect rect, float scale){
		Vector2 center = rect.center;
		float newWidth = rect.width * scale;
		float newHeight = rect.height * scale;
		return new Rect(center.x - newWidth * 0.5f, center.y - newHeight * 0.5f, newWidth, newHeight);
	}
}
