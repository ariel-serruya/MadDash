#pragma strict
//CarController1.js
var wheels : Transform[];
 
var enginePower=150.0;
 
var power=0.0;
var brake=0.0;
var steer=0.0;
 
var maxSteer=25.0;

function Start(){
    GetComponent(Rigidbody).centerOfMass=Vector3(0,-0.5,0.3);
}
 
function Update () {
    power=Input.GetAxis("Vertical") * enginePower * Time.deltaTime * 250.0;
    steer=Input.GetAxis("Horizontal") * maxSteer;
    brake=Input.GetKey("space") ? GetComponent.<Rigidbody>().mass * 0.1: 0.0;
   
    GetCollider(0).steerAngle=steer;
    GetCollider(1).steerAngle=steer;
   
    if(brake > 0.0){
        GetCollider(0).brakeTorque=brake;
        GetCollider(1).brakeTorque=brake;
        GetCollider(2).brakeTorque=brake;
        GetCollider(3).brakeTorque=brake;
        GetCollider(2).motorTorque=0.0;
        GetCollider(3).motorTorque=0.0;
    } else {
        GetCollider(0).brakeTorque=0;
        GetCollider(1).brakeTorque=0;
        GetCollider(2).brakeTorque=0;
        GetCollider(3).brakeTorque=0;
        GetCollider(2).motorTorque=power;
        GetCollider(3).motorTorque=power;
    }
}

function GetCollider(n : int) : WheelCollider{
    return wheels[n].gameObject.GetComponent(WheelCollider);
}