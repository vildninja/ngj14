using UnityEngine;
using System.Collections;

public class MessageScript : MonoBehaviour {

	public LoveShotInMotion message;
	public float messageForce = 100f;
	public int playerNumber;

	// Update is called once per frame
	void Update () {
		foreach(Touch t in Input.touches) {
			if(t.phase == TouchPhase.Began)
			{
				Vector3 touchPos = Camera.main.ScreenToWorldPoint(t.position);
				LoveShotInMotion newShot = Instantiate(message,transform.position,transform.rotation) as LoveShotInMotion;
				Vector3 heading = touchPos - transform.position;
				heading.z = 0;
				Vector3 dir = heading / heading.magnitude;
				newShot.transform.rotation = Quaternion.LookRotation(Vector3.forward,heading);
				newShot.rigidbody.AddForce(dir * messageForce);
				newShot.sender = playerNumber;
			}
		}
	}

	void OnTriggerEnter(Collider c){
		if(c.GetComponent<LoveShotInMotion>())
		{
			if (c.GetComponent<LoveShotInMotion>().sender != playerNumber) {
				c.GetComponent<LoveShotInMotion>().hateFactor = 1;
				foreach(Transform t in c.transform)
				{
					if(t.GetComponent<LoveOrHate>().isLove)
					{
						t.gameObject.SetActive(false);
					}
					else
					{
						t.gameObject.SetActive(true);
					}
				}

			}
		}
	}
}
