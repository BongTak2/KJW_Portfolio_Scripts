using UnityEngine;

public class InfiniteMap : MapGenerator
{
    private const float phase1 = 1500f;     
    private const float phase2 = 3000f;     
    private const float phase3 = 4500f;       
    private int phaseNum;

    private void Awake()
    {
        maxClearItem = 1;
    }

    private void Update()
    {
        int sectionNum = ((int)sectionX / 100) % 2;
        currentSection = sectionX - (sectionScale / 2f);

        if (player.transform.position.x >= sectionX - sectionScale + 20f)
        {
            if (playerController.ClearItemCount != 0)
            {
                Section(sectionNum);
            }
        }
        if (player.transform.position.x >= sectionX - 120f)
        {
            ObjectClear(sectionNum);
        }

        Phase();
    }

    private void Phase()
    {
        if (player.transform.position.x <= phase1)
        {
            phaseNum = 0;
        }
        else if (player.transform.position.x > phase1 && player.transform.position.x <= phase2)
        {
            phaseNum = 1;
        }
        else if (player.transform.position.x > phase2 && player.transform.position.x <= phase3)
        {
            phaseNum = 2;
        }
        else
        {
            phaseNum = 3;
        }
    }

    protected override void SpawnSection(int sectionNum)
    {
        // 10~15개 장애물
        int obstacleCount = phaseNum switch
        {
            0 => Random.Range(10, 15),
            1 => Random.Range(15, 20),
            2 => Random.Range(20, 25),
            _ => 25,
        };

        int curSkillCount = 0;

        objSection = sectionScale / obstacleCount;
        for (int i = 0; i < obstacleCount; i++)
        {
            int obstacleIndex = Random.Range(0, 3);
            float obstaclePosX = currentSection + (0.5f * objSection);
            float obstaclePosY = Random.Range(-4f, 4f);
            GameObject obstacle = SpawnObstacle(sectionNum, obstacleIndex);
            obstacle.transform.position = new Vector2(obstaclePosX, obstaclePosY);

            if (i == obstacleCount - 1)
            {
                ItemPos(ItemType.CLEARITEM, obstacle.transform, sectionNum);
            }
            else
            {
                if (i % 2 == 0)
                {
                    int probability = RandomValue(curSkillCount, maxSkillItem);
                    int condition = phaseNum switch
                    {
                        0 => 40,
                        1 => 50,
                        _ => 60,
                    };
                    // 생성될 확률
                    if (probability <= condition)
                    {
                        ItemPos(ItemType.SKILLITEM, obstacle.transform, sectionNum);
                        curSkillCount++;
                    }
                }
            }
            currentSection += objSection;
        }
    }

    protected override void ItemPos(ItemType type, Transform obstacle, int sectionNum)
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
        if (type == ItemType.CLEARITEM)
        {
            itemObj.tag = "Score_Item";
            itemObj.GetComponent<SpriteRenderer>().color = new Color(1f, 0.8f, 0f);
        }
        itemObj.transform.position = new Vector2(itemPosX, itemPosY);
    }
}