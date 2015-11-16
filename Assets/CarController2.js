// Original code from: https://dl.dropboxusercontent.com/u/16956434/Car_Tutorial_Scripts/PlayerCar_Script.js

// These variables allow the script to power the wheels of the car.
var FrontLeftWheel : WheelCollider;
var FrontRightWheel : WheelCollider;

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
private var EngineRPM : float = 0.0;



function Start () {
	// I usually alter the center of mass to make the car more stable. I'ts less likely to flip this way.
	GetComponent.<Rigidbody>().centerOfMass += Vector3(0, -.75, .25);
}

function Update () {
	
	
	/*for (var touch : Touch in Input.touches) 
     {
         if (GasButton.HitTest(touch.position))
         {
             bBrakeButtonPressed = false;
             bGasButtonPressed = true;
         }
         else if (BrakeButton.HitTest(touch.position))
         {
             bGasButtonPressed = false;
             bBrakeButtonPressed = true;
         }
         
         if (LeftButton.HitTest(touch.position))
         {
             bRightButtonPressed = false;
             bLeftButtonPressed = true;
         }
         else if (RightButton.HitTest(touch.position))
         {
             bLeftButtonPressed = false;
             bRightButtonPressed = true;
         }
     }*/
	
	
	
	
	// Compute the engine RPM based on the average RPM of the two wheels, then call the shift gear function
	EngineRPM = (FrontLeftWheel.rpm + FrontRightWheel.rpm) / 2 * GearRatio[CurrentGear];
	ShiftGears();

	// set the audio pitch to the percentage of RPM to the maximum RPM plus one, this makes the sound play
	// up to twice it's pitch, where it will suddenly drop when it switches gears.
	GetComponent.<AudioSource>().pitch = Mathf.Abs(EngineRPM / MaxEngineRPM) + 1.0 ;
	// this line is just to ensure that the pitch does not reach a value higher than is desired.
	if ( GetComponent.<AudioSource>().pitch > 2.0 ) {
		GetComponent.<AudioSource>().pitch = 2.0;
	}

	// finally, apply the values to the wheels.	The torque applied is divided by the current gear, and
	// multiplied by the user input variable.
	FrontLeftWheel.motorTorque = EngineTorque / GearRatio[CurrentGear] * Input.GetAxis("Vertical");
	FrontRightWheel.motorTorque = EngineTorque / GearRatio[CurrentGear] * Input.GetAxis("Vertical");
		
	// the steer angle is an arbitrary value multiplied by the user input.
	FrontLeftWheel.steerAngle = 20 * Input.GetAxis("Horizontal");
	FrontRightWheel.steerAngle = 20 * Input.GetAxis("Horizontal");
	
	//if(Input.GetKey("Mouse0") or ){
	//}
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
	
	//Debug.Log(CurrentGear);
}