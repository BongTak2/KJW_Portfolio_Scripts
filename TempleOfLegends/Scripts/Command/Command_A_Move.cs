using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command_A_Move : Command_Move
{
    public Unit target;
    public Command_A_Move(Unit _owner, Vector3 _targetPos) : base(_owner, _targetPos)
    {

    }

    public override void Start()
    {
        target = owner.FindNearEnemyFromPoint(targetPos);

        //if (target && owner.CheckAttackRange(target.transform.position) && owner.currentRegion != target.currentRegion)
        //{
        //    owner.SetAttack(target);
        //}
    }

    public override void Stay()
    {
        if (target != null && owner.CheckEnemy(target))
        {
            if (target.calibrumMark && owner.TryCast(out Character character))
            {
                if (character.CheckAttackRange(target.transform.position, 1.8f))
                {
                    character.CalibrumMarkAttack(target);
                    owner.SetMoveStop();
                    End();
                }
                else
                {
                    owner.SetMove(target.transform.position);
                }
            }
            else if (owner.CheckAttackRange(target.transform.position))
            {
                owner.SetAttack(target);
                owner.SetMoveStop();
            }
            else
            {
                owner.SetMove(target.transform.position);
            }

            if (target.gameObject.activeSelf == false)
            {
                target = null;
                End();
            }
        }
        else
        {
            owner.SetMove(targetPos);
        }
    }

    public override void End()
    {
        base.End();
    }
}
