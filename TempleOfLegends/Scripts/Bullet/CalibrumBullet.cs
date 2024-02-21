using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrumBullet : Bullet
{
    protected bool skillAttack;

    public void SetBullet(Unit _owner, Unit _target, float bulletSpeed, bool _skillAtk)
    {
        target = _target;
        bulletVelocity = bulletSpeed;
        owner = _owner;
        skillAttack = _skillAtk;

        if (!target)
        {
            PoolManager.Destroy(gameObject);
        }
    }

    protected override void HitTarget(Unit _target, Vector3 _targetDir, float multiplier = 1f)
    {
        base.HitTarget(_target, _targetDir);
        if (skillAttack)
        {
            _target.SetCalibrumMarkTrigger(true);
        }
    }
}
