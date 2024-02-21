using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Manager
{
    public GameManager instance;

    public List<Manager> managerList = new List<Manager>();

    public GameObject panel;
    public GameObject statUI;
    private void Awake()
    {
        Initialize();
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!panel.activeSelf)
                Time.timeScale = 0;
            else
                Time.timeScale = 1;
            panel.SetActive(!panel.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.F8))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 5;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }

    public void OnClickQuit_btn()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
    }

    public void OnClickExit_Btn()
    {
        SceneManager.LoadScene(0);
    }

    public void OnClickResume_btn()
    {
        if (panel.activeSelf)
        {
            panel.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public override void Initialize()
    {
        base.Initialize();
        this.Singleton(ref instance);

        AddManager(gameObject.GetOrAddComponent<ResourceManager>());
        AddManager(gameObject.GetOrAddComponent<TimeManager>());
        AddManager(gameObject.GetOrAddComponent<PoolManager>());
        AddManager(gameObject.GetOrAddComponent<SpawnManager>());
        AddManager(gameObject.GetOrAddComponent<ControllerManager>());
        AddManager(gameObject.GetOrAddComponent<UIManager>());

        statUI.GetComponent<UI_CharacterStatUI>().Initialize();

    }

    public void AddManager(Manager targetManager)
    {
        targetManager.Initialize();
        if (managerList.Contains(targetManager) == false)
            managerList.Add(targetManager);
    }
}
