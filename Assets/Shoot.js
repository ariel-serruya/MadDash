var projectile : Rigidbody;
var projectile2 : Rigidbody;
var speed = 1000000000;

function Update () {
    if(Input.GetButtonUp("Fire1")){
        clone = Instantiate(projectile, transform.position, transform.rotation);
        clone.velocity = transform.TransformDirection(Vector3 (0, 0, 350));
        clone = Instantiate(projectile2, transform.position, transform.rotation);
        clone.velocity = transform.TransformDirection(Vector3 (0, 0, 350));
        Destroy (clone.gameObject, 5);
    }
}