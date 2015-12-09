using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	private int bulletSpeed = 300;
	// Use this for initialization
	void OnEnable () {
		Rigidbody rigidB = GetComponent<Rigidbody> ();
		rigidB.velocity = transform.forward * bulletSpeed;
		Invoke ("Destroy", 2f);//Destroys bullet 2 seconds after spawning. 
	}
    void OnCollisionEnter(Collision col)
    {
        Debug.Log("hit");
        Destroy();
        //Make blowup effect
    }
    void Destroy(){
		//Creating/Destroying over and over again makes the device reallocate memory multiple times which kills the cpu.
		gameObject.SetActive(false);
	}
	void OnDisable(){
		CancelInvoke ();//Makes sure if the bullet is disable for another reason the invokes don't stack up.
	}
}
