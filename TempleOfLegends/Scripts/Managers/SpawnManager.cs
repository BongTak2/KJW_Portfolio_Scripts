using UnityEngine;

public class SpawnManager : Manager
{
    public static SpawnManager instance;

    public override void Initialize()
    {
        base.Initialize();
        this.Singleton(ref instance);
    }


    [SerializeField]
    private float setDelay;
    private const float spawnDelay = 0.8f;
    [SerializeField]
    private float setCycle;
    private const float spawnCycle = 30f;
    private const float firstSpawnTime = 1f;
    private int count;
    private const int minionCount = 6;

    public static GameObject player;

    private void OnEnable()
    {
        setCycle = firstSpawnTime;
        setDelay = spawnDelay;
        //firstDelay = firstSpawnTime;
        count = minionCount;
        player = SpawnObject(PrefabType.Prefabs__Object__Character, 1);
        //PlayerSpawn();
    }

    public override void Update()
    {
        if (setCycle > 0)
        {
            setCycle -= Time.deltaTime;
        }
        else
        {
            MinionSpawn();
        }
        if (player.activeInHierarchy == false)
        {
            if (TimeManager.instance.DeathRespawn_Ready)
            {
                player = SpawnObject(PrefabType.Prefabs__Object__Character, 1);
                TimeManager.instance.RespawnStart();
                //PlayerSpawn();
            }
        }
    }

    public GameObject SpawnObject(PrefabType prefabType, int index)
    {
        GameObject obj = PoolManager.Instantiate(prefabType);
        obj.transform.position = SpawnPoint.GetSpawnPoint(index);


        obj.GetComponent<Unit>().AgentStart();
        return obj;
    }

    public void PlayerSpawn()
    {
        player = SpawnObject(PrefabType.Prefabs__Object__Character, 1);
        player.GetComponent<Character>().AgentStart();
        
    }

    public void MinionSpawn()
    {
        if (setDelay > 0)
        {
            setDelay -= Time.deltaTime;
        }
        else
        {
            if (count > 3)
            {
                SpawnObject(PrefabType.Prefabs__Object__Red_MeleeMinion, 2);
                SpawnObject(PrefabType.Prefabs__Object__Blue_MeleeMinion, 0);
                count--;
            }
            else if (count > 0 && count <= 3)
            {
                SpawnObject(PrefabType.Prefabs__Object__Red_CasterMinion, 2);
                SpawnObject(PrefabType.Prefabs__Object__Blue_CasterMinion, 0);
                count--;
            }
            else
            {
                count = minionCount;
                setCycle = spawnCycle;
            }
            setDelay = spawnDelay;
        }

    }
}
