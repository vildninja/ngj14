using UnityEngine;
using System.Collections;

public class MovementScript : MonoBehaviour {
	public float force = 10f;
	public float forceMultiplierForGyro = 10f;
	public AnimationCurve midCurve;
	public float midForce = 20f;
	public AnimationCurve curve;

	//wobble
	/*public AnimationCurve wobbleCurve;
	public float wobbleFactor;
	private float time;
	private float wobble = 0;*/

	//network
    private SpacePlayer sp;

    void Awake()
    {
        sp = GetComponent<SpacePlayer>();
    }

	void FixedUpdate(){
        if (networkView.owner == Network.player && sp.canMove)
        {
            //keyboard control
            float vertical = Input.GetAxis("Vertical") * force;
            float horizontal = Input.GetAxis("Horizontal") * force;
            rigidbody.AddForce(new Vector3(horizontal, vertical, 0));

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
            if (dir.sqrMagnitude > 1)
            {
                dir.Normalize();
            }

            dir.x = Mathf.Sign(dir.x) * curve.Evaluate(dir.x);
            dir.y = Mathf.Sign(dir.y) * curve.Evaluate(dir.y);

            // Make it move 10 meters per second instead of 10 meters per frame...
            dir *= Time.deltaTime;


            // Move object
            rigidbody.AddForce(dir * force * forceMultiplierForGyro);

			Vector3 towardsMiddle = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2,Screen.height/2,0))- transform.position;
			towardsMiddle.Normalize();
			towardsMiddle.x = Mathf.Sign(towardsMiddle.x)*midCurve.Evaluate(towardsMiddle.x);
			towardsMiddle.y = Mathf.Sign(towardsMiddle.y)*midCurve.Evaluate(towardsMiddle.y);

			if(rigidbody.velocity.magnitude > 5){
				towardsMiddle -= rigidbody.velocity;
			}

			rigidbody.AddForce(towardsMiddle);

			//time += Time.deltaTime;
			//wobble = curve.Evaluate (time);

			transform.up = rigidbody.velocity.normalized;
        }
	}

	void OnGUI(){
		GUI.Label (new Rect (10, 10, 100, 100), Input.acceleration.x.ToString ()+ "     " + Input.acceleration.y.ToString());
	}
}
