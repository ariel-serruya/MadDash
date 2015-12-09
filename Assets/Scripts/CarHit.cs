using UnityEngine;
using System.Collections;

public class BullCollide : MonoBehaviour
{
    void OnCollisionEnter(Collision col)
    {
        Debug.Log("collision name = ");
        /*if(col.gameObject.name == "PlaceHolderBullet")
        {
            Debug.Log("WOOPWOOPWOOP");
        }*/
    }
}