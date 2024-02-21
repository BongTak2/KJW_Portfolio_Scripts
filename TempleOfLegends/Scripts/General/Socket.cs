using UnityEngine;

public class Socket : MonoBehaviour
{
    protected WeaponAppearance owned;

    public virtual void SetAppearance(WeaponAppearance target)
    {
        if (owned) 
        { 
            owned.RemoveSocket(); 
        }
        
        owned = target;

        if (owned) 
        { 
            owned.AddSocket(this); 
        }
    }

    public virtual WeaponAppearance GetAppearance() => owned;
}
