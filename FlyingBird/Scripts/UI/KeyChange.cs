using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class KeyChange : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI changeText;

    private int keyNum = -1;
    private KeyCode keyCode;
    public static bool changeable;
    private readonly KeyCode[] defaultKeys = new KeyCode[] { KeyCode.Space, KeyCode.LeftShift, KeyCode.A };
    [SerializeField] protected AudioClip error;

    private void OnGUI()
    {
        Event keyEvent = Event.current;
        if (keyEvent.isKey && changeable)
        {
            keyCode = KeyType(keyEvent);
            TextStyle(changeText);
            changeText.text = keyCode switch
            {
                KeyCode.None => "다시 입력하세요..",
                _ => TextSetting(keyCode)
            };

            if (keyCode != KeyCode.None)
            {
                SwapKey(keyNum, keyCode);
                changeable = false;
                gameObject.SetActive(false);
                EventSystem.current.SetSelectedGameObject(null);
            }
            else
            {
                SoundManager.instance.EffectSoundPlay(error);
            }
        }
    }

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void ChangeKeyNum(int num)
    {
        if (!gameObject.activeSelf)
        {
            keyNum = num;
            keyCode = KeySetting.keyValues[(KeyAction)keyNum];
            changeable = true;
            changeText.text = TextSetting(keyCode);
            TextStyle(changeText);
            gameObject.SetActive(true);
        }        
    }

    public void OnClickDefault()
    {
        SwapKey(keyNum, defaultKeys[keyNum]);

        gameObject.SetActive(false);
    }

    private void SwapKey(int num, KeyCode _keyCode)
    {
        for (int i = 0; i < 3; i++)
        {
            if (KeySetting.keyValues[(KeyAction)i] == _keyCode)
            {
                KeySetting.keyValues[(KeyAction)i] = KeySetting.keyValues[(KeyAction)num];
            }
        }
        KeySetting.keyValues[(KeyAction)num] = _keyCode;
    }

    private string TextSetting(KeyCode key)
    {
        string text = key switch
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
            KeyCode.UpArrow => "↑",
            KeyCode.DownArrow => "↓",
            KeyCode.LeftArrow => "←",
            KeyCode.RightArrow => "→",
            _ => key.ToString()
        };
        return text;
    }

    private KeyCode KeyType(Event keyEvent)
    {
        switch (keyEvent.keyCode)
        {
            case KeyCode.A:
            case KeyCode.B:
            case KeyCode.C:
            case KeyCode.D:
            case KeyCode.E:
            case KeyCode.F:
            case KeyCode.G:
            case KeyCode.H:
            case KeyCode.I:
            case KeyCode.J:
            case KeyCode.K:
            case KeyCode.L:
            case KeyCode.M:
            case KeyCode.N:
            case KeyCode.O:
            case KeyCode.P:
            case KeyCode.Q:
            case KeyCode.R:
            case KeyCode.S:
            case KeyCode.T:
            case KeyCode.U:
            case KeyCode.V:
            case KeyCode.W:
            case KeyCode.X:
            case KeyCode.Y:
            case KeyCode.Z:
            case KeyCode.Space:
            case KeyCode.LeftAlt:
            case KeyCode.LeftShift:
            case KeyCode.LeftControl:
            //case KeyCode.RightShift:
            //case KeyCode.RightControl:
            //case KeyCode.RightAlt:
            case KeyCode.Comma:
            case KeyCode.Slash:
            case KeyCode.Semicolon:
            case KeyCode.Period:
            case KeyCode.Quote:
            case KeyCode.Backslash:
            case KeyCode.RightBracket:
            case KeyCode.LeftBracket:
            case KeyCode.UpArrow:
            case KeyCode.DownArrow:
            case KeyCode.LeftArrow:
            case KeyCode.RightArrow:
            case KeyCode.Tab:
                break;
            default:
                keyEvent.keyCode = KeyCode.None;
                break;

        }
        return keyEvent.keyCode;
    }
    private TextMeshProUGUI TextStyle(TextMeshProUGUI text)
    {
        text.fontSize = keyCode switch
        {
            KeyCode.None => 40f,
            KeyCode.UpArrow => 80f,
            KeyCode.DownArrow => 80f,
            KeyCode.LeftArrow => 80f,
            KeyCode.RightArrow => 80f,
            _ => 50f
        };
        text.fontStyle = keyCode switch
        {
            KeyCode.UpArrow => FontStyles.Bold,
            KeyCode.DownArrow => FontStyles.Bold,
            KeyCode.LeftArrow => FontStyles.Bold,
            KeyCode.RightArrow => FontStyles.Bold,
            _ => FontStyles.Normal
        };
        return text;
    }
}
