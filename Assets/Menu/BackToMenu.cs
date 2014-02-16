using UnityEngine;
using System.Collections;

public class BackToMenu : UIButton {

    protected override void Up()
    {
        if (Network.isClient || Network.isServer)
            Network.Disconnect();
        Application.LoadLevel("Menu");
    }
	
	// Update is called once per frame
	void Update () 
    {
        base.Update();
        bool show = !FindObjectOfType<GameServer>().startGame;
        renderer.enabled = show;
        collider.enabled = show;
	}
}
