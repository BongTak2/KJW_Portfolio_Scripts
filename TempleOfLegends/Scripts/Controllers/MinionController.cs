using UnityEngine;

public class MinionController : Controller
{
    [SerializeField]
    private Unit target;

    Vector3 from;
    Vector3 to;

    protected override void OnEnable()
    {
        controlledMinion = GetComponent<Minion>();
        from = controlledMinion.currentRegion == Region.Red ? new Vector3(45, 0, 45) : new Vector3(-45, 0, -45);
        to = -from;
    }

    private void Update()
    {
        // Lerp() t°ª
        float myPosRate = Vector3.Distance(controlledMinion.transform.position, from) / (Vector3.Distance(controlledMinion.transform.position, from) + Vector3.Distance(controlledMinion.transform.position, to));

        target = controlledMinion.GetTarget();


        if (controlledMinion.CurrentCommand == null)
        {
            if (target == null)
            {
                controlledMinion.SetCommand(new Command_Move(controlledMinion, to));
            }
            else
            {
                controlledMinion.SetCommand(new Command_Attack(controlledMinion, target));
            }
        }

        if (target != null && controlledMinion.CurrentCommand.TryCast(out Command_Move move))
        {
            controlledMinion.SetCommand(new Command_Attack(controlledMinion, target));
        }


        //if (controlledMinion.CheckTarget())
        //{
        //    controlledMinion.SetCommand(new Command_Attack(controlledMinion, target));
        //}
        //else
        //{
        //    if (!controlledMinion.CurrentCommand.TryCast(out Command_Move move))
        //    {
        //        MoveMinion(myPosRate);
        //    }
        //}


        //if (controlledMinion.CurrentCommand == null)
        //{
        //    MoveMinion(myPosRate);
        //}

        //if (controlledMinion.changeCommand)
        //{
        //    if (target != null)
        //    {
        //        controlledMinion.SetCommand(new Command_Attack(controlledMinion, target));
        //    }
        //    else
        //    {
        //        if (myPosRate > 0.45f)
        //        {
        //            {
        //                controlledMinion.SetCommand(new Command_Move(controlledMinion, to));
        //            }
        //        }
        //        else
        //        {
        //            controlledMinion.SetCommand(new Command_Move(controlledMinion, new Vector3(0f, 0f, 0f)));
        //        }
        //    }
        //    controlledMinion.changeCommand = false;
        //}
    }
}
