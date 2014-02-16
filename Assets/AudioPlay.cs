using UnityEngine;
using System.Collections;

public class AudioPlay : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (Network.player == networkView.owner) {
				audio.Play ();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
