using UnityEngine;
using System.Collections;

public class NetworkPlayer : Photon.MonoBehaviour {
    public float hp = 100;
    //public GameObject myCamera;

    /*public Transform transform = new Transform();
	private float lastSynchronizationTime = 0f;
	private float syncDelay = 0f;
	private float syncTime = 0f;
	private Vector3 syncStartPosition = transform.positon;
	private Vector3 syncEndPosition = transform.position;
	 
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(GetComponent<Rigidbody>().position);
		}
		else
		{
			syncEndPosition = (Vector3)stream.ReceiveNext();
			syncStartPosition = GetComponent<Rigidbody>().position;
	 
			syncTime = 0f;
			syncDelay = Time.time - lastSynchronizationTime;
			lastSynchronizationTime = Time.time;
		}
	}
	
	void Update()
	{
		if (photonView.isMine)
		{
			//InputMovement(); already handled by turning on CarController2 individually
		}
		else
		{
			SyncedMovement();
		}
	}
	 
	private void SyncedMovement()
	{
		syncTime += Time.deltaTime;
		GetComponent<Rigidbody>().position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
	}*/

    // Use this for initialization
    //void Start () {
    //    myCamera.SetActive(true);
    //}

    /*void Update()
    {
		Debug.Log("working");
        if (photonView.isMine) {
            Debug.Log("It's meee");
		}
    }*/

    // in an "observed" script:
    /*public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			Vector3 pos = transform.localPosition;
			stream.Serialize(ref pos);
		}
		else
		{
			Vector3 pos = Vector3.zero;
			stream.Serialize(ref pos);  // pos gets filled-in. must be used somewhere
		}
	}*/

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.name == "PlaceHolderBullet(Clone)")
        {
            hp -= 2;
            if (hp > 0)
            {
                Debug.Log(" Health:" + hp);
            }
            if (hp <0)
            {
                Debug.Log("DEAD");
            }
        }
    }

}
