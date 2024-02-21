using UnityEngine;

public class TimeAttackMap : MapGenerator
{
    private void Awake()
    {
        maxClearItem = 1;
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
        }

        if (player.transform.position.x >= sectionX - 120f)
        {
            ObjectClear(sectionNum);
        }
    }

    protected override void SpawnSection(int sectionNum)
    {
        // 10~15개 장애물
        int obstacleCount = Random.Range(10, 16);

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
                if (sectionNum == 0)
                    ItemPos(ItemType.CLEARITEM, obstacle.transform, sectionNum);
            }
            else
            {
                if (i % 2 == 0)
                {
                    int probability = RandomValue(curSkillCount, maxSkillItem);

                    // 생성될 확률
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
}
