using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command
{
    public Unit owner;
    
    public Command(Unit _owner)
    {
        owner = _owner;
    }

    public abstract void Start();

    public abstract void Stay();

    public abstract void End();  
}
