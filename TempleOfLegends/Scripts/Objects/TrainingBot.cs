using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingBot : Unit
{
    protected override void SetBase()
    {
        levelPoint = 0;
        firstHealth = 10000f;
        health.Max = firstHealth;
        health.Current = firstHealth;
        healthRegenRate = 1000f;

        firstAtk = 0f;
        atkStat.AtkPower = firstAtk;
        atkStat.AtkRange = 0f;
        firstSpeed = 0f;
        atkStat.AtkSpeed = firstSpeed;

        firstDef = 100f;
        defPower = firstDef;
        moveSpeed = 0f;

    }
}
