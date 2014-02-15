using UnityEngine;
using System.Collections;

public class LoveShotInMotion : MonoBehaviour {

	public int senderID;	
	public float loveFactor;
	public float selfDestructTimer = 20;

	IEnumerator Start(){
		yield return new WaitForSeconds (selfDestructTimer);
		Network.Destroy (gameObject);
	}

    [RPC]
    public void SetVelocity(Vector3 direction, Color color)
    {
        rigidbody.velocity = direction;
    }
}
