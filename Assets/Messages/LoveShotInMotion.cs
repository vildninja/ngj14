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
    public void SetVelocity(Vector3 direction, int id)
    {
        senderID = id;
        rigidbody.velocity = direction;
        if (FindObjectsOfType<SpacePlayer>().Any(sp => sp.networkView.owner == networkView.owner))
            player = FindObjectsOfType<SpacePlayer>().First(sp => sp.networkView.owner == networkView.owner);

        transform.Find("GiftTag").renderer.material.color = player.uiColor;
    }

    [RPC]
    public void Turd()
    {
        loveFactor = -1;
        foreach (Transform t in transform)
        {
            if (t.GetComponent<LoveOrHate>())
            {
                if (t.GetComponent<LoveOrHate>().isLove)
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
