using UnityEngine;


public class Bullet : MonoBehaviour
{
    [SerializeField]
    protected Unit target;
    protected Unit owner;

    protected ParticleSystem[] particles;

    protected float bulletVelocity;

    protected virtual void OnEnable() { }

    protected void Update()
    {
        if (target != null && !target.gameObject.activeInHierarchy)
        {
            PoolManager.Destroy(gameObject);
        }        
    }

    protected void FixedUpdate()
    {
        if (target != null && target.gameObject.activeInHierarchy)
        {
            Vector3 targetDis = target.transform.position - transform.position;

            if (targetDis.magnitude < bulletVelocity * Time.fixedDeltaTime)
            {
                HitTarget(target, targetDis.normalized);
            }
            else
            {               
                transform.position += targetDis.normalized * bulletVelocity * Time.fixedDeltaTime;
            }
        }
        else
        {
            PoolManager.Destroy(gameObject);
        }
    }
    public void SetBullet(Unit _owner, Unit _target, float bulletSpeed)
    {
        target = _target;
        bulletVelocity = bulletSpeed;
        owner = _owner;
        if (!target)
        {
            PoolManager.Destroy(gameObject);
        }
    }

    protected virtual void HitTarget(Unit _target, Vector3 _targetDir, float multiplier = 1f)
    {
        target.TakeDamage(owner, owner.DamagePower(), multiplier);

        PoolManager.Destroy(gameObject);
    }
}
