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
    float charge = 0;
    public float loveFalloff = 0.1f;
    public float maxLove = 1;

    public Dictionary<SpacePlayer, PlayerRelation> relations;

	// Use this for initialization
	void Start ()
    {
        foreach (var sp in FindObjectsOfType<SpacePlayer>())
            relations.Add(sp, new PlayerRelation(sp));
	}
	
	// Update is called once per frame
	void Update () {
        foreach (var relation in relations.Values)
            relation.love -= loveFalloff * Time.deltaTime;

        // the fewer who loves it the faster it will shoot;
        timeToShoot += (relations.Count(r => r.Value.love < maxLove / 2) + relations.Count(r => r.Value.love < maxLove / 4) * 2) * Time.deltaTime;

        if (charge > timeToShoot)
        {
            charge = 0;
            Vector3 forward = Random.insideUnitCircle.normalized;
            var rocket = Network.Instantiate(rocketPrefab, transform.position + forward, Quaternion.LookRotation(forward), 0) as Rocket;

            PlayerRelation minLove = null;
            foreach (var rel in relations.Values)
                if (minLove == null || rel.love < minLove.love)
                    minLove = rel;

            rocket.target = minLove.player;
            minLove.love += 0.2f;
        }
	}
}
