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
	public string meteor = "Health1";//"Meteor";
	public static int numMeteors = 3;
    public static int numHealth = 2;
	//public string controlsPrefab = "MobileSingleStickControl";
    public Transform[] SpawnPoints = new Transform[4];
    public Transform[] HealthSpawn = new Transform[2];
	public Transform[] MeteorSpawn = new Transform[2];
	//public RectTransform SpawnPointControls;
	public GameObject controls;
	public Camera initCam;
	private int numAlivePlayers = -1;
	private bool gameInitiated = false;
	
	//private int PoolSize = 5;
	private string bulletPreFab = "PlaceHolderBullet";
	//List<GameObject> bullets;
	private GameObject Player;
	private Transform shooter1;
	private Transform shooter2;
	
    // Use this for initialization
    void Start()
    {
		gui.normal.textColor = Color.white;
		gui.fontSize = 36;
		
        PhotonNetwork.ConnectUsingSettings(VERSION);
    }
	
	void Update(){
		if (Input.GetKey(KeyCode.Escape)) { ///    add: && Player != null ??? prevent quit after death?
			if (PhotonNetwork.connected)
				PhotonNetwork.Disconnect();
			Application.LoadLevel(0);
		}
		if (CrossPlatformInputManager.GetButtonUp("Fire1") && Player != null && Player.GetComponent<PlayerHealth>().getNumMeteors() >=  3) {
			
			/*public float thrust;
	public Vector3 Dir;
	private Rigidbody rb;
	private ParticleSystem Ps;
	public GameObject FxPrefabLOW, FxPrefabHIGH;
	public GameObject Anim;*/Time.timeScale = 1f;
		
	//Rigidbody rb;MeshRenderer mh;Vector3 Dir = shooter1.transform.position;//new Vector3(0, 0, 0);
			//Debug.Log("create fire");
			

			GameObject fx = PhotonNetwork.Instantiate("MeteorSwarm", new Vector3(0,0,0), Quaternion.identity, 0);
			Player.GetComponent<PlayerHealth>().resetMeteors();
			//fx.transform.localScale += new Vector3(3F, 3F, 3F);
			//fx.transform.parent = this.gameObject.transform;
			//fx.transform.localPosition = Vector3.zero;
			//fx.transform.localRotation = Quaternion.Euler(320,-90,0);
			//rb = fx.GetComponent<Rigidbody>();
			//mh = fx.GetComponent<MeshRenderer>();
			//mh.enabled = true;
			//rb.AddForce(Dir * 2);//thrust);
			//DESTROY FIRE!
			StartCoroutine(DestroyFX(fx, 10));
		}
		if (CrossPlatformInputManager.GetButtonUp("Shoot") && Player != null) {	
            //Should really have a seperate event listener that manages everything. Avoiding using the update function is always good.
			GameObject b1 = PhotonNetwork.Instantiate(bulletPreFab, shooter1.transform.position, shooter1.transform.rotation, 0);
			GameObject b2 = PhotonNetwork.Instantiate(bulletPreFab, shooter2.transform.position, shooter2.transform.rotation, 0);
            AudioSource audio = GetComponent<AudioSource>();
            audio.Play();
            //Invoke ("Destroy", 2f);//Destroys bullet 2 seconds after spawning.	
            StartCoroutine(Destroy(b1, b2, 2));
		}
	}
	 IEnumerator DestroyFX(GameObject fx, float delay)
	 {
		 yield return new WaitForSeconds(delay);
		 PhotonNetwork.Destroy(fx);
	 }
	 
	 IEnumerator Destroy(GameObject b1, GameObject b2, float delay)
	 {
		 yield return new WaitForSeconds(delay);
		 PhotonNetwork.Destroy(b1);
		 PhotonNetwork.Destroy(b2);
	 }
	 
	 IEnumerator Disconnect(float delay)
	 {
		 yield return new WaitForSeconds(delay);
		 if (PhotonNetwork.connected)
				PhotonNetwork.Disconnect();
		 Application.LoadLevel(0);
	 }

    void OnJoinedRoom()
    {
        Player = PhotonNetwork.Instantiate(playerPrefab, 
				SpawnPoints[PhotonNetwork.playerList.Length - 1].position, 
				SpawnPoints[PhotonNetwork.playerList.Length - 1].rotation, 0);
        if(PhotonNetwork.playerList.Length == 1)
        {
                for(int i = 0; i < numHealth; i++)
				{
					PhotonNetwork.Instantiate(health, HealthSpawn[i].position, 
						HealthSpawn[i].rotation, 0);
				}
				for(int i = 0; i < numMeteors; i++)
				{
					PhotonNetwork.Instantiate(meteor, MeteorSpawn[i].position, 
						MeteorSpawn[i].rotation, 0);
				}
				
		}

		#if !UNITY_STANDALONE
			controls.SetActive(true);
		#endif
		
		//enable camera switch for individual player
		Transform cameras = Player.transform.Find("Cameras");
		((MonoBehaviour)cameras.GetComponent("CamSwitch")).enabled = true;
		
		
		
		//enable shooting for one player only
		Transform shooter = Player.transform.Find("Shooter");
		shooter1 = shooter.transform.Find("Shooter1");
		shooter2 = shooter.transform.Find("Shooter2");
		//((MonoBehaviour)shooter1.GetComponent("ShootBullet")).enabled = true;
		//((MonoBehaviour)shooter2.GetComponent("ShootBullet")).enabled = true;
		
    }
	
	void OnPhotonJoinRoomFailed() {
		roomHasSpace[(int)char.GetNumericValue(myRoom[7])-1] = false;
	}
	
	/*************************************************************************************/
	
	//private RoomInfo[] roomsList;
	//private int numRooms = 0;
	private GUIStyle gui = new GUIStyle();
	private string myRoom;
	private bool[] roomHasSpace = { true, true, true, true };
	
	void OnGUI()
	{
		if (!PhotonNetwork.connected)
		{
			GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
		}
		else if (PhotonNetwork.room == null)
		{
			GUI.Label (new Rect (20, 20, Screen.width, Screen.height), "GAME LOBBY", gui);
			// Create Room
			if (roomHasSpace[0]) {
				if (GUI.Button(new Rect(150, 100, 250, 60), "Solo?")) {
					myRoom = roomName+"1Player"+Random.Range(1, 99);
					RoomOptions roomOptions = new RoomOptions() { isVisible = false, maxPlayers = 1 };
					PhotonNetwork.JoinOrCreateRoom(myRoom, roomOptions, TypedLobby.Default);			
				}
			} else {
				GUI.Label (new Rect (155, 110, Screen.width, Screen.height), "Game is full", gui);
			}
			if (roomHasSpace[1]) {
				if (GUI.Button(new Rect(150, 170, 250, 60), "Create/Join 2 Player Game")) {
					myRoom = roomName+"2Player";
					RoomOptions roomOptions = new RoomOptions() { isVisible = false, maxPlayers = 2 };
					PhotonNetwork.JoinOrCreateRoom(myRoom, roomOptions, TypedLobby.Default);		
					//PhotonNetwork.CreateRoom(roomName+(numRooms), roomOptions, TypedLobby.Default);
					//numRooms++;
				}
			} else {
				GUI.Label (new Rect (155, 180, Screen.width, Screen.height), "Game is full", gui);
			}
			if (roomHasSpace[2]) {
				if (GUI.Button(new Rect(150, 240, 250, 60), "Create/Join 3 Player Game")) {
					myRoom = roomName+"3Player";
					RoomOptions roomOptions = new RoomOptions() { isVisible = false, maxPlayers = 3 };
					PhotonNetwork.JoinOrCreateRoom(myRoom, roomOptions, TypedLobby.Default);			
				}
			} else {
				GUI.Label (new Rect (155, 250, Screen.width, Screen.height), "Game is full", gui);
			}
			if (roomHasSpace[3]) {
				if (GUI.Button(new Rect(150, 310, 250, 60), "Create/Join 4 Player Game")) {
					myRoom = roomName+"4Player";
					RoomOptions roomOptions = new RoomOptions() { isVisible = false, maxPlayers = 4 };
					PhotonNetwork.JoinOrCreateRoom(myRoom, roomOptions, TypedLobby.Default);
				}
			} else {
				GUI.Label (new Rect (155, 320, Screen.width, Screen.height), "Game is full", gui);
			}
			
			/*Debug.Log(roomsList);
			// Join Room
			if (roomsList != null)
			{
				Debug.Log(roomsList.Length);
				for (int i = 0; i < roomsList.Length; i++)
				{Debug.Log("2");
					if (GUI.Button(new Rect(100, 250 + (110 * i), 250, 100), "Join " + roomsList[i].name))
						PhotonNetwork.JoinRoom(roomsList[i].name);
				}
			}*/
		} else if ( myRoom != null && (myRoom[7]) == (PhotonNetwork.playerList.Length).ToString()[0] ) {
			//enable movement for one player only
			if (Player != null) {
				if (!gameInitiated) {
					((MonoBehaviour)Player.GetComponent("CarController2")).enabled = true;
					numAlivePlayers = PhotonNetwork.playerList.Length;
				}
				gameInitiated = true;
				if (numAlivePlayers == 1 && Player.GetComponent<PlayerHealth>().getHealth() > 0) {
					GUI.Label (new Rect (20, 40, Screen.width, Screen.height), "You won!", gui);
				}
			}
			
		} else if (gameInitiated) {
			GUI.Label (new Rect (20, 40, Screen.width, Screen.height), "Someone Quit", gui);
			StartCoroutine(Disconnect(2));
		} else {
			GUI.Label (new Rect (20, 40, Screen.width, Screen.height), "WAITING FOR OTHER PLAYERS", gui);
		}
	}
	
	public void playerDied() {
		//Debug.Log("trying to call RPC");
		GetComponent<PhotonView>().RPC("numPlayersDecrement",PhotonTargets.All);
	}
	
	[PunRPC]
	void numPlayersDecrement()
	{
		//Debug.Log("RPC called");
		numAlivePlayers--;
	}
	
	public GameObject getPlayer() {
		return Player;
	}
	
	public void meteorStorm() {
		Time.timeScale = 1f;
		GameObject fx = PhotonNetwork.Instantiate("MeteorSwarm", new Vector3(0,0,0), Quaternion.identity, 0);
		Player.GetComponent<PlayerHealth>().resetMeteors();
		StartCoroutine(DestroyFX(fx, 10));
	}
	
	/*void OnReceivedRoomListUpdate()
	{
		roomsList = PhotonNetwork.GetRoomList();
	}*/
}