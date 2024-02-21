using System.Collections.Generic;
using UnityEngine;

public class Gravitum : CharacterWeapon
{
    public static List<Unit> gravitumMarkUnits = new List<Unit>();

    public Gravitum(Character _owner, PrefabType _bulletType = PrefabType.Prefabs__Bullet__GravitumBullet) : base(_owner, _bulletType)
    {
        owner = _owner;
        bulletType = _bulletType;
        weaponPrefab = PrefabType.Prefabs__Weapon__Gravitum;
        WeaponType = WeaponType.Gravitum;
    }

    public override void SkillShot(RaycastHit cursor, CharacterWeapon subWeapon)
    {
        if (gravitumMarkUnits.Count > 0)
        {
            owner.TakeMana(50);
            bulletAmount -= Mathf.Min(bulletAmount, 10);
            for (int i = 0; i < gravitumMarkUnits.Count; i++)
            {
                gravitumMarkUnits[i].SetStun(true);
            }
        }
    }
}
