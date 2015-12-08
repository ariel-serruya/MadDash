using UnityEngine;

public class MenuWheelRotation : MonoBehaviour {

	float rotationAmount = 720.0f;
	
	void Update() {
		transform.Rotate(rotationAmount * Time.deltaTime, 0, 0);
	}
}
