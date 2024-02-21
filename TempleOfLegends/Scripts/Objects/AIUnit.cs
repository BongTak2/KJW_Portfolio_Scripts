using System.Collections.Generic;
using UnityEngine;

public class AIUnit : Unit
{
    protected List<Unit> targetList = new List<Unit>();      // 인지 범위에 들어온 유닛들 리스트

    [SerializeField]
    protected Unit target;     // 타겟팅하는 유닛
    protected Socket muzzle;

    protected override void Initialize()
    {
        base.Initialize();
        currentWeapon = new Weapon(this, bulletType);
        muzzle = GetComponentInChildren<Socket>();
        if (muzzle != null)
        {
            currentWeapon.SetMuzzle(muzzle);
        }
    }

    protected override void Update()
    {
        base.Update();

        if (target != null)
        {
            if (target.CalculateState() == State.Die || target.gameObject.activeInHierarchy == false)
            {
                ChangeTarget();
            }
        }
    }

    public void ChangeTarget()
    {
        targetList.Remove(target);
        target = TargetPriority();

        if (targetList.Count == 0)
        {
            SetCommand(null);
            if (animator != null)
            {
                AttackAnimationEnd();
            }
            target = null;
        }
    }

    public Unit GetTarget()
    {
        if (targetList.Count == 0)
        {
            target = null;
        }
        return target;
    }

    /// <summary>
    /// 타겟 우선순위 (지금 보고 있던 애가 죽으면 근-원-챔 (거리순))
    /// </summary>
    public Unit TargetPriority()
    {
        Unit result = null;
        float score = -100f;

        foreach (Unit currentUnit in targetList)
        {
            if (currentUnit.CheckEnemy(this))
            {
                float currentScore = 0;
                currentScore += (float)currentUnit.CurrentType * 100f;
                currentScore -= Vector3.Distance(transform.position, currentUnit.transform.position);

                if (currentScore > score)
                {
                    result = currentUnit;
                    score = currentScore;
                }
            }
        }

        return result;
    }

    protected void AttackAnimationEnd()
    {
        animator.Rebind();
        animator.Update(0);
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (!other.Cast<SphereCollider>() && other.TryGetComponent(out Unit inTarget))
        {
            if (inTarget.CheckEnemy(this))
            {
                if (!targetList.Contains(inTarget))
                {
                    if (targetList.Count == 0)
                    {
                        target = inTarget;
                    }
                    inTarget.OnDamage -= OnCharacterAttacked;
                    inTarget.OnDamage += OnCharacterAttacked;
                    targetList.Add(inTarget);
                }
            }
            // 포탑 사거리 표시 
            //RangeMark(target.CurrentType == ObjectType.Character);
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Unit outTarget))
        {
            targetList.Remove(outTarget);

            if (outTarget == target)
            {
                ChangeTarget();
            }

            outTarget.OnDamage -= OnCharacterAttacked;
            //포탑 사거리 표시
            //RangeMark(target.CurrentType != ObjectType.Character);
        }
    }

    /// <summary>
    /// 맞은애, 때린애, 입힌 피해량
    /// </summary>
    public void OnCharacterAttacked(Unit to, Unit from, float damage)
    {
        if ((from.CurrentType == ObjectType.Character) && (to.CurrentType == ObjectType.Character) && to.currentRegion == currentRegion && from.currentRegion != currentRegion)
        {
            if (targetList.Contains(from))
            {
                target = from;
            }
        }
    }

    public bool CheckTarget()
    {
        if (targetList.Count == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public override void SetDie()
    {
        base.SetDie();
        target = null;
    }
}
