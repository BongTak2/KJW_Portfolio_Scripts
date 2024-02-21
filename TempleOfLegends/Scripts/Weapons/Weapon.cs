using UnityEngine;

public enum WeaponType
{
    Calibrum,
    Severum,
    Gravitum,
    Infernum
}
public class Weapon
{
    protected Unit owner;

    protected PrefabType bulletType;

    protected GameObject muzzle;

    public Weapon(Unit _owner, PrefabType _bulletType)
    {
        owner = _owner;
        bulletType = _bulletType;
    }

    public virtual void NormalAttack(Unit target, bool subWeapon = false)
    {
        GameObject obj = PoolManager.Instantiate(bulletType);
        if (!muzzle)
        {
            obj.transform.position = owner.transform.position;
        }
        else
        {
            obj.transform.position = muzzle.transform.position;
        }

        if (target && obj.TryGetComponent(out Bullet bullet))
        {
            float bulletSpeed = 10f;
            bullet.SetBullet(owner, target, bulletSpeed);
        }
    }

    public void SetMuzzle(Socket _muzzle)
    {
        muzzle = _muzzle.gameObject;
    }
    public Vector3 GetMuzzle() => muzzle.transform.position;
}
