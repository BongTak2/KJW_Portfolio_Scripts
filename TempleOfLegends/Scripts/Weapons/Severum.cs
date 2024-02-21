using UnityEngine;

public class Severum : CharacterWeapon
{
    public Severum(Character _owner, PrefabType _bulletType = PrefabType.Prefabs__Bullet__SeverumBullet) : base(_owner, _bulletType)
    {
        owner = _owner;
        bulletType = _bulletType;
        weaponPrefab = PrefabType.Prefabs__Weapon__Severum;
        WeaponType = WeaponType.Severum;
    }

    //public override void NormalAttack(Unit target, bool subWeapon = false)
    //{
    //    if (bulletAmount > 0 && target)
    //    {
    //        owner.TakeHeal(owner.DamagePower(), 0.12f);
    //        target.TakeDamage(owner, owner.DamagePower());
    //        if (!subWeapon)
    //        {
    //            bulletAmount--;
    //        }
    //    }
    //}

    public override void CalibrumAttack(Unit target, Vector3 muzzle)
    {
        NormalAttack(target, true);
    }

    float delay = 0f;
    bool sub = true;

    public void SkillShot(Unit target, CharacterWeapon subWeapon)
    {
        if (target != null)
        {
            if (delay > 0.25f)
            {
                if (!sub)
                {
                    NormalAttack(target, true);
                }
                else
                {
                    subWeapon.SeverumAttack(target);
                }
                sub = !sub;
                delay = 0f;
            }
            else
            {
                delay += Time.deltaTime;
            }
        }
    }

}
