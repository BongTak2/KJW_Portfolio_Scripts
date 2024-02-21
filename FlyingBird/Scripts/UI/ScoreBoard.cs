using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    private Dictionary<int, List<int>> scoreList = new Dictionary<int, List<int>>();

    private Dictionary<int, Image[]> characterImageDic = new Dictionary<int, Image[]>();

    private Dictionary<int, TextMeshProUGUI[]> scoreText = new Dictionary<int, TextMeshProUGUI[]>();

    private Dictionary<int, Dictionary<int, int>> characterScoreMapping = new Dictionary<int, Dictionary<int, int>>();


    [Header("Infinite Score")]
    [SerializeField] protected GameObject i_leaderBoard;
    [SerializeField] protected TextMeshProUGUI[] i_Score;
    [SerializeField] protected Image[] i_character;

    [Header("TimeAttack Score")]
    [SerializeField] protected GameObject t_leaderBoard;
    [SerializeField] protected TextMeshProUGUI[] t_Score;
    [SerializeField] protected Image[] t_character;

    [Header("CharacterImage")]
    [SerializeField] protected Sprite[] characterImage;

    public static int standardNum = 100000000;

    private void Awake()
    {
        scoreList.Add(0, Score.infiniteScore);
        scoreList.Add(1, Score.timeAttackScore);

        characterScoreMapping.Add(0, Score.infiniteScoreCharacterMapping);
        characterScoreMapping.Add(1, Score.timeAttackScoreCharacterMapping);

        characterImageDic.Add(0, i_character);
        characterImageDic.Add(1, t_character);

        scoreText.Add(0, i_Score);
        scoreText.Add(1, t_Score);
    }

    void Update()
    {
        for (int num = 0; num < 2; num++)
        {
            //scoreList[num].Sort((a, b) => b.CompareTo(a));
            //GameManager.ListSort(scoreList[num]);

            ScoreShow(num);
        }
    }

    public void OnClickInfiniteMode()
    {
        i_leaderBoard.SetActive(true);
        t_leaderBoard.SetActive(false);
    }

    public void OnClickTimeAttackMode()
    {
        i_leaderBoard.SetActive(false);
        t_leaderBoard.SetActive(true);
    }

    public void OnClickReset(int num)
    {
        for (int i = 0; i < Score.rankingLength; i++)
        {
            scoreList[num][i] = 0;
            characterImageDic[num][i].sprite = characterImage[0];
            characterImageDic[num][i].color = new Color(1f, 1f, 1f, 0f);
        }

        MainMenu.Save();
    }

    private void ScoreShow(int modeNum)
    {
        for (int i = 0; i < Score.rankingLength; i++)
        {
            if (modeNum == 0)
            {
                scoreText[modeNum][i].text = $"{scoreList[modeNum][i] % standardNum} Á¡";
            }
            else
            {
                scoreText[modeNum][i].text = $"{scoreList[modeNum][i] % standardNum} m";
            }

            if (scoreList[modeNum][i] != 0)
            {
                characterImageDic[modeNum][i].sprite = characterImage[scoreList[modeNum][i] / standardNum];
                characterImageDic[modeNum][i].color = new Color(1f, 1f, 1f, 1f);
            }
        }
    }
}
