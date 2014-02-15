using UnityEngine;
using System.Collections;

public class pushTowardsMiddle : MonoBehaviour {
	public float pushForce = 0.1f;
	public int direction;

	void OnTriggerStay(Collider c){
		if (c.GetComponent<Rigidbody> ()) {
			if(direction == 1){
				c.rigidbody.AddForce (Vector3.up*pushForce);
			}
			else if(direction == 2){
				c.rigidbody.AddForce (Vector3.left*pushForce);
			}
			else if(direction == 3){
				c.rigidbody.AddForce (Vector3.down*pushForce);
			}
			else if(direction == 4){
				c.rigidbody.AddForce (Vector3.right*pushForce);
			}
		}
	}

}