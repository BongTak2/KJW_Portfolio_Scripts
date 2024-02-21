using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Infernum : CharacterWeapon
{
    protected float angle;

    public Infernum(Character _owner, PrefabType _bulletType = PrefabType.Prefabs__Bullet__InfernumBullet) : base(_owner, _bulletType)
    {
        owner = _owner;
        bulletType = _bulletType;
        weaponPrefab = PrefabType.Prefabs__Weapon__Infernum;
        WeaponType = WeaponType.Infernum;
        angle = 60f;
    }
   
    public override void SkillShot(RaycastHit cursor, CharacterWeapon subWeapon)
    {
        owner.TakeMana(50);
        bulletAmount -= Mathf.Min(bulletAmount, 10);
        Vector3 skillDir = (cursor.point - owner.transform.position).normalized;
        float radius = 10f;

        Collider[] cols = Physics.OverlapSphere(owner.transform.position, radius);
        List<CapsuleCollider> colliders = new List<CapsuleCollider>();

        foreach (Collider col in cols)
        {
            if (col.TryCast(out CapsuleCollider _col))
            {
                if (!colliders.Contains(_col))
                {
                    colliders.Add(_col);
                }
            }
        }

        colliders.RemoveAll(current => Vector3.Angle((current.gameObject.transform.position.Y_VectorToZero() - owner.transform.position.Y_VectorToZero()).normalized, skillDir) > angle);
        colliders.RemoveAll(current => Vector3.Distance(current.gameObject.transform.position.Y_VectorToZero(), owner.transform.position.Y_VectorToZero()) > radius);

        foreach (CapsuleCollider enemy in colliders)
        {
            if (enemy.TryGetComponent(out Minion target1))
            {
                if (target1.CheckEnemy(owner))
                {
                    target1.TakeDamage(owner, owner.DamagePower(), 1.1f);
                    GameObject effect = PoolManager.Instantiate(PrefabType.Prefabs__Skill__InfernumAddBullet);
                    effect.transform.position = target1.transform.position;
                    subWeapon.NormalAttack(target1, true);
                }
            }

            if (enemy.TryGetComponent(out Character target2))
            {
                if (target2.CheckEnemy(owner))
                {
                    target2.TakeDamage(owner, owner.DamagePower(), 1.1f);
                    GameObject effect = PoolManager.Instantiate(PrefabType.Prefabs__Skill__InfernumAddBullet);
                    effect.transform.position = target2.transform.position;
                    subWeapon.NormalAttack(target2, true);
                }
            }
        }
        //Debug.DrawLine(owner.transform.position.Y_VectorToZero(), (owner.transform.position + skillDir.RotateHorizontal(-angle) * radius).Y_VectorToZero(), Color.red, 2);
        //Debug.DrawLine(owner.transform.position.Y_VectorToZero(), (owner.transform.position + skillDir.RotateHorizontal(angle) * radius).Y_VectorToZero(), Color.red, 2);
        //Debug.DrawLine((owner.transform.position + skillDir.RotateHorizontal(-angle) * radius).Y_VectorToZero(), (owner.transform.position + skillDir.RotateHorizontal(angle) * radius).Y_VectorToZero(), Color.red, 2);
    }
}
