var projectile : Rigidbody;
var speed = 1000;

function Update () {
    if(Input.GetButtonUp("Fire1")){
        clone = Instantiate(projectile, transform.position, transform.rotation);
		clone.GetComponent(MeshRenderer).enabled = true;
        clone.velocity = transform.TransformDirection(Vector3 (0, 0, 350));
        Destroy (clone.gameObject, 5);
    }
}