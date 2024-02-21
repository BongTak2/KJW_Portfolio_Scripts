using System.Collections.Generic;
using UnityEngine;

public class InfernumBullet : Bullet
{
    private float radius;
    private float angle;

    protected override void OnEnable()
    {
        //radius = 2.4f;
        radius = 4f;
        angle = 60f;
    }

    protected override void HitTarget(Unit _target, Vector3 _targetDir, float multiplier = 1f)
    {
        Collider[] cols = Physics.OverlapSphere(_target.transform.position, radius);
        List<CapsuleCollider> colliders = new List<CapsuleCollider>();

        foreach (Collider col in cols)
        {
            if (col.TryCast(out CapsuleCollider _col))
            {
                if (!colliders.Contains(_col) && _col.gameObject != _target.gameObject)
                {
                    colliders.Add(_col);
                }
            }
        }

        colliders.RemoveAll(current => Vector3.Angle(current.gameObject.transform.position.Y_VectorToZero() - _target.transform.position.Y_VectorToZero(), _targetDir.Y_VectorToZero()) > angle);
        colliders.RemoveAll(current => current.gameObject.transform.position.DistanceXZ(_target.transform.position) > radius);

        base.HitTarget(_target, _targetDir, 1.1f);

        foreach (CapsuleCollider enemy in colliders)
        {
            if (enemy.TryGetComponent(out Minion target1))
            {
                if (!target1.CheckEnemy(_target))
                {
                    target1.TakeDamage(owner, owner.DamagePower(), 0.5f);
                    GameObject effect = PoolManager.Instantiate(PrefabType.Prefabs__Skill__InfernumAddBullet);
                    effect.transform.position = target1.transform.position;
                }
            }

            if (enemy.TryGetComponent(out Character target2))
            {
                if (!target2.CheckEnemy(_target))
                {
                    target2.TakeDamage(owner, owner.DamagePower(), 0.5f);
                    GameObject effect = PoolManager.Instantiate(PrefabType.Prefabs__Skill__InfernumAddBullet);
                    effect.transform.position = target2.transform.position;
                }
            }
        }
        Debug.DrawLine(_target.transform.position.Y_VectorToZero(), (_target.transform.position + _targetDir.RotateHorizontal(-angle) * radius).Y_VectorToZero(), Color.green, 5);
        Debug.DrawLine(_target.transform.position.Y_VectorToZero(), (_target.transform.position + _targetDir.RotateHorizontal(angle) * radius).Y_VectorToZero(), Color.green, 5);
        Debug.DrawLine((_target.transform.position + _targetDir.RotateHorizontal(-angle) * radius).Y_VectorToZero(), (_target.transform.position + _targetDir.RotateHorizontal(angle) * radius).Y_VectorToZero(), Color.green, 5);
    }
}
