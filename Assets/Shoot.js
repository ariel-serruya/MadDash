import UnityStandardAssets.CrossPlatformInput;

var projectile : Rigidbody;
var speed = 1000000000;

function Update () {
    if(CrossPlatformInputManager.GetButtonUp("Shoot")){
        var clone = Instantiate(projectile, transform.position, transform.rotation);
		clone.GetComponent(MeshRenderer).enabled = true;
        clone.velocity = transform.TransformDirection(Vector3 (0, 10, 350));
        Destroy (clone.gameObject, 5);
    }
}