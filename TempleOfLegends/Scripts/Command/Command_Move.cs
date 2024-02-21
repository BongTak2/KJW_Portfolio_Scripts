using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Command_Move : Command
{
    public Vector3 targetPos;

    public Command_Move(Unit _owner, Vector3 _targetPos) : base(_owner)
    {
        targetPos = _targetPos;
    }

    public override void Start()
    {
        owner.SetMove(targetPos);        
    }

    public override void Stay()
    {

    }

    public override void End()
    {
        owner.SetStop();
    }
}
