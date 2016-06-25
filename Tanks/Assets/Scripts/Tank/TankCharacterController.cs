using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class TankCharacterController : MonoBehaviour {

	Rigidbody rb;
	public float moveForce;

	void Start () {
		rb = GetComponent<Rigidbody> ();
	}
	
	// since its a physics body
	void FixedUpdate () {
		Vector3 move = new Vector3 (CrossPlatformInputManager.GetAxis ("Horizontal"), 0, CrossPlatformInputManager.GetAxis ("Vertical")) * moveForce;
		rb.velocity = move;
	}

	void Update()
	{
		if(CrossPlatformInputManager.GetAxis("Horizontal")!=0 && CrossPlatformInputManager.GetAxis("Vertical") != 0) // keeps the previous position when Dpad Is let go 
		transform.eulerAngles = new Vector3(transform.eulerAngles.x,Mathf.Atan2(CrossPlatformInputManager.GetAxis ("Horizontal"), CrossPlatformInputManager.GetAxis ("Vertical"))*Mathf.Rad2Deg,transform.eulerAngles.z);
	}
}
