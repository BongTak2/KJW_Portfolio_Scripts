using UnityEngine;

public class Calibrum : CharacterWeapon
{
    protected float range;
    public Calibrum(Character _owner, PrefabType _bulletType = PrefabType.Prefabs__Bullet__CalibrumBullet) : base(_owner, _bulletType)
    {
        owner = _owner;
        bulletType = _bulletType;
        weaponPrefab = PrefabType.Prefabs__Weapon__Calibrum;
        WeaponType = WeaponType.Calibrum;
        range = 12f;
    }

    public override void SkillShot(RaycastHit cursor, CharacterWeapon subWeapon)
    {
        owner.TakeMana(50);
        bulletAmount -= Mathf.Min(bulletAmount, 10);
        owner.SetSight(cursor.point);

        Vector3 skillDir = cursor.point - owner.transform.position;

        GameObject obj = PoolManager.Instantiate(PrefabType.Prefabs__Skill__CalibrumSkill);

        if (!muzzle) return;

        obj.transform.position = muzzle.transform.position;

        if (obj.TryGetComponent(out CalibrumSkill skill))
        {
            skill.SetSkillShot(owner, skillDir.Y_VectorToZero(), range);
        }
    }

    public override void SeverumAttack(Unit target)
    {
        GameObject obj = PoolManager.Instantiate(bulletType);
        obj.transform.position = owner.transform.position;

        float bulletSpeed = 10f;
        CalibrumBullet bullet = obj.GetComponent<CalibrumBullet>();
        bullet.SetBullet(owner, target, bulletSpeed, true);
    }

}
