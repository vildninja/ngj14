using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

	public Transform player;

	// Use this for initialization
	void Start () {
		transform.LookAt (player);
	}
}
