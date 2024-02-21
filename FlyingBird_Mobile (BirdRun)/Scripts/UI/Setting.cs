using UnityEngine;
using UnityEngine.UI;

public enum KeyAction
{
    RISE,
    BOOST,
    SKILL
}

public class Setting : MonoBehaviour
{

    #region Setting ΩÃ±€≈Ê
    public static Setting instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    #endregion

    [SerializeField] protected Slider bgmSlider;
    [SerializeField] protected Slider sfxSlider;

    [SerializeField] protected Sprite[] muteSprite;
    [SerializeField] protected Image[] muteIcon;

    [SerializeField] protected Toggle moveDistanceToggle;
    [SerializeField] protected Toggle buttonChangeToggle;

    private void Start()
    {
        bgmSlider.value = Sound.bgmVolume;
        sfxSlider.value = Sound.sfxVolume;

        moveDistanceToggle.isOn = ToggleKey.moveDistanceBool;
        buttonChangeToggle.isOn = ToggleKey.buttonChangeBool;
    }

    private void ChangeIcon()
    {
        if (bgmSlider.value < 0.03)
        {
            bgmSlider.value = bgmSlider.minValue;
            muteIcon[0].sprite = muteSprite[1];
        }
        else
        {
            Sound.tempbgmVolume = bgmSlider.value;
            muteIcon[0].sprite = muteSprite[0];
        }

        if (sfxSlider.value < 0.03)
        {
            sfxSlider.value = sfxSlider.minValue;
            muteIcon[1].sprite = muteSprite[1];
        }
        else
        {
            Sound.tempsfxVolume = sfxSlider.value;
            muteIcon[1].sprite = muteSprite[0];
        }
    }
    private void Update()
    {
        ChangeIcon();

        Sound.bgmVolume = bgmSlider.value;
        Sound.sfxVolume = sfxSlider.value;

        ToggleKey.moveDistanceBool = moveDistanceToggle.isOn;
        ToggleKey.buttonChangeBool = buttonChangeToggle.isOn;
    }

    public void OnClickBGMMute()
    {
        if (bgmSlider.value == bgmSlider.minValue)
            bgmSlider.value = Sound.tempbgmVolume;
        else
            bgmSlider.value = bgmSlider.minValue;
    }

    public void OnClickSFXMute()
    {
        if (sfxSlider.value == sfxSlider.minValue)
            sfxSlider.value = Sound.tempsfxVolume;
        else
            sfxSlider.value = sfxSlider.minValue;
    }

    public void OnClickSaveBack_btn()
    {
        gameObject.SetActive(false);
        MainMenu.Save();
    }

}
