using UnityEngine;
using System.Collections;
using System.Linq;

public class SpacePlayer : MonoBehaviour {

    public int id;
    public Color uiColor;
    public bool canMove;
    public int hp = 3;

    public Transform boom;

    [RPC]
    public void Impact()
    {
        hp--;
        if (hp <= 0)
        {
            foreach (var p in FindObjectsOfType<PlanetController>())
            {
                var relation = p.relations.First(r => r.player == this);
                if (relation != null)
                {
                    p.relations.Remove(relation);
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
