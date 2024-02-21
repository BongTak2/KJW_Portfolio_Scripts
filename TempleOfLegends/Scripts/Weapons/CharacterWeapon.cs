using UnityEngine;

public class CharacterWeapon : Weapon
{
    protected new Character owner;
    protected PrefabType weaponPrefab;
    public PrefabType WeaponPrefab
    {
        get => weaponPrefab;
        protected set
        {
            weaponPrefab = value;
        }
    }
    protected int bulletAmount;
    public int BulletAmount => bulletAmount;
    protected const int firstBulletAmount = 30;

    public WeaponType WeaponType { get; protected set; }

    public CharacterWeapon(Character _owner, PrefabType _bulletType) : base(_owner, _bulletType)
    {
        owner = _owner;
        bulletType = _bulletType;
        bulletAmount = firstBulletAmount;

    }

    public override void NormalAttack(Unit target, bool subWeapon = false)
    {
        if (bulletAmount > 0)
        {
            //Debug.Log(bulletAmount);
            GameObject obj = PoolManager.Instantiate(bulletType);

            if (!muzzle || subWeapon)
            {
                obj.transform.position = owner.transform.position;
            }
            else
            {
                obj.transform.position = muzzle.transform.position;
            }

            if (obj && target)
            {
                float bulletSpeed = 10f;
                Bullet bullet = obj.GetComponent<Bullet>();
                bullet.SetBullet(owner, target, bulletSpeed);
                if (!subWeapon)
                {
                    bulletAmount--;
                }
            }
        }
    }

    public virtual void SeverumAttack(Unit target)
    {
        NormalAttack(target, true);
    }

    /// <summary>
    /// Calibrum 표식 - 보조무기로 공격
    /// </summary>
    /// <param name="target">대상</param>
    /// <param name="muzzle">총구 위치</param>
    public virtual void CalibrumAttack(Unit target, Vector3 muzzle)
    {
        GameObject obj = PoolManager.Instantiate(bulletType);

        obj.transform.position = new Vector3(muzzle.x, muzzle.y - 0.01f, muzzle.z);

        if (obj && target)
        {
            float bulletSpeed = 10f;
            Bullet bullet = obj.GetComponent<Bullet>();
            bullet.SetBullet(owner, target, bulletSpeed);
        }
    }

    public void ReLoading(bool change = true)
    {
        if (change)
            bulletAmount = firstBulletAmount;
        else
        {
            bulletAmount = -1;
        }
    }

    public virtual void SkillShot(RaycastHit cursor, CharacterWeapon subWeapon) { }

    public virtual void UltimateShot(RaycastHit cursor, CharacterWeapon subWeapon)
    {
        owner.SetSight(cursor.point);

        Vector3 skillDir = cursor.point.Y_VectorToZero() - owner.transform.position.Y_VectorToZero();
        TimeManager.instance.ultCoolReady = true;
        GameObject obj = PoolManager.Instantiate(PrefabType.Prefabs__Skill__Ultimate);

        obj.transform.position = owner.transform.position;
        if (obj.TryGetComponent(out Ultimate ultimate))
        {

            ultimate.SetUlt(owner, skillDir.normalized, 10f, WeaponCheck(owner.CurrentWeapon));
        }
    }

    public WeaponType WeaponCheck(CharacterWeapon weapon)
    {
        if (weapon.TryCast(out Calibrum a))
        {
            return WeaponType.Calibrum;
        }
        else if (weapon.TryCast(out Severum b))
        {
            return WeaponType.Severum;
        }
        else if (weapon.TryCast(out Gravitum c))
        {
            return WeaponType.Gravitum;
        }
        else
        {
            return WeaponType.Infernum;
        }
    }

    public int consumeBullet(int amount)
    {
        bulletAmount -= Mathf.Min(bulletAmount, amount);
        return Mathf.Min(bulletAmount, amount);
    }
}
