using UnityEngine;
using System.Collections;

public class MovementScript : MonoBehaviour {
	public float force = 10f;

	// Use this for initialization
	void Start () {
		
	}

	void FixedUpdate(){
		/*if (Input.GetAccelerationEvent) {

		}*/
	}

	void OnGUI(){
		GUI.Label (new Rect (10, 10, 100, 100), Input.acceleration.x.ToString ()+ "     " + Input.acceleration.y.ToString());
	}

	void Update () {
		//keyboard control
		float vertical = Input.GetAxis ("Vertical") * force;
		float horizontal = Input.GetAxis ("Horizontal") * force;
		rigidbody.AddForce (new Vector3 (horizontal, 0, vertical));


		Vector3 dir = Vector3.zero;
		// we assume that device is held parallel to the ground
		// and Home button is in the right hand
		
		// remap device acceleration axis to game coordinates:
		//  1) XY plane of the device is mapped onto XZ plane
		//  2) rotated 90 degrees around Y axis
		dir.x = -Input.acceleration.y;
		dir.z = Input.acceleration.x;

		dir = Quaternion.Euler(0, 90, 0) * dir;

		// clamp acceleration vector to unit sphere
		if (dir.sqrMagnitude > 1) {
			dir.Normalize ();
		}

		// Make it move 10 meters per second instead of 10 meters per frame...
		dir *= Time.deltaTime;
		
		// Move object
		rigidbody.AddForce (dir * force);
	}
}
