using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{

    const string VERSION = "v0.2";
    public string roomName = "MadDash";
    public string playerPrefab = "Car";
    public Transform SpawnPoint;

    // Use this for initialization
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(VERSION);
    }

    void OnJoinedLobby()
    {
        RoomOptions roomOptions = new RoomOptions() { isVisible = false, maxPlayers = 4 };
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate(playerPrefab, SpawnPoint.position, SpawnPoint.rotation, 0);
    }
}