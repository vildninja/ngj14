using UnityEngine;
using System.Collections;

public class MovementScript : MonoBehaviour {
	public float force = 10f;
	public float forceMultiplierForGyro = 10f;
	public AnimationCurve curve;

	void FixedUpdate(){
		//keyboard control
		float vertical = Input.GetAxis ("Vertical") * force;
		float horizontal = Input.GetAxis ("Horizontal") * force;
		rigidbody.AddForce (new Vector3 (horizontal, vertical, 0));

		// we assume that device is held parallel to the ground
		// and Home button is in the right hand
		Vector3 dir = Vector3.zero;
		
		// remap device acceleration axis to game coordinates:
		//  1) XY plane of the device is mapped onto XZ plane
		//  2) rotated 90 degrees around Y axis
		dir.x = Input.acceleration.x;
		dir.y = Input.acceleration.y;
		
		//dir = Quaternion.Euler(0, 90, 0) * dir;
		
		// clamp acceleration vector to unit sphere
		if (dir.sqrMagnitude > 1) {
			dir.Normalize ();
		}

		dir.x = Mathf.Sign(dir.x) * curve.Evaluate (dir.x);
		dir.z = Mathf.Sign(dir.z) * curve.Evaluate (dir.z);
		
		// Make it move 10 meters per second instead of 10 meters per frame...
		dir *= Time.deltaTime;
		
		// Move object
		rigidbody.AddForce (dir * force * forceMultiplierForGyro);
	}

	void OnGUI(){
		GUI.Label (new Rect (10, 10, 100, 100), Input.acceleration.x.ToString ()+ "     " + Input.acceleration.y.ToString());
	}

	void Update () {

	}
}
