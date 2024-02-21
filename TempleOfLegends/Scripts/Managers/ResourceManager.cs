using System.Collections.Generic;
using UnityEngine;

public enum PrefabType
{
    None = -1,
    Prefabs__Object__Character,
    Prefabs__Object__TrainingBot,
    Prefabs__Object__Red_MeleeMinion,
    Prefabs__Object__Red_CasterMinion,
    Prefabs__Object__Blue_MeleeMinion,
    Prefabs__Object__Blue_CasterMinion,
    Prefabs__Object__RecoveryKit,
    Prefabs__Bullet__MeleeBullet,
    Prefabs__Bullet__CasterBullet,
    Prefabs__Bullet__TurretBullet,
    Prefabs__Bullet__CalibrumBullet,
    Prefabs__Bullet__SeverumBullet,
    Prefabs__Bullet__GravitumBullet,
    Prefabs__Bullet__InfernumBullet,
    Prefabs__Weapon__Calibrum,
    Prefabs__Weapon__Severum,
    Prefabs__Weapon__Gravitum,
    Prefabs__Weapon__Infernum,
    Prefabs__Skill__CalibrumSkill,
    Prefabs__Skill__Ultimate,
    Prefabs__Skill__InfernumAddBullet,
    Length
}

public class ResourceManager : Manager
{
    public static ResourceManager instance;

    protected Dictionary<PrefabType, GameObject> prefabDictionary = null;

    protected GameObject prefabObject;

    public override void Initialize()
    {
        this.Singleton(ref instance);
        base.Initialize();

        if (prefabDictionary == null)
        {
            prefabDictionary = new Dictionary<PrefabType, GameObject>();

            for (PrefabType currentPrefab = (PrefabType)0; currentPrefab < PrefabType.Length; currentPrefab++)
            {

                string path = currentPrefab.ToString().Replace("__", "/");

                prefabDictionary.Add(currentPrefab, Resources.Load<GameObject>(path));
            }
        }
    }

    public static GameObject GetPrefab(PrefabType wantType)
    {
        if (instance == null || instance.prefabDictionary == null || !instance.prefabDictionary.ContainsKey(wantType))
        {
            return null;
        }

        return instance.prefabDictionary[wantType];
    }

    public static GameObject GetObject(PrefabType wantPrefab)
    {
        string path = wantPrefab.ToString().Replace("__", "/");

        GameObject result = Resources.Load<GameObject>(path);

        Instantiate(result);
        return result;
    }
}
