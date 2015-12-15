import UnityStandardAssets.CrossPlatformInput;
// Original code from: https://dl.dropboxusercontent.com/u/16956434/Car_Tutorial_Scripts/PlayerCar_Script.js

// These variables allow the script to power the wheels of the car.
var FrontLeftWheel : WheelCollider;
var FrontRightWheel : WheelCollider;
var BackLeftWheel : WheelCollider;
var BackRightWheel : WheelCollider;

// These variables are for the gears, the array is the list of ratios. The script
// uses the defined gear ratios to determine how much torque to apply to the wheels.
var GearRatio : float[];
var CurrentGear : int = 0;

// These variables are just for applying torque to the wheels and shifting gears.
// using the defined Max and Min Engine RPM, the script can determine what gear the
// car needs to be in.
var EngineTorque : float = 500.0;
var MaxEngineRPM : float = 100000.0;
var MinEngineRPM : float = 30000.0;
var stuckTimer = 0;
public var stuckThreshold = 0.5;
public var stuckTimeThreshold = 250;
private var EngineRPM : float = 0.0;
var rb : Rigidbody;


function Start () {
    // Alter the center of mass to make the car more stable. It's less likely to flip this way.
    rb.centerOfMass += Vector3(0, -.75, .25);
}

function Update () {

    // Compute the engine RPM based on the average RPM of the two wheels, then call the shift gear function
    EngineRPM = (FrontLeftWheel.rpm + FrontRightWheel.rpm) / 2 * GearRatio[CurrentGear];

    // finally, apply the values to the wheels.	The torque applied is divided by the current gear, and
    // multiplied by the user input variable.
    FrontLeftWheel.motorTorque = EngineTorque / GearRatio[CurrentGear] * Mathf.Round(CrossPlatformInputManager.GetAxis("Vertical"));
    FrontRightWheel.motorTorque = EngineTorque / GearRatio[CurrentGear] * Mathf.Round(CrossPlatformInputManager.GetAxis("Vertical"));
    BackLeftWheel.motorTorque = EngineTorque / GearRatio[CurrentGear] * Mathf.Round(CrossPlatformInputManager.GetAxis("Vertical"));
    BackRightWheel.motorTorque = EngineTorque / GearRatio[CurrentGear] * Mathf.Round(CrossPlatformInputManager.GetAxis("Vertical"));

    // the steer angle is an arbitrary value multiplied by the user input.
    FrontLeftWheel.steerAngle = 20 * CrossPlatformInputManager.GetAxis("Horizontal");
    FrontRightWheel.steerAngle = 20 * CrossPlatformInputManager.GetAxis("Horizontal");

    if(rb.velocity.magnitude < stuckThreshold) {
        stuckTimer++;
    } else {
        stuckTimer = 0;
    }

    if(stuckTimer > stuckTimeThreshold) {
        //transform.rotation.z += 180;
        transform.rotation.x = 0;
        transform.rotation.y = 0;
        transform.rotation.z = 0;
        FrontLeftWheel.motorTorque = 0;
        BackLeftWheel.motorTorque = 0;
        FrontRightWheel.motorTorque = 0;
        FrontRightWheel.motorTorque = 0;

        //rb.AddTorque(10000, 100000, 10000000, ForceMode.Force);
        //transform.eulerAngles = new Vector3 (transform.rotation.x, transform.rotation.y, 0);
    }
}

function ShiftGears() {
    // this function shifts the gears of the vehicle, it loops through all the gears, checking which will make
    // the engine RPM fall within the desired range. The gear is then set to this "appropriate" value.
    if ( EngineRPM >= MaxEngineRPM ) {
        var AppropriateGear : int = CurrentGear;
		
        for ( var i = 0; i < GearRatio.length; i ++ ) {
            if ( FrontLeftWheel.rpm * GearRatio[i] < MaxEngineRPM ) {
                AppropriateGear = i;
                break;
            }
        }
		
        CurrentGear = AppropriateGear;
    }
	
    if ( EngineRPM <= MinEngineRPM ) {
        AppropriateGear = CurrentGear;
		
        for ( var j = GearRatio.length-1; j >= 0; j -- ) {
            if ( FrontLeftWheel.rpm * GearRatio[j] > MinEngineRPM ) {
                AppropriateGear = j;
                break;
            }
        }
		
        CurrentGear = AppropriateGear;
    }
}