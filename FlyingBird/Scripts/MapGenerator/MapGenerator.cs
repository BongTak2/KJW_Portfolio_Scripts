using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum ItemType
    {
        CLEARITEM,
        SKILLITEM
    }

    [Header("Prefabs")]
    [SerializeField] protected List<GameObject> obstacleList = new List<GameObject>();
    [SerializeField] protected GameObject clearItemPrefab;
    [SerializeField] protected GameObject skillItemPrefab;

    [Header("Essential GameObject")]
    public GameObject player;
    [SerializeField] protected GameObject endMap;

    protected PlayerController playerController;

    protected float sectionX;     // 구간 x좌표
    protected const float sectionScale = 100f;    // 구간 크기

    protected float currentSection;
    protected float objSection;

    protected int maxClearItem = 3;
    protected const int maxSkillItem = 5;

    // 구간별 활성화 된 아이템 타입별 리스트
    protected Dictionary<int, Dictionary<ItemType, List<GameObject>>> itemListDic = new Dictionary<int, Dictionary<ItemType, List<GameObject>>>();

    // 구간별 활성화 된 장애물 리스트
    protected Dictionary<int, Dictionary<int, List<GameObject>>> obstacleListDic = new Dictionary<int, Dictionary<int, List<GameObject>>>();

    // 구간별 아이템 타입별 풀
    protected Dictionary<int, Dictionary<ItemType, Queue<GameObject>>> itemPoolDic = new Dictionary<int, Dictionary<ItemType, Queue<GameObject>>>();

    // 구간별 장애물 풀
    protected Dictionary<int, Dictionary<int, Queue<GameObject>>> obstaclePoolDic = new Dictionary<int, Dictionary<int, Queue<GameObject>>>();

    // 구간 리스트
    public List<GameObject> sectionList = new List<GameObject>();

    // 아이템 프리팹 딕셔너리
    protected Dictionary<ItemType, GameObject> prefabDic = new Dictionary<ItemType, GameObject>();

    void Start()
    {
        player = GameManager.instance.player;
        sectionX = sectionScale;
        playerController = player.GetComponent<PlayerController>();

        Initiate();
    }

    private void Update()
    {
        int sectionNum = ((int)sectionX / 100) % 2;
        currentSection = sectionX - (0.5f * sectionScale);

        if (player.transform.position.x >= sectionX - sectionScale + 20f)
        {
            if (playerController.ClearItemCount != 0)
            {
                Section(sectionNum);
            }
            else
                LastSection();
        }
        if (player.transform.position.x >= sectionX - 120f)
        {
            ObjectClear(sectionNum);
        }

    }

    protected void Initiate()
    {
        for (int i = 0; i < 2; i++)
        {
            itemListDic.Add(i, new Dictionary<ItemType, List<GameObject>>());
            obstacleListDic.Add(i, new Dictionary<int, List<GameObject>>());

            itemPoolDic.Add(i, new Dictionary<ItemType, Queue<GameObject>>());
            obstaclePoolDic.Add(i, new Dictionary<int, Queue<GameObject>>());

            for (int j = 0; j < 3; j++)
            {
                obstacleListDic[i].Add(j, new List<GameObject>());
                obstaclePoolDic[i].Add(j, new Queue<GameObject>());
            }

            for (ItemType type = ItemType.CLEARITEM; type <= ItemType.SKILLITEM; type++)
            {
                itemListDic[i].Add(type, new List<GameObject>());
                itemPoolDic[i].Add(type, new Queue<GameObject>());
            }
        }
        prefabDic.Add(ItemType.CLEARITEM, clearItemPrefab);
        prefabDic.Add(ItemType.SKILLITEM, skillItemPrefab);

        InitiateBackground();
    }

    protected void InitiateBackground()
    {
        for (int i = 0; i < 2; i++)
        {            
            sectionList[i].SetActive(false);

            InitiateObastcle(i);
            InitiateItem(ItemType.CLEARITEM, maxClearItem, i);
            InitiateItem(ItemType.SKILLITEM, maxSkillItem, i);
        }
    }

    protected void InitiateObastcle(int index)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                GameObject obj = Instantiate(obstacleList[i], Vector3.zero, Quaternion.identity);
                obj.transform.SetParent(sectionList[index].transform, true);
                obj.SetActive(false);
                obstaclePoolDic[index][i].Enqueue(obj);
            }
        }
    }

    protected void InitiateItem(ItemType type, int count, int index)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(prefabDic[type], Vector3.zero, Quaternion.identity);
            obj.transform.SetParent(sectionList[index].transform, true);
            obj.SetActive(false);
            itemPoolDic[index][type].Enqueue(obj);
        }
    }

    protected void Section(int sectionNum)
    {
        sectionList[sectionNum].transform.position = new Vector2(sectionX, 0f);
        sectionList[sectionNum].SetActive(true);

        SpawnSection(sectionNum);

        // 아이템 생성 후 다음 구간
        sectionX += sectionScale;
    }

    protected void LastSection()
    {
        endMap.transform.position = new Vector3(sectionX - (sectionScale / 4f), 0f);
        endMap.SetActive(true);
        if (GameManager.instance.gameClear)
        {
            endMap.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    protected GameObject SpawnObstacle(int sectionNum, int index)
    {
        if (obstaclePoolDic[sectionNum][index].Count <= 0)
        {
            index = (index + 1) % 3;
        }
        if (obstaclePoolDic[sectionNum][index].Count <= 0)
        {
            index = (index + 1) % 3;
        }

        GameObject obstacle = obstaclePoolDic[sectionNum][index].Dequeue();
        obstacle.SetActive(true);
        obstacleListDic[sectionNum][index].Add(obstacle);

        return obstacle;
    }

    protected GameObject SpawnItem(ItemType type, int sectionNum)
    {
        GameObject item = itemPoolDic[sectionNum][type].Dequeue();
        item.SetActive(true);
        itemListDic[sectionNum][type].Add(item);

        return item;
    }

    protected virtual void ItemPos(ItemType type, Transform obstacle, int sectionNum)
    {
        float itemPosX = Random.Range((currentSection + 1f), (currentSection + objSection - 2f));
        float itemPosY;

        if (obstacle.position.y >= 0f)
        {
            itemPosY = Random.Range(obstacle.position.y - 2.5f, -4f);
        }
        else
        {
            itemPosY = Random.Range(obstacle.position.y + 2.5f, 4f);
        }
        GameObject itemObj = SpawnItem(type, sectionNum);
        itemObj.transform.position = new Vector2(itemPosX, itemPosY);
    }

    protected virtual void SpawnSection(int sectionNum)
    {
        // 10~15개 장애물
        int obstacleCount = Random.Range(10, 16);
        int curClearCount = 0;
        int curSkillCount = 0;

        objSection = sectionScale / obstacleCount;
        for (int i = 0; i < obstacleCount; i++)
        {
            int obstacleIndex = Random.Range(0, 3);
            float obstaclePosX = Random.Range((currentSection + 3f), (currentSection + objSection - 3f));
            float obstaclePosY = Random.Range(-4f, 4f);
            GameObject obstacle = SpawnObstacle(sectionNum, obstacleIndex);
            obstacle.transform.position = new Vector2(obstaclePosX, obstaclePosY);

            int probability = RandomValue(curClearCount, maxClearItem);
            if (i % 2 == 0)
            {
                // 생성될 확률 : 15%
                if (probability <= 20)
                {
                    ItemPos(ItemType.CLEARITEM, obstacle.transform, sectionNum);
                    curClearCount++;
                }
                else
                {
                    probability = RandomValue(curSkillCount, maxSkillItem);

                    // 생성될 확률 : 25%
                    if (probability <= 25)
                    {
                        ItemPos(ItemType.SKILLITEM, obstacle.transform, sectionNum);
                        curSkillCount++;
                    }
                }
            }

            currentSection += objSection;
        }
    }

    protected void ObjectClear(int sectionNum)
    {
        for (int k = 0; k < 3; k++)
        {
            List<GameObject> objList = obstacleListDic[sectionNum][k];
            Queue<GameObject> objPool = obstaclePoolDic[sectionNum][k];

            for (int i = 0; i < objList.Count; i++)
            {
                // obj : 오브젝트 리스트에서  i번째에 있는 오브젝트
                GameObject obj = objList[i];
                int count = obj.transform.childCount;
                for (int j = 0; j < count; j++)
                {
                    if (obj.transform.GetChild(j).gameObject.activeSelf == false)
                    {
                        obj.transform.GetChild(j).gameObject.SetActive(true);
                    }
                }

                obj.SetActive(false);
                objPool.Enqueue(obj);
            }
            // 리스트 삭제
            objList.Clear();
        }
        for (ItemType type = ItemType.CLEARITEM; type <= ItemType.SKILLITEM; type++)
        {
            List<GameObject> objList = itemListDic[sectionNum][type];
            Queue<GameObject> objPool = itemPoolDic[sectionNum][type];

            for (int i = 0; i < objList.Count; i++)
            {
                // obj : 오브젝트 리스트에서  i번째에 있는 오브젝트
                GameObject obj = objList[i];

                obj.SetActive(false);
                objPool.Enqueue(obj);
            }
            // 리스트 삭제
            objList.Clear();
        }
    }

    protected int RandomValue(int currentCount, int maxCount)
    {
        int condition;

        if (currentCount >= maxCount)
        {
            condition = 100;
        }
        else
        {
            condition = Random.Range(0, 100);    // 확률
        }

        return condition;
    }
}