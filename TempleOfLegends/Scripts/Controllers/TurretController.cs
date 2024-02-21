public class TurretController : Controller
{
    private Unit target;
    protected override void OnEnable()
    {
        controlledTurret = GetComponent<Turret>();

    }
    private void Update()
    {
        if (controlledTurret.CheckTarget())
        {
            target = controlledTurret.GetTarget();
        }
        ControlUpdate();

    }
    public void ControlUpdate()
    {
        if (target != null)
            controlledTurret.SetCommand(new Command_Attack(controlledTurret, target));
        else
        {
            controlledTurret.SetStop();
        }
    }

}
