using UnityEngine;

public class About : MonoBehaviour {

	private GUIStyle gui = new GUIStyle();
	private bool show;

	void Start() {
		
		gui.normal.textColor = Color.white;
	}
	
	void OnMouseUp () {
		show = !show;
	}

	void OnGUI() {
		if (show) {
			#if UNITY_STANDALONE
				gui.fontSize = 24;
				GUI.Label (new Rect (10, 10, Screen.width, Screen.height), "Controls: \n - Move: Arrow Keys or WASD \n - Shoot: P \n - Change Camera: V \n - Quit: ESC", gui);
			#else
				gui.fontSize = 24;
				GUI.Label (new Rect (10, 10, Screen.width, Screen.height), "Controls: \n - Move: Joystick \n - Shoot: Shoot Button \n - Change Camera: Swipe (Top Right) \n - Quit: Phone Default", gui);
			#endif
			
			
		}
	}

}
