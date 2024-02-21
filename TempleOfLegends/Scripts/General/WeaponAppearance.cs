using System.Collections.Generic;
using UnityEngine;

public class WeaponAppearance : MonoBehaviour, IPoolInitializer
{
    protected Character owned;

    protected Socket handSocket;

    //protected State prevState = State.Idle;

    public void PoolInitialize(params object[] parameterArray)
    {
        //Initialize(transform.GetComponentInParent<Character>());
        Initialize(transform.GetComponent<Character>());
    }

    public void Initialize(Character target)
    {
        //prevState = target.GetState();

        if (target == owned) return;

        if (target != null)
        {
            handSocket = target.GetComponentInChildren<Socket>();
            handSocket.SetAppearance(this);
            ShowWeapon(target.CurrentWeapon);

        }

        owned = target;
    }

    public virtual void ShowWeapon(Weapon target)
    {
        GameObject weaponInstance = null;

        if (target.TryCast(out Calibrum calibrum))
        {
            weaponInstance = PoolManager.Instantiate(calibrum.WeaponPrefab);
        }
        else if (target.TryCast(out Severum severum))
        {
            weaponInstance = PoolManager.Instantiate(severum.WeaponPrefab);
        }
        else if (target.TryCast(out Gravitum gravitum))
        {
            weaponInstance = PoolManager.Instantiate(gravitum.WeaponPrefab);
        }
        else if (target.TryCast(out Infernum infernum))
        {
            weaponInstance = PoolManager.Instantiate(infernum.WeaponPrefab);
        }
        List<GameObject> prevAttached = new List<GameObject>();

        DetachObjectFromSocket(handSocket, ref prevAttached);

        while (prevAttached.Count > 0)
        {
            PoolManager.Destroy(prevAttached[0]);
            prevAttached.RemoveAt(0); 
        }
        //DetachObjectFromSocket(handSocket);

        if (weaponInstance == null) return;

        if (handSocket == null)
        {
            PoolManager.Destroy(weaponInstance);
            return;
        }

        AttachObjectToSocket(handSocket, weaponInstance);

        Socket currentSocket = weaponInstance.GetComponentInChildren<Socket>();

        if (currentSocket)
        {
            target.SetMuzzle(currentSocket);
        }
    }

    public virtual void AttachObjectToSocket(Socket currentSocket, GameObject target)
    {
        if (currentSocket == null) return;

        target.transform.SetParent(currentSocket.transform);
        target.transform.localPosition = Vector3.zero;
        target.transform.localRotation = Quaternion.identity;
    }
    /*
    public virtual void DetachObjectFromSocket(Socket currentSocket)
    {
        if (currentSocket == null) return;

        if (currentSocket.transform.childCount > 0)
        {
            Transform currentChild = currentSocket.transform.GetChild(0);

            PoolManager.Destroy(currentChild.gameObject);
        }
    }*/
    public virtual void DetachObjectFromSocket(Socket currentSocket, ref List<GameObject> result)
    {
        if (currentSocket == null) return;

        if (result == null) result = new List<GameObject>();

        while (currentSocket.transform.childCount > 0)
        {
            Transform currentChild = currentSocket.transform.GetChild(0);

            result.Add(currentChild.gameObject);

            currentChild.SetParent(null);
        }
    }

    public virtual void AddSocket(Socket target)
    {
        if (handSocket == null)
        {
            handSocket = target;
        }
    }

    public virtual void RemoveSocket()
    {
        handSocket = null;
    }
    public virtual Socket GetSocket() => handSocket;
}
