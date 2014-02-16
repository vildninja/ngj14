using UnityEngine;
using System.Collections;
using System.Linq;

public class SpacePlayer : MonoBehaviour {

    public int id;
    public Color uiColor;
    public bool canMove;
    public int hp = 3;

    public Texture2D[] damage;

    public Transform victoryAnimation;

    public Transform boom;

    [RPC]
    public void Impact()
    {
        hp--;
		if (Network.player == networkView.owner) {
			Camera.main.GetComponent<Shake> ().SetShake (0.2f, 0.2f);
		}

        switch (hp)
        {
            case 2:
                GetComponentInChildren<Renderer>().material.mainTexture = damage[0];
                break;
            case 1:
                GetComponentInChildren<Renderer>().material.mainTexture = damage[1];
                break;
        }

        if (hp <= 0 && gameObject)
        {
            foreach (var p in FindObjectsOfType<PlanetController>())
            {
                if (p.relations.Any(r => r.player == this))
                {
                    var relation = p.relations.First(r => r.player == this);
                    p.relations.Remove(relation);
                    Destroy(p.spirals[relation.player].gameObject);
                    p.spirals.Remove(relation.player);
                }
            }

            if (Network.isServer)
            {
                if (boom)
                    Network.Instantiate(boom, transform.position, transform.rotation, 0);
                Network.Destroy(gameObject);
            }
        }

    }
}
