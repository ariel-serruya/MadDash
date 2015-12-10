using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;

public class NetworkManager : MonoBehaviour
{

    const string VERSION = "v4.2";
    public string roomName = "MadDash";
    public string playerPrefab = "Car";
    public string health = "Health";
    public static int numHealth = 3;
	//public string controlsPrefab = "MobileSingleStickControl";
    public Transform[] SpawnPoints = new Transform[4];
    public Transform[] HealthSpawn = new Transform[3];
	//public RectTransform SpawnPointControls;
	public GameObject controls;
	public Camera initCam;
	
	
	
	//private int PoolSize = 5;
	private string bulletPreFab = "PlaceHolderBullet";
	//List<GameObject> bullets;
	private GameObject Player;
	private Transform shooter1;
	private Transform shooter2;
	

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
	
	void Update(){
		if (Input.GetKey(KeyCode.Escape)) {
			if (PhotonNetwork.connected)
				PhotonNetwork.Disconnect();
			Application.LoadLevel(0);
		}
		if (CrossPlatformInputManager.GetButtonUp("Shoot")) {
            //Should really have a seperate event listener that manages everything. Avoiding using the update function is always good.
			GameObject b1 = PhotonNetwork.Instantiate(bulletPreFab, shooter1.transform.position, shooter1.transform.rotation, 0);
			GameObject b2 = PhotonNetwork.Instantiate(bulletPreFab, shooter2.transform.position, shooter2.transform.rotation, 0);
			//Shoot ();
			//Invoke ("Destroy", 2f);//Destroys bullet 2 seconds after spawning.	
			StartCoroutine(Destroy(b1, b2, 2));
			//Shoot();
		}
	}
	 IEnumerator Destroy(GameObject b1, GameObject b2, float delay)
	 {
		 yield return new WaitForSeconds(delay);
		 PhotonNetwork.Destroy(b1);
		 PhotonNetwork.Destroy(b2);
	 }

    // Update is called once per frame
    /*void Shoot () {
		for (int i = 0; i < bullets.Count; i++) {//Cycles through bullets in list
			if(!bullets[i].activeInHierarchy){//Ensures it doesn't grab a bullet that is already active
				bullets[i].transform.position = Player.transform.position;//Puts bullet in correct spot
				bullets[i].transform.rotation = Player.transform.rotation;//Rotates bullet to match the spawner
				bullets[i].SetActive(true);//Activates the bullet
				break;//Breaks so we don't active every bullet when we shoot once.
			}
		}
	}*/

    void OnJoinedRoom()
    {
        Player = PhotonNetwork.Instantiate(playerPrefab, 
				SpawnPoints[PhotonNetwork.playerList.Length - 1].position, 
				SpawnPoints[PhotonNetwork.playerList.Length - 1].rotation, 0);
        if(PhotonNetwork.playerList.Length == 1)
        {
                for(int i = 0; i < numHealth; i++)
            {
                Debug.Log(PhotonNetwork.playerList.Length);
                Debug.Log("health spawned");
                PhotonNetwork.Instantiate(health, HealthSpawn[i].position, 
                    HealthSpawn[i].rotation, 0);
            }
        }
		//PhotonNetwork.Instantiate(controlsPrefab, SpawnPointControls.position, SpawnPointControls.rotation, 0);
		
		
		/*bullets = new List<GameObject> ();
		for (int i = 0; i < PoolSize; i++) {//Creates the pool of bullets
			//GameObject b = (GameObject)Instantiate(bullet);
			GameObject b = PhotonNetwork.Instantiate(bulletPreFab, transform.position, transform.rotation, 0);
			b.SetActive(false);
			bullets.Add(b);
		}*/
		
		
		//instantiate bullets here somehow??

		#if !UNITY_STANDALONE
			controls.SetActive(true);
		#endif
		
		//enable movement for one player only
		((MonoBehaviour)Player.GetComponent("CarController2")).enabled = true;
		
		//enable shooting for one player only
		Transform shooter = Player.transform.Find("Shooter");
		shooter1 = shooter.transform.Find("Shooter1");
		shooter2 = shooter.transform.Find("Shooter2");
		//((MonoBehaviour)shooter1.GetComponent("ShootBullet")).enabled = true;
		//((MonoBehaviour)shooter2.GetComponent("ShootBullet")).enabled = true;
		
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
			if (GUI.Button(new Rect(300, 150, 250, 100), "Create/Join Game")) {
				
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