using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region GameManager 싱글톤
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    #endregion

    [Header("Section")]
    [SerializeField] protected GameObject mapGenerator;

    [Header("UI")]
    [SerializeField] protected UIManager uiController;
    [SerializeField] protected GameObject canvasUI;
    [SerializeField] protected GameObject gameOverUI;
    [SerializeField] protected GameObject gameClearUI;
    [SerializeField] protected TextMeshProUGUI gameOverText;
    [SerializeField] protected TextMeshProUGUI gameClearText;
    [SerializeField] protected GameObject save;
    [SerializeField] protected GameObject actionButton;

    [Header("Audio")]
    [SerializeField] protected AudioClip[] audioClips;

    public GameObject player;
    public int score;
    public float setTimer;
    public bool gameOver;
    public bool gameClear;
    public Dictionary<ButtonType, bool> inputKey = new Dictionary<ButtonType, bool>();

    private const float setTime = 120f;      // 타임어택 초;
    private bool storable;

    void OnEnable()
    {
        setTimer = setTime;

        if (PlayerSelect.startPlayer != null)
        {
            player = Instantiate(PlayerSelect.startPlayer);
            player.transform.position = new Vector3(5f, 0f, 0f);
        }
        else
        {
            SceneManager.LoadScene(0);
        }

        inputKey.Add(ButtonType.Rise, false);
        inputKey.Add(ButtonType.Boost, false);
        inputKey.Add(ButtonType.Skill, false);
    }

    private void Start()
    {
        SoundManager.instance.BGMPlay(audioClips[0]);
        SoundManager.instance.bgSound.Pause();
    }

    void Update()
    {
        if (gameOver)
        {
            Invoke("GameOver", 0.8f);
        }

        if (Time.timeScale == 1)
        {
            if (player.GetComponent<PlayerController>().IsMoving)
            {
                SoundManager.instance.bgSound.UnPause();
            }
        }
        else
        {
            SoundManager.instance.bgSound.Pause();
        }

        if (gameClear)
        {
            GameClear();
        }
    }

    private void GameOver()
    {
        Time.timeScale = 0;
        SoundManager.instance.BGMStop();
        SoundManager.instance.EffectSoundPlay(audioClips[1]);
        mapGenerator.SetActive(false);
        canvasUI.SetActive(false);
        player.SetActive(false);
        actionButton.SetActive(false);
        gameOverUI.SetActive(true);
        player.GetComponent<PlayerController>().skill_4.SetActive(false);

        switch (ModeSelect.playMode)
        {
            case 1:
                gameOverText.text = $"남은 개수 : {player.GetComponent<PlayerController>().ClearItemCount} 개";
                break;
            case 2:
                gameOverText.text = "Score : " + score;
                if (Score.infiniteScore[Score.rankingLength - 1] % ScoreBoard.standardNum < score)
                {
                    storable = true;
                    save.SetActive(true);
                }
                break;
            case 3:
                gameOverText.text = $"Score : {score} m";
                if (Score.timeAttackScore[Score.rankingLength - 1] % ScoreBoard.standardNum < score)
                {
                    storable = true;
                    save.SetActive(true);
                }
                break;
        }
    }

    public void OnClickMainMenu_btn()
    {
        setTimer = setTime;
        Time.timeScale = 1;
        gameOver = false;
        gameClear = false;

        SceneManager.LoadScene(0);
    }

    public void OnClickReGame_btn()
    {
        setTimer = setTime;
        Time.timeScale = 1;
        gameOver = false;
        gameClear = false;

        SceneManager.LoadScene(ModeSelect.playMode);
    }

    public void GameClear()
    {
        int min = uiController.Min;
        int sec = (int)uiController.Sec;
        int distance = (int)uiController.Distance();
        mapGenerator.SetActive(false);
        canvasUI.SetActive(false);
        gameClearUI.SetActive(true);
        actionButton.SetActive(false);
        gameClearText.text = string.Format("클리어 시간  :  {0:D1}' {1:D2}\"\n이동한 거리  : {2}m", min, sec, distance);
    }

    public void OnClickSave_btn(int mode)
    {
        if (mode == 2)
        {
            DataSave(Score.infiniteScore, Score.infiniteScoreCharacterMapping, score);
        }
        else if (mode == 3)
        {
            DataSave(Score.timeAttackScore, Score.timeAttackScoreCharacterMapping, score);
        }

        MainMenu.Save();

        storable = false;
    }

    private void DataSave(List<int> scoreList, Dictionary<int, int> mapping, int score)
    {
        if (storable)
        {
            int characterNum = PlayerSelect.characterNum * ScoreBoard.standardNum;

            if (mapping.ContainsKey(score + characterNum))
            {
                mapping.Add(score + characterNum, PlayerSelect.characterNum);
            }

            scoreList[Score.rankingLength - 1] = score + characterNum;

            ListSort(scoreList);
        }
    }

    private void ListSort(List<int> scoreList)
    {
        for (int i = scoreList.Count - 1; i > 0; i--)
        {
            int temp;
            if (scoreList[i] % ScoreBoard.standardNum > scoreList[i - 1] % ScoreBoard.standardNum)
            {
                temp = scoreList[i];
                scoreList[i] = scoreList[i - 1];
                scoreList[i - 1] = temp;
            }
        }
    }
}
