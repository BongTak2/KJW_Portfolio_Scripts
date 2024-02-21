using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] protected GameObject windowTab;
    [SerializeField] protected GameObject settingTab;
    [SerializeField] protected GameObject startButtons;
    [SerializeField] protected GameObject selectCharacter;
    [SerializeField] protected GameObject selectMode;
    [SerializeField] protected GameObject status;
    [SerializeField] protected GameObject side;
    [SerializeField] protected GameObject leaderBoard;
    [SerializeField] protected AudioClip mainBGM;

    protected static SaveData saveData;

    private void Start()
    {
        Load();
        SoundManager.instance.BGMPlay(mainBGM);
    }

    public void OnclickBack_btn()
    {
        windowTab.SetActive(false);
        selectMode.SetActive(true);
        startButtons.SetActive(true);
        status.SetActive(false);
        side.SetActive(false);
        selectCharacter.SetActive(false);
        settingTab.SetActive(false);
        leaderBoard.SetActive(false);

        Save();
    }

    public void SelectWindow()
    {
        selectMode.SetActive(false);
        selectCharacter.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnclickBack_btn();
        }
    }

    public void OnClickStart_btn()
    {
        startButtons.SetActive(false);
        windowTab.SetActive(true);
        Save();
    }

    public void OnClickSetting_btn()
    {
        startButtons.SetActive(false);
        settingTab.SetActive(true);
    }
    public void OnClickRanking_btn()
    {
        startButtons.SetActive(false);
        leaderBoard.SetActive(true);
    }

    public void OnClickQuit_btn()
    {
        Save();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif 
    }

    public static void Save()
    {
        for (int i = 0; i < Score.rankingLength; i++)
        {
            saveData.infiniteModeScore[i] = Score.infiniteScore[i];
            saveData.timeAttackModeScore[i] = Score.timeAttackScore[i];
        }

        saveData.bgmVolume = Sound.bgmVolume;
        saveData.sfxVolume = Sound.sfxVolume;

        saveData.tempbgmVolume = Sound.tempbgmVolume;
        saveData.tempsfxVolume = Sound.tempsfxVolume;

        saveData.moveDistance = ToggleKey.moveDistanceBool;
        saveData.buttonChange = ToggleKey.buttonChangeBool;

        SaveManager.Save(saveData);
    }

    private void Load()
    {
        saveData = SaveManager.Load();

        if (Score.infiniteScore.Count == 0)
        {
            for (int i = 0; i < Score.rankingLength; i++)
            {
                Score.infiniteScore.Add(saveData.infiniteModeScore[i]);
            }
        }

        if (Score.timeAttackScore.Count == 0)
        {
            for (int i = 0; i < Score.rankingLength; i++)
            {
                Score.timeAttackScore.Add(saveData.timeAttackModeScore[i]);
            }
        }
        Sound.bgmVolume = saveData.bgmVolume;
        Sound.sfxVolume = saveData.sfxVolume;

        Sound.tempbgmVolume = saveData.tempbgmVolume;
        Sound.tempsfxVolume = saveData.tempsfxVolume;


        ToggleKey.moveDistanceBool = saveData.moveDistance;
        ToggleKey.buttonChangeBool = saveData.buttonChange;
    }
}
