using System.Collections.Generic;
using UnityEngine;

public class Command_Ultimate : Command_Skill
{
    protected new const float skillDelayTime = 0.32f;
    protected Ultimate ult;

    public Command_Ultimate(Character _owner, RaycastHit _cusurPos) : base(_owner, _cusurPos)
    {
    }

    public override void Start()
    {
        delayTime = skillDelayTime;
    }
    public override void Stay()
    {
        delayTime -= Time.deltaTime;

        if (delayTime < 0)
        {
            owner.SetUltimate(cusurPos);
            End();
        }
    }
    public override void End()
    {
        delayTime = skillDelayTime;
        owner.SetStop();
    }
}
