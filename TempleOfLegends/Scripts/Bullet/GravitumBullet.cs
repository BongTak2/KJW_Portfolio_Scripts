using UnityEngine;

public class GravitumBullet : Bullet
{ 
    
    protected override void HitTarget(Unit _target, Vector3 _targetDir, float multiplier = 1f)
    {
        base.HitTarget(_target, _targetDir);
        _target.SetGravitumMarkTrigger(true);
    }
     
}
