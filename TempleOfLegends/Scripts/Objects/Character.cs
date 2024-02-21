using System.Collections.Generic;
using UnityEngine;

public class Character : Unit
{
    protected FloatAmount mana;
    protected float manaRegenRate;

    protected float currentXp;
    protected int xpPerLevel = 300;

    protected float firstMana;

    protected int firstLevel = 1;

    private Calibrum calibrum;
    private Severum severum;
    private Gravitum gravitum;
    private Infernum infernum;

    private Queue<CharacterWeapon> weaponQueue = new Queue<CharacterWeapon>();

    public CharacterWeapon CurrentWeapon
    {
        get => (CharacterWeapon)currentWeapon;
        protected set
        {
            currentWeapon = value;
        }
    }

    private CharacterWeapon subWeapon;
    private Transform appearance;
    private WeaponAppearance weaponAppearance;

    public DelayTime weaponChange;
    private DelayTime enqueueWeapon;
    private DelayTime severumAttack;

    private Vector3 destination;

    private Unit severumTarget;

    private int atkPowerLevel = 0;
    public int AtkPowerLevel
    {
        get => atkPowerLevel;
        set => atkPowerLevel = Mathf.Clamp(value, 0, 5);
    }

    private int atkSpeedLevel = 0;
    public int AtkSpeedLevel
    {
        get => atkSpeedLevel;
        set => atkSpeedLevel = Mathf.Clamp(value, 0, 5);
    }
    private int defPowerLevel = 0;
    public int DefPowerLevel
    {
        get => defPowerLevel;
        set => defPowerLevel = Mathf.Clamp(value, 0, 5);
    }
    public bool statUpWindow;
    protected override void Initialize()
    {
        base.Initialize();
        if (gameObject.CompareTag("Player"))
        {
            appearance = transform.GetChild(0);
            SetAppearance(true);
        }
        weaponAppearance = GetComponent<WeaponAppearance>();
        animator = GetComponent<Animator>();

        calibrum ??= new Calibrum(this);
        severum ??= new Severum(this);
        gravitum ??= new Gravitum(this);
        infernum ??= new Infernum(this);

        currentWeapon ??= calibrum;
        subWeapon ??= severum;

        if (weaponQueue.Count == 0)
        {
            weaponQueue.Enqueue(gravitum);
            weaponQueue.Enqueue(infernum);
        }
    }


    protected override void Update()
    {
        base.Update();

        StatPerLevel(levelPoint);
        if (Input.GetKeyDown(KeyCode.F9))
        {
            LevelUpPoint();
        }
    }

    protected override void SetBase()
    {
        if (gameObject.CompareTag("Bot"))
        {
            levelPoint = 0;
            firstHealth = 10000f;
            health.Max = firstHealth;
            health.Current = firstHealth;
            healthRegenRate = 1000f;
            firstMana = 0f;
            mana.Max = firstMana;
            mana.Current = firstMana;
            manaRegenRate = 0f;

            firstAtk = 0f;
            atkStat.AtkPower = firstAtk;
            atkStat.AtkRange = 0f;
            firstSpeed = 0f;
            atkStat.AtkSpeed = firstSpeed;

            firstDef = 100f;
            defPower = firstDef;
            moveSpeed = 0f;

            return;
        }
        CurrentType = ObjectType.Character;

        levelPoint = firstLevel;
        firstHealth = 550f;
        health.Max = firstHealth;
        health.Current = firstHealth;
        healthRegenRate = 0.6f;
        firstMana = 300f;
        mana.Max = firstMana;
        mana.Current = firstMana;
        manaRegenRate = 1f;

        firstAtk = 50f;
        atkStat.AtkPower = firstAtk;
        atkStat.AtkRange = 6f;
        firstSpeed = 0.6f;
        atkStat.AtkSpeed = firstSpeed;

        firstDef = 30f;
        //firstDef = 1030f;
        defPower = firstDef;
        moveSpeed = 6f;
        //slowSpeed = 3f;
    }


    /// <summary>
    /// 레벨 당 스탯
    /// </summary>
    protected override void StatPerLevel(int _levelPoint)
    {
        float temp = health.Max - health.Current;
        health.Max = firstHealth + LevelUpStat(200f, _levelPoint);
        health.Current = health.Max - temp;

        temp = mana.Max - mana.Current;
        mana.Max = firstMana + LevelUpStat(80f, _levelPoint);
        mana.Current = mana.Max - temp;

        atkStat.AtkPower = firstAtk + LevelUpStat(8f, _levelPoint) + (AtkPowerLevel * 7.24f);
        atkStat.AtkSpeed = firstSpeed + LevelUpStat(0.08f, _levelPoint) + (AtkSpeedLevel * 0.0224f);
        defPower = firstDef + LevelUpStat(6f, _levelPoint) + (DefPowerLevel * 8.68f);
    }

    public void LevelUpPoint()
    {
        levelPoint++;
        statUpWindow = true;
    }

    protected override void SetDelayTime()
    {
        base.SetDelayTime();
        weaponChange.SetCoolTime("weaponChange", 0.3f);
        enqueueWeapon.SetCoolTime("enqueueWeapon", 1f);
        severumAttack.SetCoolTime("severumAttack", 1.8f);
    }

    public virtual void TakeMana(float _mana, float multiplier = 1)
    {
        float result = _mana * multiplier;
        //Mathf.RoundToInt(damage * multiplier);
        result = Mathf.Min(result, mana.Current);

        mana.Current -= result;
    }

    public override void SetCommand(Command _command)
    {
        if (currentCommand is not null)
        {
            if (currentCommand.TryCast(out Command_Skill skillCommand) || currentCommand.TryCast(out Command_Ultimate ultCommand))
            {
                return;
            }
            else
            {
                currentCommand.End();
            }
        }

        currentCommand = _command;

        if (currentCommand is not null)
        {
            currentCommand.Start();
        }

    }

    protected override void DelayEnd()
    {
        base.DelayEnd();

        if (CoolEnd(weaponChange))
        {
            WeaponChange();
            coolEnd.Remove(weaponChange.name);
        }

        if (CoolEnd(enqueueWeapon))
        {
            EnqueueWeapon();
            coolEnd.Remove(enqueueWeapon.name);
        }

        if (CoolEnd(severumAttack))
        {
            severumTarget = null;
            isFast = false;
            coolEnd.Remove(severumAttack.name);
            TakeMana(50);
            CurrentWeapon.consumeBullet(10);
        }
        else if (coolEnd.ContainsKey(severumAttack.name))
        {
            if (CurrentWeapon.TryCast(out Severum severumWeapon))
            {
                severumTarget = SeverumTarget(FindNearUnits());
                severumWeapon.SkillShot(severumTarget, subWeapon);
            }
        }
    }

    protected override void UnitUpdate()
    {
        base.UnitUpdate();

        if (health.Current < health.Max && health.Current > 0f)
        {
            health.Current += RecoveryValue(healthRegenRate, Time.deltaTime);
        }
        if (mana.Current < mana.Max)
        {
            mana.Current += RecoveryValue(manaRegenRate, Time.deltaTime);
        }

        if (levelPoint < 15)
        {
            if (currentXp >= xpPerLevel)
            {
                if (levelPoint == 14)
                {
                    currentXp = xpPerLevel;
                }
                else
                {
                    currentXp = 0;
                    xpPerLevel += 100;
                }

                if (levelPoint == 4)
                {
                    TimeManager.instance.UltActive();
                }
                levelPoint++;
                statUpWindow = true;
            }
            else
            {
                currentXp += 8f * Time.deltaTime;
            }
        }
        else
        {
            levelPoint = 15;
            currentXp = xpPerLevel;
        }

        if (CheckCurrentWeapon(WeaponType.Calibrum))
        {
            atkStat.AtkRange = 7.5f;
        }
        else
        {
            atkStat.AtkRange = 6f;
        }
        if (CurrentWeapon.BulletAmount == 0)
        {
            //delayTimeFunc -= EnqueueWeapon;
            //delayTimeFunc += EnqueueWeapon;
            weaponQueue.Enqueue(CurrentWeapon);
            CurrentWeapon.ReLoading(false);
            StartCoolTime(ref enqueueWeapon);
        }

        if (agent.hasPath)
        {
            if (destination.DistanceXZ(transform.position) < 0.3f)
            {
                if (animator != null)
                {
                    animator.SetBool("Move", false);
                }
            }
        }
        UIInfomation();
    }

    /// <summary>
    /// 힐팩 먹었을때 실행되는 함수
    /// </summary>
    public void HealPack(float multiplier = 0.1f)
    {
        health.Current += RecoveryValue(health.Max - health.Current, multiplier);
        mana.Current += RecoveryValue(mana.Max - mana.Current, multiplier);
    }

    public override void SetMove(Vector3 wantPos)
    {
        base.SetMove(wantPos);
        if (animator != null)
        {
            animator.SetBool("Move", true);
        }
        destination = wantPos;
    }

    public override void SetDie()
    {
        if (CurrentType == ObjectType.Character)
        {
            animator.SetTrigger("Die");
        }

        firstLevel = levelPoint;
    }

    public override void SetAttackMove(Vector3 targetPos)
    {
        if (!CheckAttackRange(targetPos))
        {
            SetMove(targetPos);
        }
        else
        {
            if (animator != null)
            {
                animator.SetBool("Move", false);
            }
            agent.ResetPath();
        }
    }

    // 애니메이션 이벤트
    public void DieAppearance()
    {
        //SetAppearance(false);
        if (TimeManager.instance.DeathRespawn_Ready)
        {
            TimeManager.instance.deathRespawn_CoolReady = true;
        }
        PoolManager.Destroy(gameObject);
    }

    public void SetAppearance(bool value)
    {
        appearance.gameObject.SetActive(value);
        GetComponent<CapsuleCollider>().enabled = value;
    }

    public override void SetMoveStop()
    {
        if (agent.hasPath)
        {
            if (animator != null)
            {
                animator.SetBool("Move", false);
            }
            agent.ResetPath();
        }
    }

    public override void SetStop()
    {
        if (gameObject.activeInHierarchy && agent.enabled)
        {
            if (animator != null)
            {
                animator.SetBool("Move", false);
            }
            agent.ResetPath();
        }
        currentCommand = null;
    }

    public void SetSkill(RaycastHit hit)
    {
        TimeManager.instance.skill_Q_CoolReady = true;
        if (CheckCurrentWeapon(WeaponType.Severum))
        {
            StartCoolTime(ref severumAttack);
            isFast = true;
        }
        else
        {
            CurrentWeapon.SkillShot(hit, subWeapon);
        }
    }

    public void SetUltimate(RaycastHit cusurPos)
    {
        CurrentWeapon.UltimateShot(cusurPos, subWeapon);
    }

    public void GetExp(float expPoint)
    {
        if (levelPoint < 15)
        {
            if (currentXp + expPoint >= xpPerLevel)
            {
                if (levelPoint == 14)
                {
                    currentXp = xpPerLevel;
                }
                else
                {
                    float temp = currentXp + expPoint - xpPerLevel;
                    currentXp = temp;
                    xpPerLevel += 100;
                }
                if (levelPoint == 4)
                {
                    TimeManager.instance.UltActive();
                }
                levelPoint++;
                statUpWindow = true;

            }
            else
            {
                currentXp += expPoint;
            }
        }
    }

    /// <summary>
    /// 현재 가지고 있는 무기 확인
    /// <para>'0' : calibrum </para>
    /// <para>'1' : severum </para>
    /// <para>'2' : gravitum </para>
    /// '3' : infernum
    /// </summary>
    public bool CheckCurrentWeapon(WeaponType weaponType)
    {
        switch ((int)weaponType)
        {
            case 0:
                if (CurrentWeapon == calibrum) return true;
                else return false;
            case 1:
                if (CurrentWeapon == severum) return true;
                else return false;
            case 2:
                if (CurrentWeapon == gravitum) return true;
                else return false;
            case 3:
                if (CurrentWeapon == infernum) return true;
                else return false;
        }
        return false;
    }

    /// <summary>
    /// 현재 보조 무기 확인
    /// <para>'0' : calibrum </para>
    /// <para>'1' : severum </para>
    /// <para>'2' : gravitum </para>
    /// '3' : infernum
    /// </summary>
    public bool CheckSubWeapon(WeaponType weaponType)
    {
        switch ((int)weaponType)
        {
            case 0:
                if (subWeapon == calibrum) return true;
                else return false;
            case 1:
                if (subWeapon == severum) return true;
                else return false;
            case 2:
                if (subWeapon == gravitum) return true;
                else return false;
            case 3:
                if (subWeapon == infernum) return true;
                else return false;
        }
        return false;
    }

    public Unit SeverumTarget(Unit[] targetArr)
    {
        if (targetArr != null)
        {
            foreach (Unit currentUnit in targetArr)
            {
                if (currentUnit.CheckEnemy(this))
                {
                    if ((float)currentUnit.CurrentType == 0)
                    {
                        return currentUnit;
                    }
                }
            }
            return targetArr[0];
        }
        return null;
    }

    public void CalibrumMarkAttack(Unit target)
    {

        // 애니메이션 넣었을 때 공격 순간 움직임 멈추는 것 만들기
        if (target.calibrumMark && AtkReady && target != null)
        {
            if (CurrentType == ObjectType.Character || CurrentType == ObjectType.CasterMinion || CurrentType == ObjectType.MeleeMinion)
            {
                SetSight(target.transform.position);
            }

            if (CurrentWeapon != null && subWeapon != null)
            {
                //CurrentWeapon.CalibrumAttack(target);
                currentWeapon.NormalAttack(target);
                subWeapon.CalibrumAttack(target, CurrentWeapon.GetMuzzle());
                AtkReady = false;
                target.SetCalibrumMark(false);
            }

        }
    }

    public void UIInfomation()
    {
        UI_CharacterStatUI.instance.character.currentHp = health.Current;
        UI_CharacterStatUI.instance.character.maxHp = health.Max;
        UI_CharacterStatUI.instance.character.currentMana = mana.Current;
        UI_CharacterStatUI.instance.character.maxMana = mana.Max;
        UI_CharacterStatUI.instance.character.atkPower = atkStat.AtkPower;
        UI_CharacterStatUI.instance.character.atkSpeed = atkStat.AtkSpeed;
        UI_CharacterStatUI.instance.character.defPower = defPower;
        UI_CharacterStatUI.instance.character.currentWeaponBullet = CurrentWeapon.BulletAmount;
        UI_CharacterStatUI.instance.character.subWeaponBullet = subWeapon.BulletAmount;
        UI_CharacterStatUI.instance.character.maxExp = xpPerLevel;
        UI_CharacterStatUI.instance.character.exp = currentXp;
        UI_CharacterStatUI.instance.character.level = levelPoint;
    }


    public void EnqueueWeapon()
    {
        CurrentWeapon.ReLoading();

        CurrentWeapon = weaponQueue.Dequeue();
        weaponAppearance.ShowWeapon(CurrentWeapon);
    }

    public void WeaponChange()
    {
        CharacterWeapon temp;
        temp = CurrentWeapon;
        CurrentWeapon = subWeapon;
        subWeapon = temp;

        weaponAppearance.ShowWeapon(CurrentWeapon);
    }


    // Test용
    public string Text()
    {
        string text = $"Main : {CurrentWeapon} ({CurrentWeapon.BulletAmount}) / Sub : {subWeapon}\nNext : {weaponQueue.Peek()}\nxp : {currentXp:F0} / {xpPerLevel} level : {levelPoint}\nhealth: {health.Current:F0} / {health.Max}\nmana: {mana.Current:F0} / {mana.Max}\natkPower: {atkStat.AtkPower}\natkSpeed: {atkStat.AtkSpeed}\nDefPower: {defPower}";
        return text;
    }
}
