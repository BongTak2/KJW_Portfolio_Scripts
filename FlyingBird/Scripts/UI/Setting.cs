using TMPro;
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
    [SerializeField] protected TextMeshProUGUI[] keyTexts;
    [SerializeField] protected GameObject changeKey;

    [SerializeField] protected Slider bgmSlider;
    [SerializeField] protected Slider sfxSlider;

    [SerializeField] protected Sprite[] muteSprite;
    [SerializeField] protected Image[] muteIcon;

    private void Start()
    {
        bgmSlider.value = Sound.bgmVolume;
        sfxSlider.value = Sound.sfxVolume;
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
        KeyText();
        ChangeIcon();
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


    public void OnclickBack_btn()
    {
        if (!changeKey.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }

    private void KeyText()
    {
        for (int i = 0; i < keyTexts.Length; i++)
        {
            keyTexts[i].text = KeySetting.keyValues[(KeyAction)i] switch
            {
                KeyCode.LeftShift => "Shift",
                KeyCode.LeftAlt => "Alt",
                KeyCode.LeftControl => "Ctrl",
                //KeyCode.RightShift => "R-Shift",
                //KeyCode.RightControl => "R-Ctrl",
                //KeyCode.RightAlt => "R-Alt",
                KeyCode.Comma => ",",
                KeyCode.Slash => "/",
                KeyCode.Semicolon => ";",
                KeyCode.Period => ".",
                KeyCode.Quote => "'",
                KeyCode.Backslash => "\\",
                KeyCode.RightBracket => "]",
                KeyCode.LeftBracket => "[",
                KeyCode.UpArrow => "ก่",
                KeyCode.DownArrow => "ก้",
                KeyCode.LeftArrow => "ก็",
                KeyCode.RightArrow => "กๆ",
                _ => KeySetting.keyValues[(KeyAction)i].ToString()
            };

            keyTexts[i].fontSize = KeySetting.keyValues[(KeyAction)i] switch
            {
                KeyCode.UpArrow => 100f,
                KeyCode.DownArrow => 100f,
                KeyCode.LeftArrow => 100f,
                KeyCode.RightArrow => 100f,
                _ => 45f
            };

            keyTexts[i].fontStyle = KeySetting.keyValues[(KeyAction)i] switch
            {
                KeyCode.UpArrow => FontStyles.Bold,
                KeyCode.DownArrow => FontStyles.Bold,
                KeyCode.LeftArrow => FontStyles.Bold,
                KeyCode.RightArrow => FontStyles.Bold,
                _ => FontStyles.Normal
            };
        }
    }
}
