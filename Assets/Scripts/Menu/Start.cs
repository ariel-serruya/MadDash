using UnityEngine;

public class Start : MonoBehaviour {
						
	void OnMouseUp () {
		Application.LoadLevel(1);
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit();
		}
	}

}
