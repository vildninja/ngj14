using UnityEngine;
using System.Collections;
using System.Linq;

public class LoveShotInMotion : MonoBehaviour {

	public int senderID;
    public SpacePlayer player;
	public float loveFactor;
	public float selfDestructTimer = 20;

	IEnumerator Start(){
		yield return new WaitForSeconds (selfDestructTimer);
		Network.Destroy (gameObject);
	}

    [RPC]
    public void SetVelocity(Vector3 direction)
    {
        rigidbody.velocity = direction;
        if (FindObjectsOfType<SpacePlayer>().Any(sp => sp.networkView.owner == networkView.owner))
            player = FindObjectsOfType<SpacePlayer>().First(sp => sp.networkView.owner == networkView.owner);

        transform.Find("GiftTag").renderer.material.color = player.uiColor;
    }
}
