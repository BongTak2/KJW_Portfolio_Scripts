using UnityEngine;
using UnityEngine.SceneManagement;

public class SubMenu : MonoBehaviour
{
    [SerializeField] protected GameObject settingMenu;

    public void OnClickResume_btn()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
        GameManager.instance.isMenuActive = false;
    }

    public void OnClickSetting_btn()
    {
        settingMenu.SetActive(true);
    }

    public void OnClickQuit_btn()
    {
        SceneManager.LoadScene(0);
        SoundManager.instance.BGMStop();
        GameManager.instance.isMenuActive = false;
        ModeSelect.playMode = 0;
        Time.timeScale = 1;
    }
}
