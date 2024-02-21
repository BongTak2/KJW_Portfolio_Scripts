using UnityEngine;

public class Turret : AIUnit
{
    [SerializeField] protected int turretNum;
    private Transform cannon;
    
    protected override void Initialize()
    {
        base.Initialize();
        StatPerLevel(turretNum);
        cannon = transform.GetChild(0);
        
    }
    
    //private IEnumerator Start()
    //{
    //    Vector3 origin;

    //    origin = transform.position;

    //    yield return new WaitForSeconds(0.3f);
    //    agent.enabled = false;
    //    transform.position = origin;

    //}
    protected override void Update()
    {
        base.Update();

        if (target != null)
        {
            SetSight(target.transform.position);
        }
    }

    protected override void SetBase()
    {
        CurrentType = ObjectType.Turret;
        bulletType = PrefabType.Prefabs__Bullet__TurretBullet;

        healthRegenRate = 0f;

        // °ø°Ý½ºÅÈÀº ÃÑ¸¶´Ù ´Ù¸§
        atkStat.AtkSpeed = 0.5f;
        atkStat.AtkRange = 15.625f;

        moveSpeed = 0f;
        //slowSpeed = 0f;
    }
    protected override void StatPerLevel(int num)
    {
        switch (num)
        {
            case 1:
                {
                    health.Max = 1500f;
                    health.Current = health.Max;

                    atkStat.AtkPower = 100f;
                    defPower = 30f;
                    break;
                }
            case 2:
                {
                    health.Max = 2000f;
                    health.Current = health.Max;

                    atkStat.AtkPower = 300f;
                    defPower = 40f;
                    break;
                }
            case 3:
                {
                    health.Max = 3000f;
                    health.Current = health.Max;

                    atkStat.AtkPower = 500f;
                    break;
                }
        }

    }

    public override void SetDie()
    {
        if (transform.CompareTag("Turret"))
        {
            gameObject.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
        }
    }

    public void SetCapsuleActive(bool value)
    {
        capsule.enabled = value;
    }

    public override Vector3 SetSight(Vector3 wantPos, bool yaw = true, bool pitch = true)
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
                cannon.rotation = Quaternion.Euler(0, sightDir.y, 0);
                break;
        }

        yawDelta = sightDir.y - yawDelta;

        return sightDir;
    }


}
