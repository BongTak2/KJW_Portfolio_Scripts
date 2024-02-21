using System.Collections.Generic;
using UnityEngine;

public class RecoveryKit : MonoBehaviour
{
    [SerializeField]
    private List<Character> unitList = new List<Character>();

    [SerializeField]
    private float setDelay;
    private const float spawnDelay = 30f;

    private const float radius = 10f;    // radius => 인식 범위

    BoxCollider boxCol;
    Transform mesh;
    Transform line;

    private void OnEnable()
    {
        setDelay = 30f;
        mesh = transform.GetChild(0);
        line = transform.GetChild(1);
        boxCol = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        Appearance(false);
        mesh.gameObject.SetActive(false);
        line.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!boxCol.isTrigger)
        {
            if (setDelay > 0)
            {
                setDelay -= Time.deltaTime;
            }
            else
            {
                Appearance(true);
                setDelay = spawnDelay;
            }
        }
        else
        {
            mesh.gameObject.SetActive(true);
        }

        if (active)
        {
            mesh.gameObject.SetActive(false);
            if (activeDelay > 0)
            {
                activeDelay -= Time.deltaTime;
            }
            else
            {
                active = false;
                line.gameObject.SetActive(false);
                Recovery();
            }
        }
    }

    private bool Appearance(bool _active)
    {
        boxCol.isTrigger = _active;

        return _active;
    }

    public List<Character> FindNearUnitsFromPoint()
    {
        Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, radius);   

        if (colliders.Length <= 0) return null;

        List<Collider> tempCol = new List<Collider>(colliders);

        tempCol.RemoveAll((target) =>
        {
            if (target.gameObject == gameObject)
            {
                return true;
            }

            bool exist = target.TryGetComponent(out Character resultUnit);

            if (exist)
            {
                unitList.Add(resultUnit);
            }

            return exist == false;
        });

        if (unitList.Count <= 0) return null;

        return unitList;
    }

    public void Recovery()
    {
        if (unitList.Count != 0)
        {
            for (int i = 0; i < unitList.Count; i++)
            {
                unitList[i].HealPack();
            }
        }
    }

    bool active;
    [SerializeField]
    float activeDelay;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Character target))
        {
            line.gameObject.SetActive(true);
            FindNearUnitsFromPoint();
            Appearance(false);
            active = true;
            activeDelay = 2f;
            target.HealPack(0.05f);
        }
    }
}
