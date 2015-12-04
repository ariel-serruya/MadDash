using UnityEngine;
using System.Collections;

public class NetworkPlayer : Photon.MonoBehaviour {
    //public GameObject myCamera;

	// Use this for initialization
	//void Start () {
    //    myCamera.SetActive(true);
	//}
	
	void Update()
    {
		Debug.Log("working");
        if (photonView.isMine) {
            Debug.Log("It's meee");
		}
    }
	
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
}
