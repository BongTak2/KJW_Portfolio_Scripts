using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : Controller
{
    private RaycastHit hit;
    private Transform _cameraTransform;
    private Transform CameraTransform
    {
        get
        {
            if (_cameraTransform)
            {
                return _cameraTransform;
            }
            else
            {
                Camera temp = Camera.main;

                if (temp)
                {
                    _cameraTransform = temp.transform;
                }
                else
                {
                    temp = new GameObject("MainCamera").AddComponent<Camera>();
                    temp.tag = "MainCamera";
                    temp.gameObject.AddComponent<AudioListener>();

                    _cameraTransform = temp.transform;
                }

                return _cameraTransform;
            }
        }
    }

    Vector3 cameraOffset = new Vector3(0.15f, 11f, -7.7f);
    float cameraMoveSpeed;
    bool isCameraFollowed;
    bool isGamePlay = true;
    bool ready_A_attack = false;

    private void Update()
    {
        InputUpdate();
        if (Input.GetKeyDown(KeyCode.F11))
        {
            debugMode = !debugMode;
        }
    }

    private void Start()
    {
        controlledCharacter = SpawnManager.player.GetComponent<Character>();
        cameraMoveSpeed = 1.6f;
        CameraTransform.position = controlledCharacter.transform.position + cameraOffset;
    }

    public void InputUpdate()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(mouseRay, out hit, 1000f, -1, QueryTriggerInteraction.Ignore); // -1 : 모두 1 (이진법)
        //Debug.DrawRay(transform.position, (hit.point - transform.position) * 1000f, Color.red);

        if (hit.collider != null)
            sightPos = hit.point;
        else
            sightPos = CameraTransform.position + (CameraTransform.forward * 100.0f);

        //Vector3 temp = CameraTransform.position;
        //Mathf.Clamp(temp.x, -51f, 51f);
        //Mathf.Clamp(temp.z, -70f, 45f);
        //CameraTransform.position = temp;

        if (isCameraFollowed)
            FollowCamera();
        else
            CameraUpdate();

        if (controlledCharacter.GetState() != State.Die)
        {
            ControlUpdate();
        }
    }

    public void ControlUpdate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (hit.collider.TryGetComponent(out Unit target))
            {
                controlledCharacter.SetCommand(new Command_Attack(controlledCharacter, target));
            }
            else
            {
                controlledCharacter.SetCommand(new Command_Move(controlledCharacter, sightPos));
            }
        }

        GetAtkRange(Input.GetKey(KeyCode.A));

        if (Input.GetKeyDown(KeyCode.A))
        {
            ready_A_attack = true;
        }
        if (ready_A_attack)
        {
            if (Input.GetMouseButtonDown(0))
            {
                controlledCharacter.SetCommand(new Command_A_Move(controlledCharacter, sightPos));
                ready_A_attack = false;
            }
            else if (Input.GetMouseButtonDown(1))
            {
                ready_A_attack = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.W) && !Input.GetKey(KeyCode.LeftControl))
        {
            //controlledCharacter.delayTimeFunc -= controlledCharacter.WeaponChange;
            //controlledCharacter.delayTimeFunc += controlledCharacter.WeaponChange;
            controlledCharacter.StartCoolTime(ref controlledCharacter.weaponChange);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            ControlStop();
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            isCameraFollowed = !isCameraFollowed;
        }

        if (Input.GetKeyDown(KeyCode.Q) && TimeManager.instance.Skill_Q_Ready && !Input.GetKey(KeyCode.LeftControl))
        {
            if (controlledCharacter.CheckCurrentWeapon(WeaponType.Gravitum))
            {
                if (Gravitum.gravitumMarkUnits.Count > 0)
                {
                    controlledCharacter.SetCommand(new Command_Skill(controlledCharacter, hit));
                }
                else return;
            }
            else
            {
                controlledCharacter.SetCommand(new Command_Skill(controlledCharacter, hit));
            }
            //controlledCharacter.SetSkill(hit);
        }

        if (Input.GetKeyDown(KeyCode.R) && TimeManager.instance.UltReady)
        {
            controlledCharacter.SetCommand(new Command_Ultimate(controlledCharacter, hit));
        }

        if (Input.GetKeyDown(KeyCode.D) && TimeManager.instance.Spell_Heal_Ready)
        {
            controlledCharacter.TakeHeal(200f);
            TimeManager.instance.spell_Heal_CoolReady = true;
        }

        if (Input.GetKeyDown(KeyCode.F) && TimeManager.instance.Spell_Flash_Ready)
        {
            controlledCharacter.gameObject.transform.position = (controlledCharacter.gameObject.transform.position + (((hit.point - controlledCharacter.gameObject.transform.position).Y_VectorToZero().normalized) * 5f));
            controlledCharacter.SetMoveStop();
            TimeManager.instance.spell_Flash_CoolReady = true;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                if (botCount >= 5)
                {
                    PoolManager.Destroy(botList[0]);
                    botList.RemoveAt(0);
                    bot = PoolManager.Instantiate(PrefabType.Prefabs__Object__TrainingBot);
                    bot.GetComponent<NavMeshAgent>().enabled =false;
                    botList.Add(bot);
                    bot.transform.position = hit.point;
                    bot.GetComponent<NavMeshAgent>().enabled = true;
                }
                else
                {
                    bot = PoolManager.Instantiate(PrefabType.Prefabs__Object__TrainingBot);
                    bot.GetComponent<NavMeshAgent>().enabled = false;
                    botList.Add(bot);
                    bot.transform.position = hit.point;
                    bot.GetComponent<NavMeshAgent>().enabled = true;

                    botCount++;
                }
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                for (int i = 0; i < botCount; i++)
                {
                    PoolManager.Destroy(botList[i]);
                }
                botList.Clear();
                botCount = 0;
            }
        }
    }
    int botCount;
    GameObject bot;
    List<GameObject> botList = new List<GameObject>();
    public void FollowCamera()
    {
        if (controlledCharacter)
        {
            CameraTransform.position = controlledCharacter.transform.position + cameraOffset;
        }
        else
        {
            CameraTransform.position = cameraOffset;
        }
    }
    bool debugMode;
    public void CameraUpdate()
    {
        Vector3 cameraMoveVector = CameraMove();

        if (isGamePlay)
        {
            CameraZoom();

            if (Input.GetKey(KeyCode.Space))
            {
                if (controlledCharacter)
                {
                    CameraTransform.position = controlledCharacter.transform.position + cameraOffset;
                }
                else
                {
                    CameraTransform.position = cameraOffset;
                }
            }

            Cursor.lockState = CursorLockMode.Confined;

            Vector3 movedCameraPosition = CameraTransform.position + (20 * Time.deltaTime * cameraMoveVector);

            if (!debugMode)
            {
                movedCameraPosition.x = Mathf.Clamp(movedCameraPosition.x, -56f, 56f);
                movedCameraPosition.z = Mathf.Clamp(movedCameraPosition.z, -72f, 48f);
            }

            CameraTransform.position = movedCameraPosition;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            CameraTransform.position = controlledCharacter.transform.position + cameraOffset;
        }

    }

    private Vector3 CameraMove()
    {
        Vector3 cursorPos = Input.mousePosition;
        Vector3 cameraMoveVector = Vector3.zero;


        if (cursorPos.x < 5 || Input.GetKey(KeyCode.LeftArrow))
        {
            cameraMoveVector += (Vector3.left);
        }
        if (cursorPos.x > Screen.width - 5 || Input.GetKey(KeyCode.RightArrow))
        {
            cameraMoveVector += (Vector3.right);
        }
        if (cursorPos.y < 5 || Input.GetKey(KeyCode.DownArrow))
        {
            cameraMoveVector += (Vector3.back);
        }
        if (cursorPos.y > Screen.height - 5 || Input.GetKey(KeyCode.UpArrow))
        {
            cameraMoveVector += (Vector3.forward);
        }
        cameraMoveVector *= cameraMoveSpeed;
        return cameraMoveVector;
    }

    private void CameraZoom()
    {
        float zoomSpeed = 3f;
        float mouseScroll = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        float cameraSize = Camera.main.orthographicSize;

        float minSize = 4f;
        float maxSize = 10f;

        cameraSize -= mouseScroll;

        if (cameraSize < minSize)
        {
            cameraSize = minSize;
        }
        else if (cameraSize > maxSize)
        {
            cameraSize = maxSize;
        }

        Camera.main.orthographicSize = cameraSize;
    }

}
