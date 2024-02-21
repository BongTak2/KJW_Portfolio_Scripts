using UnityEngine;

public interface IPoolInitializer
{
    public void PoolInitialize(params object[] parameterArray);
}

public class PoolObject : MonoBehaviour
{
    public PrefabType origin;

    public void Initialize(int currentID)
    {
        foreach (var currentInitializer in GetComponentsInChildren<IPoolInitializer>())
        {
            currentInitializer.PoolInitialize(currentID);
        }
    }
}
