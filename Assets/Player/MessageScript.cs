using UnityEngine;
using System.Collections;

public class MessageScript : MonoBehaviour {

	public Transform message;
	public float messageForce = 100f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		foreach(Touch t in Input.touches) {
			if(t.phase == TouchPhase.Began)
			{
				Vector3 touchPos = Camera.main.ScreenToWorldPoint(t.position);
				Transform newShot = Instantiate(message,transform.position,transform.rotation) as Transform;
				Vector3 heading = touchPos - transform.position;
				heading.z = 0;
				Vector3 dir = heading / heading.magnitude;
				newShot.transform.rotation = Quaternion.LookRotation(Vector3.forward,heading);
				newShot.rigidbody.AddForce(dir * messageForce);
				Destroy(newShot.gameObject,5);
			}
		}
	}
}
