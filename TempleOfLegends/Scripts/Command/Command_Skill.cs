using UnityEngine;

public class Command_Skill : Command
{
    public RaycastHit cusurPos;
    public new Character owner;

    protected float delayTime;
    protected const float skillDelayTime = 0.2f;

    public Command_Skill(Character _owner, RaycastHit _cusurPos) : base(_owner)
    {
        owner = _owner;
        cusurPos = _cusurPos;
    }

    public override void Start()
    {
        if (owner.CheckCurrentWeapon(WeaponType.Severum))
        {
            delayTime = 0;
        }
        else
        {
            delayTime = skillDelayTime;
        }
    }

    public override void Stay()
    {
        delayTime -= Time.deltaTime;

        if (delayTime < 0)
        {
            owner.SetSkill(cusurPos);
            End();
        }
    }

    public override void End()
    {
        delayTime = skillDelayTime;
        owner.SetStop();
    }
}
