using UnityEngine;
using UnityEngine.EventSystems;

public enum ButtonType
{
    Rise,
    Boost,
    Skill
}
public class ActionButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] ButtonType buttonType;

    public void OnPointerDown(PointerEventData eventData)
    {
        GameManager.instance.inputKey[buttonType] = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GameManager.instance.inputKey[buttonType] = false;
    }
}
