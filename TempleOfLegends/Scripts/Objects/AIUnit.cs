using System.Collections.Generic;
using UnityEngine;

public class AIUnit : Unit
{
    protected List<Unit> targetList = new List<Unit>();      // ���� ������ ���� ���ֵ� ����Ʈ

    [SerializeField]
    protected Unit target;     // Ÿ�����ϴ� ����
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
    /// Ÿ�� �켱���� (���� ���� �ִ� �ְ� ������ ��-��-è (�Ÿ���))
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
            // ��ž ��Ÿ� ǥ�� 
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
            //��ž ��Ÿ� ǥ��
            //RangeMark(target.CurrentType != ObjectType.Character);
        }
    }

    /// <summary>
    /// ������, ������, ���� ���ط�
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
