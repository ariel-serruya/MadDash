using UnityEngine;
using System.Collections;

public class HealthPickedUp : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
    void OnCollisionEnter(Collision col)
    {
        Debug.Log("hit");
        Destroy();
        //Make blowup effect
    }
    void Destroy()
    {
        //Creating/Destroying over and over again makes the device reallocate memory multiple times which kills the cpu.
        gameObject.SetActive(false);
        Invoke("Respawn", 45f);
    }
    void Respawn()
    {
        gameObject.SetActive(true);
    }
    // Update is called once per frame
    void Update () {
	
	}
}
