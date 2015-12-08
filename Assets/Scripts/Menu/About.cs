using UnityEngine;

public class About : MonoBehaviour {

	bool show;

	void OnMouseUp () {
		show = !show;
	}

	void OnGUI() {
		if (show) {
			GUIStyle gui = new GUIStyle();
			#if UNITY_STANDALONE
				gui.fontSize = 24;
			#else
				gui.fontSize = 128;
			#endif

			GUI.Label (new Rect (10, 10, Screen.width, Screen.height), "Swipe from top right quarter of screen to change camera", gui);
		}
	}

}
