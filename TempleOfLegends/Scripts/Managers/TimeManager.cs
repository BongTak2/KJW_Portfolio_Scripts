using UnityEngine;

/// <summary>
/// SetDelayTime에서 쿨타임 설정 -> 시작할 때 [delayTimeFunc에 함수 추가 , StartCoolTime로 실행 -> DelayActClear에서 함수 빼기
/// </summary>
public struct DelayTime
{
    public string name;
    public float count;
    public bool startReady;

    /// <param name="_name">DelayTime 이름</param>
    /// <param name="_time">쿨타임 시간</param>
    /// <param name="_start"></param>
    public void SetCoolTime(string _name, float _time, bool _start = false)
    {
        name = _name;
        count = _time;
        startReady = _start;
    }
}

public class TimeManager : Manager
{
    public static TimeManager instance;

    private float _playTime;
    public float PlayTime => _playTime;

    private float levelUpTime;
    private const float levelUpDelay = 30f;

    public int minionLevel;

    public bool ultCoolReady;
    public float ultCool;
    private const float ultCoolTime = 30f;
    public bool UltReady { get; protected set; }

    public bool spell_Heal_CoolReady;
    public float spell_Heal_Cool;
    private const float spell_Heal_CoolTime = 90f;
    public bool Spell_Heal_Ready { get; protected set; }

    public bool spell_Flash_CoolReady;
    public float spell_Flash_Cool;
    private const float spell_Flash_CoolTime = 120f;
    public bool Spell_Flash_Ready { get; protected set; }

    public bool skill_Q_CoolReady;
    public float skill_Q_Cool;
    private const float skill_Q_CoolTime = 6f;
    public bool Skill_Q_Ready { get; protected set; }

    public bool deathRespawn_CoolReady;
    public float deathRespawn_Cool;
    private const float deathRespawn_CoolTime = 20f;
    public bool DeathRespawn_Ready { get; protected set; }

    public override void Initialize()
    {
        base.Initialize();
        this.Singleton(ref instance);

        minionLevel = 1;
        levelUpTime = levelUpDelay;
        ultCool = ultCoolTime;
        spell_Heal_Cool = spell_Heal_CoolTime;
        spell_Flash_Cool = spell_Flash_CoolTime;
        skill_Q_Cool = skill_Q_CoolTime;
        deathRespawn_Cool = deathRespawn_CoolTime;
        Spell_Heal_Ready = true;
        Spell_Flash_Ready = true;
        Skill_Q_Ready = true;
        DeathRespawn_Ready = true;
    }

    public override void Update()
    {
        _playTime += Time.deltaTime;

        MinionUpdate();

        if (ultCoolReady)
        {
            UltCoolUpdate();
        }
        if (spell_Heal_CoolReady)
        {
            Spell_D_CoolUpdate();
        }
        if (spell_Flash_CoolReady)
        {
            Spell_F_CoolUpdate();
        }
        if (skill_Q_CoolReady)
        {
            Skill_Q_CoolUpdate();
        }
        if (deathRespawn_CoolReady)
        {
            DeathRespawn_CoolUpdate();
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            ultCool = 1f;
        }
    }

    private void MinionUpdate()
    {
        if (levelUpTime > 0f)
        {
            levelUpTime -= Time.deltaTime;
        }
        else
        {
            levelUpTime = levelUpDelay;
            minionLevel++;
        }
    }

    private void UltCoolUpdate()
    {
        //Debug.Log(ultCool);
        if (ultCool > 0f)
        {
            ultCool -= Time.deltaTime;
            UltReady = false;
        }
        else
        {
            ultCool = ultCoolTime;
            ultCoolReady = false;
            UltReady = true;
        }
    }

    private void Spell_D_CoolUpdate()
    {
        if (spell_Heal_Cool > 0f)
        {
            spell_Heal_Cool -= Time.deltaTime;
            Spell_Heal_Ready = false;
        }
        else
        {
            spell_Heal_Cool = spell_Heal_CoolTime;
            spell_Heal_CoolReady = false;
            Spell_Heal_Ready = true;
        }
    }

    private void Spell_F_CoolUpdate()
    {
        if (spell_Flash_Cool > 0f)
        {
            spell_Flash_Cool -= Time.deltaTime;
            Spell_Flash_Ready = false;
        }
        else
        {
            spell_Flash_Cool = spell_Heal_CoolTime;
            spell_Flash_CoolReady = false;
            Spell_Flash_Ready = true;
        }
    }

    private void Skill_Q_CoolUpdate()
    {
        if (skill_Q_Cool > 0f)
        {
            skill_Q_Cool -= Time.deltaTime;
            Skill_Q_Ready = false;
        }
        else
        {
            skill_Q_Cool = skill_Q_CoolTime;
            skill_Q_CoolReady = false;
            Skill_Q_Ready = true;
        }
    }

    private void DeathRespawn_CoolUpdate()
    {
        if (deathRespawn_Cool > 0f)
        {
            deathRespawn_Cool -= Time.deltaTime;
            DeathRespawn_Ready = false;
        }
        else
        {
            deathRespawn_Cool = deathRespawn_CoolTime;
            deathRespawn_CoolReady = false;
            DeathRespawn_Ready = true;
        }
    }

    public void UltActive()
    {
        UltReady = true;
    }

    public void RespawnStart()
    {
        DeathRespawn_Ready = true;
    }
}
