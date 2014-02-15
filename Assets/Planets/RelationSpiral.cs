using UnityEngine;
using System.Collections;

public class RelationSpiral : MonoBehaviour {

    float zZero = 10;
    float zFull = 6.682863f;
    public float zOne;
    public float value;

	// Use this for initialization
	void Update () {
        var pos = transform.position;
        pos.z = Mathf.Lerp(zZero, zFull, Mathf.Clamp01(value) * zOne);
        transform.position = pos;
	}
}
