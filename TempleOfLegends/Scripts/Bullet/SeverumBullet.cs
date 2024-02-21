using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeverumBullet : Bullet
{
    protected override void HitTarget(Unit _target, Vector3 _targetDir, float multiplier = 1f)
    {
        base.HitTarget(_target, _targetDir);
        owner.TakeHeal(owner.DamagePower(), 0.12f);
    }
}
