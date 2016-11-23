#pragma strict

/* @author: Boca Alexandru "alexandru.boca@assist.ro"
 * @company: Assist Software
 * This behaviour uses the rotation rate of the gyroscope, maps 
 * the values to interact with the object only when the device is
 * moving and generates a smooth rotation to the object that is set.
 * This component is best used for the camera when aiming.
 */

//gyroscope constructor 
private var gyro : Gyroscope;

//debug filter for this component
public var onDebug : boolean = false;

//check if the device has a gyroscope
private var enabledGyro : boolean = false;

//offset rotation sensibility
//@SerializeField 
static var sensitivity : float = 90.0;

/* reference for the axes we want to detect changes 
 * NOTE: All axes can be used, depending on the type of
 * 		 control we want.
 */
private var x : float;
private var y : float;


function Awake() {
	//check if the gyro is enabled
	#if UNITY_IPHONE
		SetUpGyro();
	#endif

	#if UNITY_ANDROID
		SetUpGyro();
	#endif

	#if UNITY_WINDOWS_PHONE
		SetUpGyro();
	#endif

	#if UNITY_EDITOR 
		ToDebug("No gyro in the Unity Editor, use Unity Remote to test the functionality!");
	#endif

}

function SetUpGyro() {
	//force activate the gyroscope of the device, for Android,
	//in iOS is activated by default
	Input.gyro.enabled = true;
	
	if(Input.gyro.enabled){
			enabledGyro = true;
		} else {
			// show a error message for the devices without a gyroscope
			ToErrorDebug("The device's gyroscope can't be detected");
		}
	//debug
	ToDebug("Gyro Enabled: " + Input.gyro.enabled);
}

function Start () {

}

function Update() {
	
	//if platform has a gyroscope use the rotation rate of the device to aim
	if(enabledGyro){
		GyroRotation();
	}

	//test the camera rotation sensitivity in the editor
	#if UNITY_EDITOR 
		calibrateMovementEditor();
	#endif

}

function GyroRotation(){
	//get values from the gyroscope
	x = Input.gyro.rotationRate.x;
	y = Input.gyro.rotationRate.y;

	//map the x rotationRate for continuos rotation when the device is mveing
	var xFiltered : float = FilterGyroValues(x, "x");
	RotateUpDown(xFiltered);

	//map the y rotationRate for continuos rotation when the device is moving
	var yFiltered : float = FilterGyroValues(y, "y");
	RotateRightLeft(yFiltered);
}

function FilterGyroValues(axis : float, axisName : String) : float {
	if(axis < -0.1 || axis > 0.1){
		return axis;
		ToDebug("Rotating on " + axisName + " :" + axis);
	} else {
		// ToDebug("No movememt on " + axisName + " axis");
	}
}

//override the rotation for the editor(w,a,s,d)
function calibrateMovementEditor() {
	if(Input.GetKey("w")){
		RotateUpDown(1.5);
	}

	if(Input.GetKey("s")){
		RotateUpDown(-1.5);
	}
	
	if(Input.GetKey("a")){
		RotateRightLeft(1.5);
	}

	if(Input.GetKey("d")){
		RotateRightLeft(-1.5);
	}
}

//rotate the camera up and down(x rotation)
function RotateUpDown(axis : float){
	transform.RotateAround(transform.position , transform.right, -axis * Time.deltaTime * sensitivity);
}

//rotate the camera rigt and left (y rotation)
function RotateRightLeft(axis : float){
 	transform.RotateAround(transform.position, Vector3.up, -axis * Time.deltaTime * sensitivity);
}

// use this to see realtime values in the console when building to device
function debug3DSensors(){
	if(enabledGyro){
		ToDebug("Gyro attitude: " + Input.gyro.attitude);

		ToDebug("Gyro gravity: " + Input.gyro.gravity);

		ToDebug("Gyro rotation rate " + Input.gyro.rotationRate);
	}
}

function ToDebug(info) {
	if(onDebug) {
		Debug.Log("GyroRotate: \n" + info);
	}
}

function ToErrorDebug(info) {
	if(onDebug) {
		Debug.LogError("GyroRotate: \n" + info);
	}
}