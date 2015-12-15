using UnityEngine;

public class About : MonoBehaviour {

	private GUIStyle gui = new GUIStyle();
	private bool show;
	private GameObject logo;

	void Start() {
		logo = GameObject.Find("Logo");
		gui.normal.textColor = Color.white;
	}
	
	void OnMouseUp () {
		show = !show;
	}

	void OnGUI() {
		if (show) {
			logo.SetActive(false);

			#if UNITY_STANDALONE
				gui.fontSize = 24;
				GUI.Label (new Rect (10, 10, Screen.width, Screen.height), "Controls: \n - Move: Arrow Keys or WASD \n - Shoot: P \n - Change Camera: V \n - Quit: ESC\n - Meteor Storm: O\n\n Collect 3 meteors to get Meteor Storm\n Collect wrenches to heal", gui);
			#else
				gui.fontSize = 24;
				GUI.Label (new Rect (10, 10, Screen.width, Screen.height), "Controls: \n - Move: Joystick \n - Shoot: Shoot Button \n - Change Camera: Swipe Left/Right \n - Quit: Phone Default\n - Meteor Storm: Swipe down \n\n Collect 3 meteors to get Meteor Storm\n Collect wrenches to heal", gui);
			#endif
		} else {
			logo.SetActive(true);
		}
	}

}
