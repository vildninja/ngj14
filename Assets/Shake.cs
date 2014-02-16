using UnityEngine;
using System.Collections;

public class Shake : MonoBehaviour {
	public float amount;
	public float timer = 0;

	private bool switcher = true;

	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if (timer > 0) {
			if (switcher) {
				float newX = Random.Range (-amount, amount);
				float newY = Random.Range (-amount, amount);
				transform.Translate(new Vector3(newX,newY,-10));
				switcher = false;
				return;
				} 
			else {
				transform.position = new Vector3(0,0,-10);
				switcher = true;
				return;
			}
		}
		else {
			transform.position = new Vector3(0,0,-10);
			switcher = true;
		}
	}

	public void SetShake(float a, float t){
		amount = a;
		timer = t;
	}
}
