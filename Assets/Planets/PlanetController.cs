using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlanetController : MonoBehaviour {

    [System.Serializable]
    public class PlayerRelation
    {
        public SpacePlayer player;
        public float love;

        public PlayerRelation(SpacePlayer sp)
        {
            player = sp;
            love = 1;
        }
    }

    public Rocket rocketPrefab;
    public float timeToShoot;
    public float charge;
    public float loveFalloff = 0.1f;
    public float maxLove = 1;

    public List<PlayerRelation> relations;
    public Dictionary<SpacePlayer, RelationSpiral> spirals;

    public RelationSpiral spiralPrefab;

	public Material[] faces;

	public Transform child;

	// Use this for initialization
	void Start ()
    {
        if (Network.isServer)
        {
            charge = Random.Range(-timeToShoot, 0);
        }
        relations = new List<PlayerRelation>();
        var players = FindObjectsOfType<SpacePlayer>();
        foreach (var sp in players)
            relations.Add(new PlayerRelation(sp));

        spirals = new Dictionary<SpacePlayer, RelationSpiral>();
        for (int i = 0; i < players.Length; i++)
        {
            var s = Instantiate(spiralPrefab, transform.position, Quaternion.Euler(0, 0, (i * 360) / (float)players.Length)) as RelationSpiral;
            s.GetComponentInChildren<Renderer>().material.color = players[i].uiColor;
            s.zOne = 1 / (float)players.Length;
            spirals.Add(players[i], s);
        }
	}

    // Update is called once per frame
    void Update()
    {
        if (Network.isServer)
        {
            foreach (var relation in relations)
            {
                if (relation.love > 0)
                {
                    relation.love -= loveFalloff * Time.deltaTime;
                }

                if (relation.love <= 0)
                {
                    Vector3 up = Random.insideUnitCircle.normalized;
                    var rocket = Network.Instantiate(rocketPrefab, transform.position + up, Quaternion.LookRotation(Vector3.forward, up), 0) as Rocket;


                    relation.love += 0.4f;

                    StartCoroutine(SetRocketTarget(rocket, relation.player));
                }
            }
        }

        foreach (var rel in relations)
            spirals[rel.player].value = rel.love;

		foreach (var rel in relations) {
			if(rel.player.networkView.owner == Network.player){
				if(rel.love > 0.7){
					child.GetComponentInChildren<MeshRenderer> ().material = faces [0];
				}
				else if(rel.love > 0.2){
					child.GetComponentInChildren<MeshRenderer> ().material = faces [1];
				}
				else{
					child.GetComponentInChildren<MeshRenderer> ().material = faces [2];
				}
			}
		}
	}

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        foreach (var rel in relations)
        {
            stream.Serialize(ref rel.love);
        }
    }

    IEnumerator SetRocketTarget(Rocket rocket, SpacePlayer target)
    {
        yield return false;

        rocket.SetTarget(target.networkView.owner);
        rocket.networkView.RPC("SetTarget", RPCMode.Others, target.networkView.owner);
    }

	//When hit by a message, the love for the matching relation is decreased by the message's loveFactor. 
    void OnTriggerEnter(Collider c)
    {
        if (Network.isServer && c.GetComponent<LoveShotInMotion>())
        {
            foreach (var relation in relations)
            {
                if (relation.player.id == c.GetComponent<LoveShotInMotion>().senderID)
                {
                    relation.love = Mathf.Clamp01(relation.love + c.GetComponent<LoveShotInMotion>().loveFactor);
                }
            }
            Network.Destroy(c.networkView.viewID);
        }
    }
}
