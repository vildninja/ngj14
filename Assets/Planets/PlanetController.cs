﻿using UnityEngine;
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

	// Use this for initialization
	void Start ()
    {
        if (Network.isServer)
        {
            charge = Random.Range(-timeToShoot, 0);
            relations = new List<PlayerRelation>();
            foreach (var sp in FindObjectsOfType<SpacePlayer>())
                relations.Add(new PlayerRelation(sp));
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Network.isServer)
        {
            foreach (var relation in relations)
                relation.love -= loveFalloff * Time.deltaTime;

            // the fewer who loves it the faster it will shoot;
            charge += (relations.Count(r => r.love < maxLove / 2) + relations.Count(r => r.love < maxLove / 4) * 2) * Time.deltaTime;

            if (charge > timeToShoot)
            {
                charge = 0;
                Vector3 up = Random.insideUnitCircle.normalized;
                var rocket = Network.Instantiate(rocketPrefab, transform.position + up, Quaternion.LookRotation(Vector3.forward, up), 0) as Rocket;

                PlayerRelation minLove = null;
                foreach (var rel in relations)
                    if (minLove == null || rel.love < minLove.love)
                        minLove = rel;

                minLove.love += 0.2f;

                StartCoroutine(SetRocketTarget(rocket, minLove.player));
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
                    relation.love -= c.GetComponent<LoveShotInMotion>().loveFactor;
                }
            }

        }
    }
}
