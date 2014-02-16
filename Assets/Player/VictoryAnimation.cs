using UnityEngine;
using System.Collections;

public class VictoryAnimation : MonoBehaviour {

    Transform model;
    public float speed;

	// Use this for initialization
	void Awake ()
    {
        model = transform.Find("Model");
        if (FindObjectOfType<SpacePlayer>())
        {
            var tex = FindObjectOfType<SpacePlayer>().GetComponentInChildren<Renderer>().material.mainTexture;
            model.GetComponentInChildren<Renderer>().material.mainTexture = tex;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        model.Rotate(0, speed * Time.deltaTime, 0, Space.World);
	}
}
