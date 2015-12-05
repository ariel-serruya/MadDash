using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;

public class ShootBullet : MonoBehaviour {
	//public GameObject bullet;
	private int PoolSize = 5;
	private string bulletPreFab = "PlaceHolderBullet";
	List<GameObject> bullets;
	
	// Use this for initialization
	void Start () {
		bullets = new List<GameObject> ();
		for (int i = 0; i < PoolSize; i++) {//Creates the pool of bullets
			//GameObject b = (GameObject)Instantiate(bullet);
			GameObject b = PhotonNetwork.Instantiate(bulletPreFab, transform.position, transform.rotation, 0);
			b.SetActive(false);
			bullets.Add(b);
		}
	}

	void Update(){
		if (CrossPlatformInputManager.GetButtonUp("Shoot")) {
            //Should really have a seperate event listener that manages everything. Avoiding using the update function is always good.
			Shoot ();
		}
	}

	// Update is called once per frame
	void Shoot () {
		for (int i = 0; i < bullets.Count; i++) {//Cycles through bullets in list
			if(!bullets[i].activeInHierarchy){//Ensures it doesn't grab a bullet that is already active
				bullets[i].transform.position = transform.position;//Puts bullet in correct spot
				bullets[i].transform.rotation = transform.rotation;//Rotates bullet to match the spawner
				bullets[i].SetActive(true);//Activates the bullet
				break;//Breaks so we don't active every bullet when we shoot once.
			}
		}
	}
}
