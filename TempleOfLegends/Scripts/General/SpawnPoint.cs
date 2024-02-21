using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    protected static SpawnPoint[] spawns;

    
    public static Vector3 GetSpawnPoint(int index)
    {
        if (spawns == null)
            spawns = FindObjectsOfType<SpawnPoint>();

        if (spawns != null && spawns.Length > 0)
        {
            SpawnPoint targetSpawnPoint = spawns[index];
            return targetSpawnPoint.transform.position;
        }
        else
        {
            return Vector3.zero;
        }
    }
}
