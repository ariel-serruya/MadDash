/*var rend: Renderer;


function Start() {
	rend = GetComponent.<Renderer>();
}*/


function Update () {
     GetComponent.<Renderer>().material.mainTextureScale = Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
}

/*function Update () {
	// Animates main texture scale in a funky way!
	var scaleX : float = 2.0;//float = Mathf.Cos (Time.time) * 0.5 + 1;
	var scaleY : float = 1.0;//float = Mathf.Sin (Time.time) * 0.5 + 1;
	var scaleZ : float = 2.0;
	rend.material.mainTextureScale = Vector3 (scaleX,scaleY,scaleZ);
}*/