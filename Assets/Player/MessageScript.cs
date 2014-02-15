using UnityEngine;
using System.Collections;

public class MessageScript : MonoBehaviour {

	public LoveShotInMotion message;
	public float messageForce = 100f;

	// Update is called once per frame
	void Update () {
		foreach(Touch t in Input.touches) {
			if(t.phase == TouchPhase.Began)
			{
				Vector3 touchPos = Camera.main.ScreenToWorldPoint(t.position);
				LoveShotInMotion newShot = Network.Instantiate(message,transform.position,transform.rotation,0) as LoveShotInMotion;
				Vector3 heading = touchPos - transform.position;
				heading.z = 0;
				Vector3 dir = heading / heading.magnitude;
				newShot.transform.rotation = Quaternion.LookRotation(Vector3.forward,heading);
				newShot.rigidbody.AddForce(dir * messageForce);
				newShot.senderID = gameObject.GetComponent<SpacePlayer>().id;
			}
		}
	}

	void OnTriggerEnter(Collider c){
		Debug.Log("0");
		if(c.GetComponent<LoveShotInMotion>())
		{
			Debug.Log("a"+c.GetComponent<LoveShotInMotion>().senderID);
			Debug.Log("b"+gameObject.GetComponent<SpacePlayer>().id);
			if (c.GetComponent<LoveShotInMotion>().senderID != gameObject.GetComponent<SpacePlayer>().id) {
				Debug.Log("b");
				c.GetComponent<LoveShotInMotion>().loveFactor = -1;
				foreach(Transform t in c.transform)
				{
					if(t.GetComponent<LoveOrHate>().isLove)
					{
						Debug.Log("c");
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
