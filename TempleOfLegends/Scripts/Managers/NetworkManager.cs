using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : Manager
{
    public NetworkManager instance;

    public override void Initialize()
    {
        base.Initialize();
        this.Singleton(ref instance);
    }
}
