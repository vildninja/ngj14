using UnityEngine;
using System.Collections;

public class LoveShotInMotion : MonoBehaviour {

	public int sender;	
	public float hateFactor;
	public float selfDestructTimer = 20;

	IEnumerator Start(){
		yield return new WaitForSeconds (selfDestructTimer);
		Network.Destroy (gameObject);
	}	
}
