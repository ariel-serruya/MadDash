using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	private int bulletSpeed = 300;
	// Use this for initialization
	void OnEnable () {
		Rigidbody rigidB = GetComponent<Rigidbody> ();
		rigidB.velocity = transform.forward * bulletSpeed;
		Invoke ("Destroy", 1f);//Destroys bullet 2 seconds after spawning. 
	}
	void Destroy(){
		gameObject.SetActive(false);//We don't destroy objects, we disable them and they sit in the pool again. Creating/Destroying over and over again makes the device reallocate memory multiple times which kills the cpu.
	}
	void OnDisable(){
		CancelInvoke ();//Makes sure if the bullet is disable for another reason the invokes don't stack up.
	}
}
