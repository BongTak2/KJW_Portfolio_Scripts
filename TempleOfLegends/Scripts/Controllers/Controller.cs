using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    protected Character controlledCharacter;
    protected Minion controlledMinion;
    protected Turret controlledTurret;

    protected Vector3 targetPoint;
    protected Vector3 sightDir;
    protected Vector3 sightPos;
    protected Vector3 moveDir;

    protected virtual void OnEnable()
    {
        
    }
    //public virtual void ControlMove(Vector3 target)
    //{
    //    if (controlledCharacter)
    //    {
    //        controlledCharacter.SetMove(target);
    //    }
    //    //else if (controlledMinion)
    //    //{
    //    //    return controlledMinion.SetMove(wantDir);
    //    //}
    //}

    //public virtual void ControlAttackMove(Vector3 target)
    //{
    //    if (controlledCharacter)
    //    {
    //        controlledCharacter.SetAttackMove(target);
    //    }
    //}


    public virtual void ControlStop()
    {
        if (controlledCharacter)
        {
            controlledCharacter.SetStop();
        }
    }

    public virtual Vector3 ControlSight(Vector3 wantDir)
    {
        if (controlledCharacter)
        {
            return controlledCharacter.SetSight(wantDir);
        }
        else
        {
            return Vector3.zero;
        }

    }

    public virtual bool GetAtkRange(bool active)
    {
        if (controlledCharacter)
        {
            return controlledCharacter.RangeMark(active);
        }
        //else if (controlledTurret)
        //{
        //    return controlledTurret.RangeMark(active);
        //}
        else
            return false;
    }

}
