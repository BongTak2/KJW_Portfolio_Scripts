using UnityEngine;

public class Nexus : Turret
{
    public GameObject panel;
    protected override void Initialize()
    {
        base.Initialize();

    }
    protected override void SetBase()
    {
        CurrentType = ObjectType.Turret;
        bulletType = PrefabType.Prefabs__Bullet__TurretBullet;

        healthRegenRate = 0f;

        atkStat.AtkRange = 12.5f;

        moveSpeed = 0f;

        turretNum = 3;

        firstDef = 100f;
        defPower = firstDef;
    }

    public override void SetDie()
    {
        Time.timeScale = 0;
        panel.SetActive(true);
    }
}