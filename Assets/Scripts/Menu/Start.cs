using UnityEngine;
using System.Collections;

public class Start : MonoBehaviour {
						
	void OnGUI () {
		if (GUI.Button (new Rect (500, 250, 100, 30), "Start")) {
			Application.LoadLevel(1);
		}
	}

}
