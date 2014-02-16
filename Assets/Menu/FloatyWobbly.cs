using UnityEngine;
using System.Collections;

public class FloatyWobbly : MonoBehaviour {

    public AnimationCurve floaty;
    public AnimationCurve wobbly;
    public AnimationCurve scaly;

    Vector3 position;
    Vector3 scale;
    Vector3 rotation;

	// Use this for initialization
	void Start () {
        position = transform.position;
        scale = transform.localScale;
        rotation = transform.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = position + Vector3.up * floaty.Evaluate(Time.time);
        transform.eulerAngles = rotation + Vector3.left * wobbly.Evaluate(Time.time);
        transform.localScale = scale * scaly.Evaluate(Time.time);
	}
}
