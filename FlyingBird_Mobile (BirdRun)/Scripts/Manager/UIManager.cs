using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Player")]
    protected GameObject player;
    private PlayerController playerController;

    [Header("Skill Essential")]
    [SerializeField] protected Image[] skill;
    [SerializeField] protected HorizontalLayoutGroup layoutGroup;

    [Header("Text")]
    [SerializeField] protected TextMeshProUGUI clearText;
    [SerializeField] protected TextMeshProUGUI highScoreText;
    [SerializeField] protected TextMeshProUGUI distanceText;
    [SerializeField] protected TextMeshProUGUI timeText;
    [SerializeField] protected TextMeshProUGUI countText;
    [SerializeField] protected GameObject stopMenu;

    [Header("RectTransform")]
    [SerializeField] protected RectTransform upPos;

    [Header("Audio")]
    [SerializeField] protected AudioClip[] audioClips;

    private RectTransform scoreTextPos;

    private float distance;
    private int skillcount;
    [HideInInspector] public float Sec { get; protected set; }
    [HideInInspector] public int Min { get; protected set; }
    private int gameMode;
    private int highScore;
    private float infiniteScore;

    private int lastCount = 4;

    private const int maxSkill = 4;

    private void Start()
    {
        gameMode = ModeSelect.playMode;
        player = GameManager.instance.player;
        playerController = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (!playerController.IsMoving)
        {
            Count();
            distanceText.text = " ";
        }
        else
        {
            countText.gameObject.SetActive(false);
            if (upPos.anchoredPosition.y > 0) UImove();
            distanceText.text = $"<i>{(int)Distance()}</i> m";
        }

        distanceText.gameObject.SetActive(ToggleKey.moveDistanceBool);

        SkillBlank();

        switch (gameMode)
        {
            case 1:
                ClassicMode();
                break;
            case 2:
                InfiniteMode();
                break;
            case 3:
                TimeAttackMode();
                break;
        }


    }
    public float SetSecond(float value)
    {
        return Sec = value;
    }
    private void SkillBlank()
    {
        skillcount = playerController.skillItemCount - 1;

        for (int i = 0; i < maxSkill; i++)
        {
            RectTransform scale = skill[i].rectTransform.parent.GetComponent<RectTransform>();
            skill[i].color = new Color(0.8f, 0.8f, 0.8f, 0.5f);
            scale.localScale = new Vector3(1f, 1f, 1f);

            if (i == skillcount)
            {
                skill[i].color = new Color(1f, 1f, 1f, 1f);
                scale.localScale = new Vector3(1.3f, 1.3f, 1f);
            }
        }

        layoutGroup.enabled = false;
        layoutGroup.enabled = true;
    }

    public float Distance()
    {
        distance = player.transform.position.x - 20f;

        if (distance <= 0f)
        {
            distance = 0f;
        }

        return distance;
    }

    private void TimeCount()
    {
        if (Min >= 60)
        {
            Min = 59;
            Sec = 59;
        }
        else
        {
            if (!playerController.IsMoving)
                Sec = 0;
            else
                Sec += Time.deltaTime;

            if ((int)Sec > 59)
            {
                Sec = 0;
                Min++;
            }
        }

        if (playerController.IsMoving) timeText.text = string.Format("{0:D1}' {1:D2}\"", Min, (int)Sec);
        else { timeText.text = " "; }
    }

    private void ClassicMode()
    {
        if (!GameManager.instance.gameClear)
        {
            TimeCount();
        }

        clearText.text = $"{playerController.ClearItemCount} / 20";
    }

    private void InfiniteMode()
    {
        if (playerController.IsMoving)
        {
            if (playerController.CurrentSpeed > 0f)
            {
                infiniteScore += 1.5f * playerController.CurrentSpeed * Time.deltaTime;
            }
            else
            {
                infiniteScore += 0f;
            }
        }

        highScore = Score.infiniteScore[0] % ScoreBoard.standardNum;

        if (!GameManager.instance.gameOver)
        {
            GameManager.instance.score = (int)infiniteScore + playerController.Score; ;
        }

        if (GameManager.instance.score <= 0)
        {
            GameManager.instance.score = 0;
        }
        if (GameManager.instance.score >= 9999999)
        {
            GameManager.instance.score = 9999999;
        }

        scoreTextPos = clearText.rectTransform;
        if (GameManager.instance.score < 1000)
        {
            scoreTextPos.anchoredPosition = new Vector2(-400f, -100f);
        }
        else if (GameManager.instance.score < 10000)
        {
            scoreTextPos.anchoredPosition = new Vector2(-450f, -100f);
        }
        else if (GameManager.instance.score < 100000)
        {
            scoreTextPos.anchoredPosition = new Vector2(-500f, 100f);
        }
        else if (GameManager.instance.score < 1000000)
        {
            scoreTextPos.anchoredPosition = new Vector2(-550f, 100f);
        }
        else if (GameManager.instance.score < 10000000)
        {
            scoreTextPos.anchoredPosition = new Vector2(-600f, 100f);
        }

        TimeCount();

        if (GameManager.instance.score < highScore)
        {
            highScoreText.enabled = true;
            highScoreText.text = $"최고 점수 : {highScore} ";
        }
        else
        {
            highScoreText.enabled = false;
        }

        clearText.text = $"점수 : {GameManager.instance.score} ";
    }

    private void TimeAttackMode()
    {
        if (GameManager.instance.setTimer > 0)
        {
            if (playerController.IsMoving)
                GameManager.instance.setTimer -= Time.deltaTime;
        }
        else
        {
            GameManager.instance.setTimer = 0;
            GameManager.instance.gameOver = true;
        }
        int min = (int)GameManager.instance.setTimer / 60;
        int sec = (int)GameManager.instance.setTimer - (60 * min);

        highScore = Score.timeAttackScore[0] % ScoreBoard.standardNum;

        GameManager.instance.score = (int)Distance();

        if (GameManager.instance.score <= 0)
        {
            GameManager.instance.score = 0;
        }

        if (GameManager.instance.score >= 9999999)
        {
            GameManager.instance.score = 9999999;
        }

        if (GameManager.instance.score < highScore)
        {
            highScoreText.enabled = true;
            highScoreText.text = $"최고 점수 : {highScore} m";
        }
        else
        {
            highScoreText.enabled = false;
        }

        timeText.text = string.Format("{0:D1}' {1:D2}\"", min, sec);
    }

    private void UImove()
    {
        float upY = upPos.anchoredPosition.y;

        if (upPos.anchoredPosition.y > 0)
            upY -= 10f;
        else
            upY = 0f;

        upPos.anchoredPosition = new Vector3(0f, upY);
    }

    private void Count()
    {
        float leftTime = (19 - player.transform.position.x) / 4;

        int currentCount = Mathf.CeilToInt(leftTime);
        if (currentCount > 0)
        {
            if (currentCount != lastCount)
            {
                switch (currentCount)
                {
                    case 3:
                        SoundManager.instance.EffectSoundPlay(audioClips[0]);
                        break;
                    case 2:
                        SoundManager.instance.EffectSoundPlay(audioClips[1]);
                        break;
                    case 1:
                        SoundManager.instance.EffectSoundPlay(audioClips[2]);
                        break;
                }
                // SoundManager.instance.EffectSoundPlay(audioClips[0]);
            }
            if (currentCount > 3) countText.text = " ";
            else countText.text = currentCount.ToString();
        }
        else
        {
            if (currentCount != lastCount)
            {
                SoundManager.instance.EffectSoundPlay(audioClips[3]);
            }
            countText.text = "GO!!";
        }
        lastCount = currentCount;
    }

    public void OnClickStop_btn()
    {
        Time.timeScale = 0;
        stopMenu.SetActive(true);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause) OnClickStop_btn();
    }
}
