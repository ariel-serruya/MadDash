#pragma strict

function OnCollisionEnter (col : Collision)
{
    if(col.gameObject == "Container"){
        Destroy(this.gameObject);
        Debug.Log("DESTROYED");
    }
}