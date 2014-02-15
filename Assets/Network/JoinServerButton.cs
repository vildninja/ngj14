using UnityEngine;
using System.Collections;

public class JoinServerButton : UIButton
{

    public HostData data;

    protected override void Up()
    {
        Network.Connect(data);
    }
}
