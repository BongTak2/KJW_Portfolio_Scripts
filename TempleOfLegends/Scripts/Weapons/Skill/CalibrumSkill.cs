using UnityEngine;

public class CalibrumSkill : MonoBehaviour
{
    private Character owner;
    private Vector3 targetDir;
    private float velocity;
    private Vector3 startPos;
    private Vector3 endPos;

    void Start()
    {
        velocity = 12f;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, endPos, Time.deltaTime * velocity);
        if (transform.position == endPos)
        {
            PoolManager.Destroy(gameObject);
        }
    }

    public void SetSkillShot(Character _owner, Vector3 _targetDir, float _range)
    {
        startPos = transform.position;
        owner = _owner;
        targetDir = _targetDir.normalized;
        endPos = startPos + (targetDir * _range);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryCast(out CapsuleCollider col))
        {
            if (col.gameObject.TryGetComponent(out Character target))
            {
                TargetAttack(target);
            }
            else if (col.gameObject.TryGetComponent(out Minion minion))
            {
                TargetAttack(minion);
            }
        }
    }

    public void TargetAttack(Unit target, float multiplier = 0.4f)
    {
        if (target.CheckEnemy(owner))
        {
            PoolManager.Destroy(gameObject);
             
            target.TakeDamage(owner, owner.DamagePower(), multiplier);

            target.SetCalibrumMarkTrigger(true);
        }
    }
}
