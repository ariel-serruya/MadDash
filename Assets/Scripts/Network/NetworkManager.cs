using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{

    const string VERSION = "v4.2";
    public string roomName = "MadDash";
    public string playerPrefab = "Car";
	//public string controlsPrefab = "MobileSingleStickControl";
    public Transform[] SpawnPoints = new Transform[4];
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
        GameObject Player = PhotonNetwork.Instantiate(playerPrefab, 
				SpawnPoints[PhotonNetwork.playerList.Length - 1].position, 
				SpawnPoints[PhotonNetwork.playerList.Length - 1].rotation, 0);
		//PhotonNetwork.Instantiate(controlsPrefab, SpawnPointControls.position, SpawnPointControls.rotation, 0);
		#if UNITY_IPHONE
			controls.SetActive(true);
		#endif
		#if UNITY_ANDROID
			controls.SetActive(true);
		#endif
		
		//enable movement for one player only
		((MonoBehaviour)Player.GetComponent("CarController2")).enabled = true;
		
		//enable shooting for one player only
		Transform shooter = Player.transform.Find("Shooter");
		Transform shooter1 = shooter.transform.Find("Shooter1");
		Transform shooter2 = shooter.transform.Find("Shooter2");
		((MonoBehaviour)shooter1.GetComponent("ShootBullet")).enabled = true;
		((MonoBehaviour)shooter2.GetComponent("ShootBullet")).enabled = true;
		
		//enable camera switch for individual player
		Transform cameras = Player.transform.Find("Cameras");
		((MonoBehaviour)cameras.GetComponent("CamSwitch")).enabled = true;
		
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