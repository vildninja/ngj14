using UnityEngine;
using System.Collections;

public class BoomPlayer : MonoBehaviour {

	public float screenShakeAmount;
	public float screenShakeTime;

	// Use this for initialization
	void Start () {
		Destroy (transform,5);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
