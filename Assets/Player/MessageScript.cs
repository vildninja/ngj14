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

				//Find nearest Planet
				PlanetController nearest = null;
				float distance = float.MaxValue;
				foreach(PlanetController pc in FindObjectsOfType<PlanetController>()){
					if (nearest == null || Vector3.Distance(touchPos,pc.transform.position) < distance){
						nearest = pc;
						distance = Vector3.Distance(touchPos,pc.transform.position);
					}
				}

				LoveShotInMotion newShot = Network.Instantiate(message,transform.position,transform.rotation,0) as LoveShotInMotion;
				Vector3 heading = nearest.transform.position - transform.position;
				heading.z = 0;
				Vector3 dir = heading / heading.magnitude;
				newShot.transform.rotation = Quaternion.LookRotation(Vector3.forward,heading);
				newShot.rigidbody.AddForce(dir * messageForce);
				newShot.senderID = gameObject.GetComponent<SpacePlayer>().id;
			}
		}
	}

	void OnTriggerEnter(Collider c){
		if(c.GetComponent<LoveShotInMotion>())
		{
			if (c.GetComponent<LoveShotInMotion>().senderID != gameObject.GetComponent<SpacePlayer>().id) {
				c.GetComponent<LoveShotInMotion>().loveFactor = -1;
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
