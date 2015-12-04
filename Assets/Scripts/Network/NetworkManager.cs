using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{

    const string VERSION = "v4.2";
    public string roomName = "MadDash";
    public string playerPrefab = "Car";
	//public string controlsPrefab = "MobileSingleStickControl";
    public Transform SpawnPoint;
	//public RectTransform SpawnPointControls;
	public GameObject controls;
	public Camera initCam;

    // Use this for initialization
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(VERSION);
		//GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
		/*Debug.Log(PhotonNetwork.connectionStateDetailed.ToString());
		while (!PhotonNetwork.connected) {
			Debug.Log("Connecting");
		}
		RoomOptions roomOptions = new RoomOptions() { isVisible = false, maxPlayers = 4 };
		PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);*/
    }

    /*void OnJoinedLobby()
    {
        RoomOptions roomOptions = new RoomOptions() { isVisible = false, maxPlayers = 4 };
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }*/

    void OnJoinedRoom()
    {
        GameObject Player = PhotonNetwork.Instantiate(playerPrefab, SpawnPoint.position, SpawnPoint.rotation, 0);
		//PhotonNetwork.Instantiate(controlsPrefab, SpawnPointControls.position, SpawnPointControls.rotation, 0);
		#if UNITY_IPHONE
			controls.SetActive(true);
		#endif
		#if UNITY_ANDROID
			controls.SetActive(true);
		#endif
		
		
		((MonoBehaviour)Player.GetComponent("CarController2")).enabled = true; //make true and make prefab with false
		Transform cameras = Player.transform.Find("Cameras");
		((MonoBehaviour)cameras.GetComponent("CamSwitch")).enabled = true;
		
		
		//Disables the currently enabled camera component
		//initCam.enabled = false;
		//Changes camera to currentCameraIndex (the next one in the array after Update() function is called for the first time)
		//currentCamera = cameras[currentCameraIndex];
		//Enables the new camera component (or main camera when called from the Start() function
		//Transform cameras = (Player.transform.Find("Cameras"));
		//Camera cam = cameras.Find("MainCamera");
    }
	
	/*************************************************************************************/
	
	private RoomInfo[] roomsList;
	 
	void OnGUI()
	{
		if (!PhotonNetwork.connected)
		{
			GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
		}
		else if (PhotonNetwork.room == null)
		{
			// Create Room
			if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server")) {
				
				RoomOptions roomOptions = new RoomOptions() { isVisible = false, maxPlayers = 4 };
				PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
				// in the "plain api", call lbClient.JoinOrCreateRoom() accordingly
				
					//PhotonNetwork.CreateRoom(roomName+roomsList.Length+1, roomOptions, TypedLobby.Default);
					//PhotonNetwork.CreateRoom(roomName + Guid.NewGuid().ToString("N"), true, true, 5);
			}
	 
			// Join Room
			if (roomsList != null)
			{
				for (int i = 0; i < roomsList.Length; i++)
				{
					if (GUI.Button(new Rect(100, 250 + (110 * i), 250, 100), "Join " + roomsList[i].name))
						PhotonNetwork.JoinRoom(roomsList[i].name);
				}
			}
		}
	}
	 
	void OnReceivedRoomListUpdate()
	{
		roomsList = PhotonNetwork.GetRoomList();
	}
}