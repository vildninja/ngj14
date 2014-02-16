using UnityEngine;
using System.Collections;

public class GoogleStuff : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Social.localUser.Authenticate(success =>
            {
                if (success)
                {
                    GameController.Instance.name = Social.localUser.userName;
                }
            });
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
