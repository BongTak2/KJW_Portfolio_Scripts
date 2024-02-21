using UnityEngine;
using UnityEngine.SceneManagement;

public class SubMenu : MonoBehaviour
{
    [SerializeField] protected GameObject settingMenu;

    public void OnClickResume_btn()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    public void OnClickReGame_btn()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(ModeSelect.playMode);
    }

    public void OnClickSetting_btn()
    {
        settingMenu.SetActive(true);
    }

    public void OnClickQuit_btn()
    {
        SceneManager.LoadScene(0);
        SoundManager.instance.BGMStop();
        ModeSelect.playMode = 0;
        Time.timeScale = 1;
    }
}
