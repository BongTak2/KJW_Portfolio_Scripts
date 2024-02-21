using UnityEngine;

public class ButtonChange : MonoBehaviour
{
    [SerializeField] RectTransform riseButton;
    [SerializeField] RectTransform boostButton;
    [SerializeField] RectTransform skillButton;

    float changeRisePosX;
    float defaultRisePosX;
    float changeBoostPosX;
    float defaultBoostPosX;
    float changeSkillPosX;
    float defaultSkillPosX;

    private void Start()
    {
        defaultRisePosX = riseButton.anchoredPosition.x;
        changeRisePosX = -riseButton.anchoredPosition.x;

        defaultBoostPosX = boostButton.anchoredPosition.x;
        changeBoostPosX = -boostButton.anchoredPosition.x;

        defaultSkillPosX = skillButton.anchoredPosition.x;
        changeSkillPosX = -skillButton.anchoredPosition.x;
    }
    private void Update()
    {
        if (ToggleKey.buttonChangeBool)
        {
            riseButton.anchoredPosition = new Vector2(changeRisePosX, riseButton.anchoredPosition.y);
            skillButton.anchoredPosition = new Vector2(changeSkillPosX, skillButton.anchoredPosition.y);
            boostButton.anchoredPosition = new Vector2(changeBoostPosX, boostButton.anchoredPosition.y);
        }
        else
        {
            riseButton.anchoredPosition = new Vector2(defaultRisePosX, riseButton.anchoredPosition.y);
            skillButton.anchoredPosition = new Vector2(defaultSkillPosX, skillButton.anchoredPosition.y);
            boostButton.anchoredPosition = new Vector2(defaultBoostPosX, boostButton.anchoredPosition.y);
        }
    }
}
