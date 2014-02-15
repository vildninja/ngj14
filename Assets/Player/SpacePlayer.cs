using UnityEngine;
using System.Collections;
using System.Linq;

public class SpacePlayer : MonoBehaviour {

    public int id;
    public Color uiColor;
    public bool canMove;
    public NetworkPlayer net;
    public int hp = 3;

    public Transform boom;

    [RPC]
    public void Impact()
    {
        hp--;
        if (hp <= 0)
        {
            foreach (var p in FindObjectsOfType<PlanetController>())
                p.relations.Remove(p.relations.First(r => r.player == this));
        }

        Network.Instantiate(boom, transform.position, transform.rotation, 0);
    }
}
