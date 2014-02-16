using UnityEngine;
using System.Collections;

public class GameController {

    private static GameController instance = null;
    public static GameController Instance
    {
        get
        {
            if (instance == null)
                instance = new GameController();
            return instance;
        }
    }


    public bool isServer;
    public string name;

    private GameController()
    {

    }
}
