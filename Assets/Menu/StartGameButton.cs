using UnityEngine;
using System.Collections;

public class StartGameButton : UIButton {

    protected override void Up()
    {
        FindObjectOfType<GameServer>().startGame = true;
        Destroy(gameObject);
    }
}
