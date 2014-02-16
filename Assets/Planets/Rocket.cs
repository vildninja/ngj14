using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Rocket : MonoBehaviour {

    public SpacePlayer target = null;
    public Transform boom;

    public List<Transform> avoid;
    Vector3 direction = Vector3.zero;

    public Texture2D[] textures;

	public float speedFactor;

	// Use this for initialization
	IEnumerator Start () {

        if (Network.isServer)
        {
            yield return new WaitForSeconds(10);

            if (boom)
                Network.Instantiate(boom, transform.position, transform.rotation, 0);
            Network.Destroy(networkView.viewID);
        }
	}

    [RPC]
    public void SetTarget(NetworkPlayer np)
    {
        if (FindObjectsOfType<SpacePlayer>().Any(sp => sp.networkView.owner == np))
        {
            target = FindObjectsOfType<SpacePlayer>().First(sp => sp.networkView.owner == np);
            GetComponentInChildren<Renderer>().material.mainTexture = textures[target.id - 1];

            avoid = new List<Transform>();
            foreach (var sp in FindObjectsOfType<SpacePlayer>())
                if (sp != target)
                    avoid.Add(sp.transform);
            foreach (var p in FindObjectsOfType<PlanetController>())
                avoid.Add(p.transform);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + direction);
    }

	// Update is called once per frame
	void FixedUpdate () {
        if (target == null)
        {
            return;
        }

        if (!target.gameObject)
        {
            if (boom)
                Instantiate(boom, transform.position, transform.rotation);
            Destroy(gameObject);
            return;
        }

        direction = (target.transform.position - transform.position).normalized * 5;
        foreach (var t in avoid)
            if (t)
                direction += (transform.position - t.transform.position).normalized * (4 / (t.transform.position - transform.position).magnitude);

        if (rigidbody.velocity.magnitude > 3)
        {
            direction -= rigidbody.velocity*2;
        }

        rigidbody.AddForce(direction);

        transform.up = rigidbody.velocity.normalized;
	}

    void OnCollisionEnter(Collision collision)
    {
        if (Network.isServer && collision.collider.GetComponent<SpacePlayer>() && collision.collider.GetComponent<SpacePlayer>() == target)
        {
            collision.collider.GetComponent<SpacePlayer>().Impact();
            collision.collider.GetComponent<SpacePlayer>().networkView.RPC("Impact", RPCMode.Others);

            if (boom)
                Network.Instantiate(boom, transform.position, transform.rotation, 0);
            Network.Destroy(networkView.viewID);
        }
    }
}
