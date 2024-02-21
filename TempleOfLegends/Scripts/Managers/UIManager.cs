using System;
using UnityEngine.UI;

public class UIManager : Manager
{
    public static UIManager instance;

    public Action<float, float> OnChangeUnitHealth;

    public override void Initialize()
    {
        base.Initialize();
        this.Singleton(ref instance);
    }

    public void ChangeUnitHP(float current, float max)
    {
        OnChangeUnitHealth?.Invoke(current, max);
    }    

}
