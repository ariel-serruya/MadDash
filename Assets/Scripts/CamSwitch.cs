using UnityEngine;
using System.Collections;

public class CamSwitch : MonoBehaviour 
{
	//Global variables
	public Camera MainCamera;
	public Camera TopDownCam;
	public Camera ThirdPersonCam;
	public Camera ActionCam;
	private Camera[] cameras;
	private int currentCameraIndex = 0;
	private Camera currentCamera;
	private string camName;

	private float initX;
	private int fingerId = -1;
	
	// Use this for initialization
	void Start () 
	{
		cameras = new Camera[] { MainCamera, TopDownCam, ThirdPersonCam, ActionCam };//this is the array of cameras
		currentCamera = MainCamera; //When the program start the main camera is selected as the default camera
		//Run the function at start
		ChangeView();
	}
	
	// Update is called once per frame
	void Update()
	{
		if (Input.GetKey(KeyCode.Escape)) {
			Application.LoadLevel(0);
		}

		int count = Input.touchCount;
		bool swipe = false;
		if (count > 0) {
			for (int i = 0; i < count; i++) {
				Touch touch = Input.GetTouch(i);

				if (touch.phase == TouchPhase.Began &&
				    touch.position.x > Screen.width / 2 &&
				    touch.position.y > Screen.height / 2) {
					initX = touch.position.x;
					fingerId = touch.fingerId;
				} else if (touch.phase == TouchPhase.Ended && touch.fingerId == fingerId) {
					float deltaX = Mathf.Abs(touch.position.x - initX);
					if (deltaX > Screen.width / 6) {
						swipe = true;
					}
					fingerId = -1;
				}
			}
		}

		//If the 'v' key is pressed and released
		if (Input.GetKeyDown("v") || swipe)
		{
			//increment currentCameraIndex by 1
			currentCameraIndex++;
			//Makes sure we dont "run out of cameras" (default to Camera[0] if so)
			if (currentCameraIndex > cameras.Length - 1)
				currentCameraIndex = 0;
			//Run the function
			ChangeView();
		}
		
	}
	void ChangeView()
	{
		//Disables the currently enabled camera component
		currentCamera.enabled = false;
		//Changes camera to currentCameraIndex (the next one in the array after Update() function is called for the first time)
		currentCamera = cameras[currentCameraIndex];
		//Enables the new camera component (or main camera when called from the Start() function
		currentCamera.enabled = true;
	}
	
	void OnGUI()
	{   
		{
			//Can be improved in indexof or .replace this just works with the cameras being nearly the same length
			//camName = ("Current Camera: " + Camera.current);
			//camName = camName.Remove(camName.Length-16);
			//Debug.Log(camName.Length-16);
			//GUI.TextArea(new Rect(10, 10, 250, 30), camName);
		}
	}
}