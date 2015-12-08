using UnityEngine;

public class About : MonoBehaviour {

	bool show;

	void OnMouseUp () {
		show = !show;
	}

	void OnGUI() {
		if (show) {
			GUI.Label (new Rect (100, 100, 100, 100), "Insert text for controls and credits here");
		}
	}

}
