using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PoolRequest
{
    public PrefabType prefab;
    public uint amout;
}

public class PoolManager : Manager
{
    public static PoolManager instance;

    protected Dictionary<PrefabType, GameObject> prefabTable = null;
    protected Dictionary<PrefabType, Queue<GameObject>> waitTable = null;
    protected Dictionary<PrefabType, Transform> rootTable = null;

    public PoolRequest[] request;

    private void Awake()
    {
        this.Singleton(ref instance);
    }

    public override void Initialize()
    {
        this.Singleton(ref instance);
        base.Initialize();

        if (prefabTable == null)
        {
            prefabTable = new Dictionary<PrefabType, GameObject>();
            waitTable = new Dictionary<PrefabType, Queue<GameObject>>();
            rootTable = new Dictionary<PrefabType, Transform>();

            foreach (var currentRequest in request)
            {
                AddPrefab(currentRequest.prefab, currentRequest.amout);
            }
        }
    }

    public static GameObject Instantiate(PrefabType wantType, int currentID = -1)
    {
        GameObject result;

        if (instance.waitTable.TryGetValue(wantType, out Queue<GameObject> queue))
        {
            if (queue.Count == 0)
            {
                instance.PrepareInstance(wantType, 2);
            }

            result = queue.Dequeue();

            if (!result.activeSelf)
            {
                result.SetActive(true);
            }

            result.GetOrAddComponent<PoolObject>().Initialize(currentID);
        }
        else
        {
            result = Instantiate(ResourceManager.GetPrefab(wantType));
        }

        return result;
    }

    public static void Destroy(GameObject target)
    {
        if (target.TryGetComponent<PoolObject>(out PoolObject asPool))
        {
            if (instance.waitTable.TryGetValue(asPool.origin, out Queue<GameObject> queue))
            {
                queue.Enqueue(target);
                target.transform.SetParent(instance.rootTable[asPool.origin]);
                target.SetActive(false);

                return;
            }
        }

        GameObject.Destroy(target);

    }

    public void AddPrefab(PrefabType wantPrefabType, uint amount)
    {
        prefabTable.Add(wantPrefabType, ResourceManager.GetPrefab(wantPrefabType));
        waitTable.Add(wantPrefabType, new Queue<GameObject>());
        rootTable.Add(wantPrefabType, new GameObject(wantPrefabType.ToString()).transform);

        PrepareInstance(wantPrefabType, amount);
    }

    public void PrepareInstance(PrefabType wantType, uint amount)
    {
        if (waitTable.TryGetValue(wantType, out Queue<GameObject> queue))
        {
            if (prefabTable.TryGetValue(wantType, out GameObject prefab) && prefab)
            {
                Transform rootTransform = rootTable[wantType];

                for (uint i = 0; i < amount; i++)
                {
                    GameObject instance = Instantiate(prefab, rootTransform);
                    instance.GetOrAddComponent<PoolObject>().origin = wantType;
                    if (wantType != PrefabType.Prefabs__Object__Character)
                    {
                        instance.SetActive(false);
                    }

                    queue.Enqueue(instance);
                }
            }
        }
    }
}
