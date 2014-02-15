using UnityEngine;
using System.Collections;

public class LoadGameScene : UIButton {

    public bool asServer;

    protected override void Up()
    {
        GameController.Instance.isServer = asServer;

        Application.LoadLevel("Server");
    }
}
