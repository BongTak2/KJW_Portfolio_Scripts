public class Command_Attack : Command
{
    public Unit target;

    public Command_Attack(Unit _owner, Unit _target) : base(_owner)
    {
        target = _target;
    }

    public override void Start()
    {
        if (owner.CurrentType != ObjectType.Turret)
        {
            owner.SetAttackMove(target.transform.position);
        }
    }

    public override void Stay()
    {
        if (target != null && owner.CheckEnemy(target))
        {
            if (owner.TryCast(out Character character) && target.calibrumMark)
            {
                if (character.CheckAttackRange(target.transform.position, 1.8f))
                {
                    character.CalibrumMarkAttack(target);
                    owner.SetMoveStop();
                    End();
                }
                else
                {
                    owner.SetMove(target.transform.position);
                }
            }
            else if (owner.CheckAttackRange(target.transform.position))
            {
                owner.SetAttack(target);
                owner.SetMoveStop();
            }
            else
            {
                owner.SetMove(target.transform.position);
            }

        }

        if (target.gameObject.activeInHierarchy == false)
        {
            target = null;
            End();
        }
    }

    public override void End()
    {
        owner.SetStop();
    }
}
