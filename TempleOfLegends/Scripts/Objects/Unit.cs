using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum State
{
    Idle,
    Move,
    Attack,
    Slow,
    Stun,
    Die
}
public enum ObjectType
{
    Character,
    TrainingBot,
    CasterMinion,
    MeleeMinion,
    Turret,
    Nexus,
    RecoveryKit
}

public enum Region { Blue, Red }

public class Unit : MonoBehaviour
{
    [Header("�� Status")]
    [SerializeField] protected FloatAmount health;
    [SerializeField] protected AtkStatus atkStat;
    [SerializeField] protected float healthRegenRate;
    [SerializeField] protected float defPower;
    [SerializeField] protected float moveSpeed;
    //[SerializeField] protected float slowSpeed;
    [SerializeField] protected State currentState;
    [SerializeField] protected int levelPoint;

    protected float firstDef;
    protected float firstSpeed;

    public bool AtkReady { get; protected set; }
    public bool isStun { get; protected set; }
    public bool isSlow { get; protected set; }
    public bool isMove { get; protected set; }
    public bool isFast { get; protected set; }

    protected float firstHealth;
    protected float firstAtk;
    protected Vector3 moveDir;
    protected Vector3 sightDir;
    protected Vector3 lastAimPosition;
    protected float currentSpeed;
    protected float atkDelay;
    protected float movementMultiplier = 1.0f;

    protected Rigidbody rigid;
    protected CapsuleCollider capsule;
    protected NavMeshAgent agent;
    protected LineRenderer atkLine;
    protected Animator animator;

    protected Weapon currentWeapon;

    protected Command currentCommand;
    public Command CurrentCommand
    {
        get => currentCommand;
        protected set
        {
            currentCommand = value;
        }
    }

    public Region currentRegion;

    private ObjectType _currentType;
    public ObjectType CurrentType
    {
        get => _currentType;
        protected set
        {
            _currentType = value;
        }
    }

    protected PrefabType bulletType;

    protected List<DelayTime> coolTimeList = new List<DelayTime>();
    protected Dictionary<string, float> coolDown = new Dictionary<string, float>();
    protected Dictionary<string, bool> coolEnd = new Dictionary<string, bool>();

    public DelayTime gravitumMarkCool;
    protected DelayTime calibrumMarkCool;
    protected DelayTime stunCool;

    public bool gravitumMark { get; protected set; }
    public bool calibrumMark { get; protected set; }
    public bool gravitumMarkTrigger { get; protected set; }
    public bool calibrumMarkTrigger { get; protected set; }
    public bool stunTrigger { get; protected set; }

    /// <returns>�����̸� true, �Ʊ��̸� false</returns>
    public bool CheckEnemy(Unit other) => currentRegion != other.currentRegion;

    //���� �¾Ҵ°�, ���� ���ȴ°�, ��ΰ�
    public Action<Unit, Unit, float> OnDamage;

    protected virtual void OnEnable()
    {
        Initialize();
    }

    protected virtual void Update()
    {
        currentState = CalculateState();

        if (currentCommand is not null)
        {
            currentCommand.Stay();
        }

        if (coolTimeList.Count > 0)
        {
            for (int i = 0; i < coolTimeList.Count; i++)
            {
                if (coolEnd.ContainsKey(coolTimeList[i].name))
                {
                    coolEnd[coolTimeList[i].name] = StartReady(coolTimeList[i]);
                }
            }
        }
        AttackReady();

        if (coolEnd.Count > 0)
        {
            DelayEnd();
        }

        UnitUpdate();
        UIManager.instance.ChangeUnitHP(health.Current, health.Max);

    }

    protected virtual void Initialize()
    {
        SetBase();
        SetDelayTime();
        StatPerLevel(levelPoint);
        rigid = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
        agent = GetComponent<NavMeshAgent>();
        atkLine = GetComponentInChildren<LineRenderer>();

    }

    protected virtual void SetBase() { }

    /// <summary>
    /// ��Ÿ�� ����
    /// </summary>
    protected virtual void SetDelayTime()
    {
        gravitumMarkCool.SetCoolTime("gravitumMarkCool", 1.8f);
        calibrumMarkCool.SetCoolTime("calibrumMarkCool", 1.8f);
        stunCool.SetCoolTime("stunCool", 1f);
    }

    protected virtual void StatPerLevel(int _levelPoint) { }

    public virtual void SetCommand(Command _command)
    {
        if (currentCommand is not null)
        {
            currentCommand.End();
        }

        currentCommand = _command;

        if (currentCommand is not null)
        {
            currentCommand.Start();
        }
    }

    //protected DelayTime SetCoolTime(string name, float time, bool start = false)
    //{
    //    DelayTime temp;

    //    temp.name = name;
    //    temp.count = time;
    //    temp.startReady = start;

    //    return temp;
    //}

    protected bool StartReady(DelayTime _cooltime)
    {
        if (coolDown.ContainsKey(_cooltime.name))
        {
            if (coolDown[_cooltime.name] > 0f)
            {
                coolDown[_cooltime.name] -= Time.deltaTime;
            }
            else
            {
                ClearCool(_cooltime);
                return true;
            }
        }
        return false;
    }

    protected bool CoolEnd(DelayTime _cooltime)
    {
        if (coolEnd.ContainsKey(_cooltime.name))
        {
            return coolEnd[_cooltime.name];
        }
        return false;
    }

    public void StartCoolTime(ref DelayTime coolTime)
    {
        if (!coolTimeList.Contains(coolTime))
        {
            coolTimeList.Add(coolTime);
        }

        if (!coolDown.ContainsKey(coolTime.name))
        {
            coolDown.Add(coolTime.name, coolTime.count);
        }

        if (!coolEnd.ContainsKey(coolTime.name))
        {
            coolEnd.Add(coolTime.name, coolTime.startReady);
        }
    }

    protected void ClearCool(DelayTime coolTime)
    {
        if (coolDown.ContainsKey(coolTime.name))
        {
            coolDown.Remove(coolTime.name);
        }

        if (coolEnd.ContainsKey(coolTime.name))
        {
            coolEnd.Remove(coolTime.name);
        }
    }

    /// <summary>
    /// ������ �޴� �Լ�
    /// </summary>
    /// <param name="instigator">���� ���ȴ���</param>
    /// <param name="damage">���ط�</param>
    /// <param name="multiplier">���</param>
    /// <returns></returns>
    public virtual float TakeDamage(Unit instigator, float damage, float multiplier = 1)
    {
        float result = damage * multiplier * (float)(1 / (1 + (0.01 * defPower)));
        //Mathf.RoundToInt(damage * multiplier);
        result = Mathf.Min(result, health.Current);

        health.Current -= result;

        OnDamage?.Invoke(this, instigator, result);

        return result;
    }

    /// <summary>
    /// ȸ���� �ٷ� ȸ��
    /// </summary>
    /// <param name="heal">ȸ����</param>
    public virtual void TakeHeal(float heal, float multiplier = 1)
    {
        health.Current += RecoveryValue(heal, multiplier);
    }

    // �ִ� ü���� �ƴ� �� ü�� ȸ��
    public float RecoveryValue(float heal, float multiplier = 1)
    {
        float result = heal * multiplier;
        return result;
    }


    /// <summary>
    /// �ǽð� ���� ������Ʈ
    /// </summary>
    protected virtual void UnitUpdate()
    {
        if (isStun)
        {
            movementMultiplier = 0;
        }
        else if (isSlow)
        {
            movementMultiplier = 0.75f;
        }
        else if (isFast)
        {
            movementMultiplier = 1.15f;
        }
        else
        {
            movementMultiplier = 1f;
        }
        currentSpeed = moveSpeed * movementMultiplier;
        if (CurrentType != ObjectType.Turret)
            agent.speed = currentSpeed;


        if (gravitumMarkTrigger)
        {
            gravitumMarkTrigger = false;
            isSlow = true;
            if (animator != null)
            {
                animator.SetBool("Slow", true);
            }
            gravitumMark = true;
            if (!Gravitum.gravitumMarkUnits.Contains(this))
            {
                Gravitum.gravitumMarkUnits.Add(this);
            }
            //delayTimeFunc -= GravitumMarkCool;
            //delayTimeFunc += GravitumMarkCool;
            StartCoolTime(ref gravitumMarkCool);
        }

        if (calibrumMarkTrigger)
        {
            calibrumMarkTrigger = false;
            calibrumMark = true;
            //delayTimeFunc -= CalibrumMarkCool;
            //delayTimeFunc += CalibrumMarkCool;
            StartCoolTime(ref calibrumMarkCool);
        }

        if (stunTrigger)
        {
            stunTrigger = false;
            isStun = true;
            coolEnd[gravitumMarkCool.name] = false;
            StartCoolTime(ref stunCool);
        }

    }

    /// <summary>
    /// ������ ���� ���� �Լ� ����
    /// </summary>
    protected virtual void DelayEnd()
    {
        if (CoolEnd(gravitumMarkCool))
        {
            GravitumMarkCool();
            coolEnd.Remove(gravitumMarkCool.name);
        }
        if (CoolEnd(calibrumMarkCool))
        {
            CalibrumMarkCool();
            coolEnd.Remove(calibrumMarkCool.name);
        }
        if (CoolEnd(stunCool))
        {
            isStun = false;
            coolEnd.Remove(stunCool.name);
        }
    }

    public virtual void SetMove(Vector3 wantPos)
    {
        if (CurrentType == ObjectType.Turret)
        {
            return;
        }
        moveDir = wantPos;

        agent.stoppingDistance = 0f;
        if (gameObject.activeInHierarchy)
        {
            agent.SetDestination(moveDir);
        }
    }

    public virtual State CalculateState()
    {
        if (health.Current <= 0)
        {
            SetDie();
            return State.Die;
        }

        if (currentState == State.Stun || currentState == State.Attack || currentState == State.Slow)
            return currentState;

        if (CurrentType != ObjectType.Turret)
        {
            if (agent.hasPath)
            {
                return State.Move;
            }
        }
        return State.Idle;
    }

    public virtual bool SetSlow(bool value) => isSlow = value;
    public virtual bool SetStun(bool value) => stunTrigger = value;

    public virtual void SetMoveStop()
    {
        if (CurrentType != ObjectType.Turret && agent.hasPath)
        {
            agent.ResetPath();
        }
    }

    public void SetCalibrumMark(bool value)
    {
        calibrumMark = value;
    }

    public virtual void SetDie()
    {
        // �״� �ִϸ��̼� ���� ��

        PoolManager.Destroy(gameObject);

    }

    /// <summary>
    /// �ൿ ���� �� ��� �ʱ�ȭ
    /// </summary>
    public virtual void SetStop()
    {
        if (CurrentType != ObjectType.Turret && gameObject.activeInHierarchy && agent.enabled)
        {
            agent.ResetPath();
        }
        currentCommand = null;
    }

    public virtual bool CheckAttackRange(Vector3 targetPos, float multiplier = 1)
    {
        return transform.position.DistanceXZ(targetPos) <= atkStat.AtkRange * multiplier;
    }

    public virtual void SetAttackMove(Vector3 targetPos)
    {
        if (!CheckAttackRange(targetPos))
        {
            SetMove(targetPos);
        }
        else
        {
            if (CurrentType != ObjectType.Turret)
            {
                agent.ResetPath();
            }
        }
    }

    public virtual Vector3 SetSight(Vector3 wantPos, bool yaw = true, bool pitch = true)
    {
        lastAimPosition = wantPos;

        float yawDelta = sightDir.y;

        Vector3 wantDir = wantPos - transform.position;

        if (yaw)
            sightDir.y = 90 - wantDir.ToHorizontalAngle();
        if (pitch)
            sightDir.x = -wantDir.ToVerticalAngle();
        sightDir.z = 0;

        switch (currentState)
        {
            case State.Stun: case State.Die: break;
            default:
                transform.rotation = Quaternion.Euler(0, sightDir.y, 0);
                break;
        }

        yawDelta = sightDir.y - yawDelta;

        return sightDir;
    }

    public virtual void SetAttack(Unit target)
    {
        // �ִϸ��̼� �־��� �� ���� ���� ������ ���ߴ� �� �����
        if (AtkReady && target != null)
        {
            if (CurrentType == ObjectType.Character || CurrentType == ObjectType.CasterMinion || CurrentType == ObjectType.MeleeMinion)
            {
                SetSight(target.transform.position);
            }
            //SetSight(target.transform.position);

            if (currentWeapon != null)
            {
                AtkReady = false;
                currentWeapon.NormalAttack(target);
            }

        }
    }

    private void AttackReady()
    {
        if (!AtkReady)
        {
            atkDelay += Time.deltaTime;
        }
        if (atkDelay > (1 / atkStat.AtkSpeed))
        {
            AtkReady = true;
            atkDelay = 0f;
        }
    }

    public void SetCalibrumMarkTrigger(bool value)
    {
        calibrumMarkTrigger = value;
    }

    public void SetGravitumMarkTrigger(bool value)
    {
        gravitumMarkTrigger = value;
    }

    /// <summary>
    /// �ش� ����  ��ó  ���ֵ� �Ÿ��� �迭
    /// </summary>
    /// <param name="hitPoint">�ش� ����</param>
    /// <param name="radius">�ν� ����</param>
    public Unit[] FindNearUnitsFromPoint(Vector3 hitPoint, float radius = 6f)
    {
        Collider[] colliders = Physics.OverlapSphere(hitPoint, radius);

        if (colliders.Length <= 0) return null;

        //List<Collider> tempCol = new List<Collider>(colliders);
        List<CapsuleCollider> tempCol = new List<CapsuleCollider>();
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].TryCast(out CapsuleCollider capsuleCollider))
            {
                tempCol.Add(capsuleCollider);
            }
        }

        List<Unit> unitList = new List<Unit>();
        tempCol.RemoveAll((target) =>
        {
            if (target.gameObject == gameObject)
            {
                return true;
            }

            bool exist = target.TryGetComponent(out Unit resultUnit);

            if (exist && resultUnit.currentRegion != currentRegion)
            {
                if (!unitList.Contains(resultUnit))
                    unitList.Add(resultUnit);
            }

            return exist == false;
        });

        if (unitList.Count <= 0) return null;

        unitList.Sort((a, b) =>
        {
            float disA = Vector3.Distance(hitPoint, a.transform.position);
            float disB = Vector3.Distance(hitPoint, b.transform.position);
            return disA > disB ? 1 : -1;
        });

        return unitList.ToArray();
    }

    /// <summary>
    /// �ش� �����̶� ���� ����� ����
    /// </summary>
    public Unit FindNearEnemyFromPoint(Vector3 hitPoint)
    {
        if (FindNearUnitsFromPoint(hitPoint) is not null) return FindNearUnitsFromPoint(hitPoint)[0];

        else return null;
    }
    /// <summary>
    /// �ڱ��ڽ��̶� ����� ����
    /// </summary>
    public Unit FindNearEnemy()
    {
        return FindNearEnemyFromPoint(transform.position);
    }

    /// <summary>
    /// �ڽ��̶� ����� ���� ������
    /// </summary>
    public Unit[] FindNearUnits()
    {
        return FindNearUnitsFromPoint(transform.position);
    }

    public bool RangeMark(bool active)
    {
        atkLine.transform.localScale = new Vector3(2f * atkStat.AtkRange, 0.1f, 2f * atkStat.AtkRange);

        atkLine.enabled = active;

        return active;
    }

    public void AgentStart()
    {
        if (agent != null)
        {
            agent.enabled = false;
            agent.enabled = true;
        }
    }

    public float DamagePower() => atkStat.AtkPower;
    public State GetState() => currentState;
    public Vector3 GetMove() => moveDir;
    public Vector3 GetSight() => sightDir;


    private void GravitumMarkCool()
    {
        isSlow = false;
        if (animator != null)
        {
            animator.SetBool("Slow", false);
        }
        gravitumMark = false;
        if (Gravitum.gravitumMarkUnits.Contains(this))
        {
            Gravitum.gravitumMarkUnits.Remove(this);
        }
    }

    private void CalibrumMarkCool()
    {
        calibrumMark = false;
    }

    public float LevelUpStat(float coefficient, int _levelPoint)
    {
        return coefficient * (_levelPoint - 1) * (0.8f + 0.025f * (_levelPoint - 1));

    }
}
