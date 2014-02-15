using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rocket : MonoBehaviour {

    public SpacePlayer target;


    public List<Transform> avoid;
    Vector3 direction = Vector3.zero;

	// Use this for initialization
	void Start () {
        avoid = new List<Transform>();
        foreach (var sp in FindObjectsOfType<SpacePlayer>())
            if (sp != target)
                avoid.Add(sp.transform);
        foreach (var p in FindObjectsOfType<PlanetController>())
            avoid.Add(p.transform);
	}

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + direction);
    }

	// Update is called once per frame
	void FixedUpdate () {
        direction = (target.transform.position - transform.position).normalized * 4;
        foreach (var t in avoid)
            direction += (transform.position - t.transform.position).normalized * (5 / (t.transform.position - transform.position).magnitude);

        if (rigidbody.velocity.magnitude > 3)
        {
            direction -= rigidbody.velocity;
        }

        rigidbody.AddForce(direction);

        transform.up = rigidbody.velocity.normalized;
	}
}
