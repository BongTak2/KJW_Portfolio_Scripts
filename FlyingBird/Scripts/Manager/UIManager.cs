using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Player")]
    protected GameObject player;
    private PlayerController playerController;

    [Header("Hp Essential")]
    [SerializeField] protected Image[] heart;
    [SerializeField] protected Sprite back, front;

    [Header("Skill Essential")]
    [SerializeField] protected Image[] skill;
    [SerializeField] protected HorizontalLayoutGroup layoutGroup;

    [Header("Text")]
    [SerializeField] protected TextMeshProUGUI clearText;
    [SerializeField] protected TextMeshProUGUI highScoreText;
    [SerializeField] protected TextMeshProUGUI distanceText;
    [SerializeField] protected TextMeshProUGUI timeText;
    [SerializeField] protected TextMeshProUGUI countText;

    [Header("RectTransform")]
    [SerializeField] protected RectTransform upPos;
    [SerializeField] protected RectTransform downPos;

    [Header("Audio")]
    [SerializeField] protected AudioClip[] audioClips;

    private RectTransform scoreTextPos;

    private int hp;
    private float distance;
    private int skillcount;
    [HideInInspector]
    public float sec;
    [HideInInspector]
    public int min;
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
        if (!playerController.isMoving)
        {
            Count();
        }
        else
        {
            countText.gameObject.SetActive(false);
        }

        HeartBlank();

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

        distanceText.text = $"{(int)Distance()} m";

        if (playerController.isMoving)
            UImove();
    }

    private void HeartBlank()
    {
        hp = playerController.CurrentHp;

        for (int i = 0; i < playerController.MaxHp; i++)
        {
            heart[i].enabled = true;
            heart[i].sprite = back;
            if (hp > i)
            {
                heart[i].sprite = front;
            }
        }
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
                scale.localScale = new Vector3(1.5f, 1.5f, 1f);
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
        if (min >= 60)
        {
            min = 59;
            sec = 59;
        }
        else
        {
            if (!playerController.isMoving)
                sec = 0;
            else
                sec += Time.deltaTime;

            if ((int)sec > 59)
            {
                sec = 0;
                min++;
            }
        }

        timeText.text = string.Format("{0:D1}' {1:D2}\"", min, (int)sec);
    }

    private void ClassicMode()
    {
        if (!GameManager.instance.gameClear)
        {
            TimeCount();
        }

        clearText.text = $"남은 개수 :  {playerController.ClearItemCount}";
    }

    private void InfiniteMode()
    {
        if (playerController.isMoving)
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
            scoreTextPos.anchoredPosition = new Vector2(500f, 370f);
        }
        else if (GameManager.instance.score < 10000)
        {
            scoreTextPos.anchoredPosition = new Vector2(450f, 370f);
        }
        else if (GameManager.instance.score < 100000)
        {
            scoreTextPos.anchoredPosition = new Vector2(400f, 370f);
        }
        else if (GameManager.instance.score < 1000000)
        {
            scoreTextPos.anchoredPosition = new Vector2(350f, 370f);
        }
        else if (GameManager.instance.score < 10000000)
        {
            scoreTextPos.anchoredPosition = new Vector2(300f, 370f);
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
            if (playerController.isMoving)
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
        float downY = downPos.anchoredPosition.y;

        if (upPos.anchoredPosition.y > 0)
            upY -= 10f;
        else
            upY = 0f;

        if (downPos.anchoredPosition.y < 0)
            downY += 10f;
        else
            downY = 0f;

        upPos.anchoredPosition = new Vector3(0f, upY);
        downPos.anchoredPosition = new Vector3(0f, downY);
    }

    private void Count()
    {
        float leftTime = (19 - player.transform.position.x) / 4;
        
        int currentCount = Mathf.CeilToInt(leftTime);
        if (currentCount > 0)
        {
            if(currentCount != lastCount)
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
            countText.text = currentCount.ToString();
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
}
