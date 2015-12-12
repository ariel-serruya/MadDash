using UnityEngine;
using System.Collections;

public class isAlive : MonoBehaviour {

	public bool status = true;
	
	public void playerDied () {
		status = false;
	}
	
	public bool getPlayerStatus () {
		return status;
	}
}
