using UnityEngine;

public enum MinionType
{
    Melee,
    Caster
}

public class Minion : AIUnit
{
    [SerializeField]
    protected MinionType currentType;
    protected float expPoint;
    public float Exp => expPoint;

    protected override void Initialize()
    {
        base.Initialize();
        levelPoint = TimeManager.instance.minionLevel;
        animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();
        if (target != null)
        {
            ChangeTargetFromCharacter();
        }

        //UIManager.instance.ChangeUnitHP(health.Current, health.Max);
    }

    protected override void SetBase()
    {
        switch (currentType)
        {
            case MinionType.Melee:
                {
                    CurrentType = ObjectType.MeleeMinion;
                    firstHealth = 250f;
                    healthRegenRate = 0f;

                    firstAtk = 10f;
                    atkStat.AtkSpeed = 0.6f;
                    atkStat.AtkRange = 1.8f;

                    defPower = 0f;
                    moveSpeed = 4f;
                    //slowSpeed = 2f;

                    expPoint = 60f;

                    bulletType = PrefabType.Prefabs__Bullet__MeleeBullet;
                    break;
                }
            case MinionType.Caster:
                {
                    CurrentType = ObjectType.CasterMinion;
                    firstHealth = 150f;
                    healthRegenRate = 0f;

                    firstAtk = 20f;
                    atkStat.AtkSpeed = 0.5f;
                    atkStat.AtkRange = 6f;

                    defPower = 0f;
                    moveSpeed = 4f;
                    //slowSpeed = 2f;

                    expPoint = 30f;

                    bulletType = PrefabType.Prefabs__Bullet__CasterBullet;
                    break;
                }
        }
    }

    protected override void StatPerLevel(int _levelPoint)
    {
        switch (currentType)
        {
            case MinionType.Melee:
                {
                    health.Max = firstHealth + LevelUpStat(100f, _levelPoint);
                    health.Current = health.Max;
                    atkStat.AtkPower = firstAtk + LevelUpStat(8f, _levelPoint);
                    break;
                }
            case MinionType.Caster:
                {
                    health.Max = firstHealth + LevelUpStat(50f, _levelPoint);
                    health.Current = health.Max;
                    atkStat.AtkPower = firstAtk + LevelUpStat(12f, _levelPoint);
                    break;
                }
        }

    }

    protected void ChangeTargetFromCharacter()
    {
        if ((targetList.Count >= 2) && target.TryGetComponent(out Character charcterTarget))
        {
            if (Vector3.Distance(charcterTarget.transform.position, gameObject.transform.position) > Mathf.Max(2f, atkStat.AtkRange))
            {
                targetList.Remove(charcterTarget);
                target = TargetPriority();
            }
        }
    }

    public override void SetMove(Vector3 wantPos)
    {
        base.SetMove(wantPos);
        if (animator != null && currentType == MinionType.Melee)
        {
            animator.SetBool("Attack", false);
        }
    }
    public override void SetAttack(Unit target)
    {
        if (animator != null)
        {
            if (currentType == MinionType.Melee)
            {
                animator.SetBool("Attack", true);
                animator.SetInteger("Attack Num", Random.Range(1, 3));
            }
            else
            {
                animator.SetTrigger("Attack");
            }
        }

        if (AtkReady && target != null)
        {
            if (CurrentType == ObjectType.Character || CurrentType == ObjectType.CasterMinion || CurrentType == ObjectType.MeleeMinion)
            {
                SetSight(target.transform.position);
            }

            if (currentWeapon != null)
            {
                AtkReady = false;
            }

        }
    }

    public void NormalAttack()
    {
        currentWeapon.NormalAttack(target);
    }

    public override float TakeDamage(Unit instigator, float damage, float multiplier = 1)
    {
        float result = damage * multiplier * (float)(1 / (1 + (0.01 * defPower)));
        //Mathf.RoundToInt(damage * multiplier);
        result = Mathf.Min(result, health.Current);

        health.Current -= result;

        if (instigator.TryCast(out Character _instigator) && health.Current <= 0f)
        {
            _instigator.GetExp(expPoint);
        }
        OnDamage?.Invoke(this, instigator, result);

        return result;
    }
}
