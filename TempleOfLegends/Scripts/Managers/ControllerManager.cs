using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType
{
    Character,
    Minion,
    Turret
}
public class ControllerManager : Manager
{
    public ControllerManager instance;
   
    public override void Initialize()
    {
        base.Initialize();
        this.Singleton(ref instance);
    }
}
