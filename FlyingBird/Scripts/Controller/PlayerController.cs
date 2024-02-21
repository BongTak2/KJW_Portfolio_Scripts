using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float weight;
    public int MaxHp { get; [SerializeField] private set; }

    [Header("skill")]
    [SerializeField] protected GameObject skill_2;
    public GameObject skill_4;
    [SerializeField] protected GameObject skillEffector;

    [Header("Audio")]
    [SerializeField] protected AudioClip[] skillAudios;
    [SerializeField] protected AudioClip[] effectAudios;
    [SerializeField] protected AudioClip[] gameAudios;

    private Animator effectAnimator;

    private float maxSpeed;
    private float currentRiseValue;
    private const float riseValue = 12f;
    private const int maxClearItem = 20;        // 클리어 아이템 개수


    private Animator animator;
    private Collider2D playerCollider;
    private Rigidbody2D rb;
    private Vector2 currentVelocity;

    [HideInInspector]
    public float CurrentSpeed { get; private set; }
    public int CurrentHp { get; private set; }
    [HideInInspector]
    public int ClearItemCount { get; private set; }
    [HideInInspector]
    public int skillItemCount;
    [HideInInspector]
    public int Score { get; private set; }

    [HideInInspector]
    public bool isMoving = false;
    private bool isCollision;
    private bool isPower;

    private bool soundPlay;

    // 딜레이 타임 확인용
    private float timeCheck;

    private void Start()
    {
        if (MaxHp > 5)
        {
            MaxHp = 5;
        }

        playerCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        CurrentHp = MaxHp;
        currentRiseValue = riseValue;
        skill_2 = Instantiate(skill_2);
        skill_4 = Instantiate(skill_4);
        skillEffector = Instantiate(skillEffector);
        effectAnimator = skillEffector.GetComponent<Animator>();
        CurrentSpeed = speed;
        maxSpeed = speed * 1.5f;

        ClearItemCount = maxClearItem;
        skillItemCount = 0;
    }

    void Update()
    {
        if (gameObject.transform.position.y <= -4.4f)
        {
            gameObject.transform.position = new Vector2(gameObject.transform.position.x, -4.4f);
        }
        if (gameObject.transform.position.y >= 4.4f)
        {
            gameObject.transform.position = new Vector2(gameObject.transform.position.x, 4.4f);
        }

        if (gameObject.transform.position.x < 22f)
        {
            rb.velocity = new Vector2(3f, 0f);
        }
        else
        {
            isMoving = true;
        }
        //----------------Debug---------------------------------
        if (Input.GetKeyDown(KeyCode.F1))
        {
            skillItemCount = 1;
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            skillItemCount = 2;
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            skillItemCount = 3;
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            skillItemCount = 4;
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            gameObject.transform.Translate(-10f, 0f, 0f);
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            ClearItemCount--;
        }
        if (Input.GetKeyDown(KeyCode.F7))
        {
            playerCollider.isTrigger = !playerCollider.isTrigger;
        }
        //--------------------------------------------------              

        if (isMoving)
        {
            skill_4.transform.position = transform.position;
            skillEffector.transform.position = transform.position;
            skillEffector.transform.GetChild(0).position = new Vector3(transform.position.x + 8f, 0f, 0f);
            skillEffector.transform.GetChild(1).position = new Vector3(transform.position.x, transform.position.y + 0.6f, 0f);

            if (GameManager.instance.gameOver)
            {
                rb.velocity = new Vector2(0f, 0f);
            }
            else
            {
                MovePlayer();

                if (Time.timeScale == 1)
                {
                    if (Input.GetKeyDown(KeySetting.keyValues[KeyAction.SKILL]))
                    {
                        Skill();
                    }
                }

                SpeedControl();
            }
        }
    }

    private void MovePlayer()
    {
        currentVelocity.x = CurrentSpeed;

        if (Input.GetKey(KeySetting.keyValues[KeyAction.RISE]))
        {
            currentVelocity.y = currentRiseValue - weight;
            animator.SetBool("Rise", true);
        }
        else
        {
            currentVelocity.y = -6f;
            animator.SetBool("Rise", false);
        }

        // 현재속도가 0보다 작을 때는 부스터 작동 X
        if (Input.GetKey(KeySetting.keyValues[KeyAction.BOOST]))
        {
            if (Time.timeScale == 1)
            {
                animator.SetBool("Boost", true);
            }
            if (soundPlay && Time.timeScale == 1)
            {
                SoundManager.instance.EffectSoundPlay(effectAudios[0]);
                soundPlay = false;
            }

            if (CurrentSpeed > 0f)
            {
                CurrentSpeed = maxSpeed;
            }
        }
        else
        {
            animator.SetBool("Boost", false);
            soundPlay = true;
        }

        if (isCollision)
        {
            isCollision = false;
            isPower = false;
        }

        rb.velocity = currentVelocity;

    }

    private void SpeedControl()
    {
        // 장애물 충돌
        if (CurrentSpeed < 0f)
        {
            animator.SetBool("Boost", false);
            CurrentSpeed += speed * 0.25f * Time.deltaTime;  // returntime = 4초 (0.25)

            if (CurrentSpeed > 0f)
            {
                CurrentSpeed = speed;
                GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                playerCollider.isTrigger = false;
                playerCollider.enabled = false;
                playerCollider.enabled = true;
            }
        }

        // 부스터
        if (CurrentSpeed > speed)
        {
            CurrentSpeed -= speed * 2f * Time.deltaTime;  // returntime = 0.5초 (2)
            if (CurrentSpeed < speed)
            {
                CurrentSpeed = speed;
            }
        }

        // 스킬 (1)
        if (currentRiseValue > riseValue)
        {
            // 스킬(1) 딜레이 타임 4초
            float delayTime = 4f;
            timeCheck += Time.deltaTime;
            if (timeCheck >= delayTime)
            {
                timeCheck = 0;
                currentRiseValue = riseValue;
                animator.SetBool("Skill", false);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.CompareTag("Obstacle"))
        {
            if (!isCollision)
            {
                collider.gameObject.SetActive(false);
                SoundManager.instance.EffectSoundPlay(effectAudios[1]);
                if (!isPower)
                {
                    StartCoroutine(FlickerPlayer());
                    if (CurrentHp <= 1)
                    {
                        // GameOver
                        CurrentHp = 0;
                        GameManager.instance.gameOver = true;
                    }
                    else
                    {
                        CurrentHp--;
                    }
                }
                else
                    skill_4.SetActive(false);
                CurrentSpeed = -3f;
                playerCollider.isTrigger = true;
                isCollision = true;
            }
        }

        // 게임 클리어 부분
        if (collider.gameObject.CompareTag("Portal"))
        {
            GameManager.instance.gameClear = true;
            SoundManager.instance.EffectSoundPlay(gameAudios[0]);
            SoundManager.instance.EffectSoundPlay(gameAudios[1]);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Clear_Item"))
        {
            if (ClearItemCount <= 0)
            {
                ClearItemCount = 0;
            }
            else
            {
                SoundManager.instance.EffectSoundPlay(effectAudios[3]);

                collision.gameObject.SetActive(false);
                effectAnimator.SetTrigger("Clear");
                ClearItemCount--;
                if (ClearItemCount == 0)
                {
                    SoundManager.instance.EffectSoundPlay(effectAudios[5]);
                }
            }
        }
        if (collision.gameObject.CompareTag("Skill_Item"))
        {
            if (skillItemCount >= 4 && skillItemCount < 5)
            {
                skillItemCount = 4;
            }
            else if (skillItemCount >= 5)
            {
                skillItemCount = 5;
            }
            else
            {
                collision.gameObject.SetActive(false);
                effectAnimator.SetTrigger("Skill");
                SoundManager.instance.EffectSoundPlay(effectAudios[2]);
                skillItemCount++;
            }
        }
        if (collision.gameObject.CompareTag("Score_Item"))
        {
            SoundManager.instance.EffectSoundPlay(effectAudios[3]);
            collision.gameObject.SetActive(false);
            effectAnimator.SetTrigger("Clear");
            Score += 100;
        }
        if (collision.gameObject.CompareTag("Time_Item"))
        {
            SoundManager.instance.EffectSoundPlay(effectAudios[4]);
            collision.gameObject.SetActive(false);
            effectAnimator.SetTrigger("Time");
            GameManager.instance.setTimer += 10f;
        }
    }

    private void Skill()
    {
        switch (skillItemCount)
        {
            case 1:
                // 일정 시간 가볍게 하기
                currentRiseValue = 16f;
                animator.SetBool("Skill", true);
                effectAnimator.SetTrigger("Skill_1");
                SoundManager.instance.EffectSoundPlay(skillAudios[0]);
                skillItemCount = 0;
                break;
            case 2:
                // 전방에 장애물 없애기
                skill_2.transform.position = new Vector2(transform.position.x + 7f, 0f);
                skill_2.SetActive(true);
                effectAnimator.SetTrigger("Skill_2");
                SoundManager.instance.EffectSoundPlay(skillAudios[1]);
                skillItemCount = 0;
                break;
            case 3:
                if (CurrentHp >= MaxHp)
                {
                    CurrentHp = MaxHp;
                    skillItemCount = 3;
                }
                else
                {
                    CurrentHp++;
                    effectAnimator.SetTrigger("Skill_3");
                    SoundManager.instance.EffectSoundPlay(skillAudios[2]);
                    skillItemCount = 0;
                }
                break;
            case 4:
                if (!isPower)
                {
                    // 무적
                    isPower = true;
                    effectAnimator.SetTrigger("Skill_4");
                    skill_4.SetActive(true);
                    SoundManager.instance.EffectSoundPlay(skillAudios[3]);
                    skillItemCount = 0;
                }
                else
                    skillItemCount = 4;
                break;
        }
    }

    private IEnumerator FlickerPlayer()
    {
        Color playerColor = new(1f, 1f, 1f, 1f);
        Color dark = new(0.7f, 0.7f, 0.7f, 0.8f);

        for (int i = 0; i <= 1; i++)
        {
            GetComponent<SpriteRenderer>().color = dark;
            yield return new WaitForSeconds(0.3f);
            GetComponent<SpriteRenderer>().color = playerColor;
            yield return new WaitForSeconds(0.3f);
        }
    }
}