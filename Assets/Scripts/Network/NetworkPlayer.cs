using UnityEngine;
using System.Collections;

public class NetworkPlayer : Photon.MonoBehaviour {
    public GameObject myCamera;

	// Use this for initialization
	void Start () {
        myCamera.SetActive(true);

	}
}
