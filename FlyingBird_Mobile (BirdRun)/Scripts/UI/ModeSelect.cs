using TMPro;
using UnityEngine;

public class ModeSelect : MonoBehaviour
{
    public static int playMode;

    [SerializeField] protected MainMenu mainUI;  

    [SerializeField] protected GameObject explainWindow;

    [SerializeField] protected TextMeshProUGUI modeTitle;
    [SerializeField] protected TextMeshProUGUI modeExplain;

    public void OnclickClassicMode_btn()
    {
        explainWindow.SetActive(true);
        modeTitle.text = "클래식 모드";
        modeExplain.text = "별을 모두 획득하여 골인 지점을 통과하세요!\n상황에 맞게 다양한 스킬을 사용해 보세요";
        playMode = 1;
    }

    public void OnclickInfiniteMode_btn()
    {
        explainWindow.SetActive(true);
        modeTitle.text = "무한 모드";
        modeExplain.text = "무한으로 달려보세요!\n별을 먹으면 점수를 추가로 얻습니다";
        playMode = 2;
    }

    public void OnclickTimeAttackMode_btn()
    {
        explainWindow.SetActive(true);
        modeTitle.text = "타임 어택";
        modeExplain.text = "제한 시간 내에 최대한 멀리 날아가세요!\n아이템을 통해 시간을 연장해보세요";
        playMode = 3;
    }

    public void OnClickSelect_btn()
    {
        mainUI.SelectWindow();
    }

    public void MouseExit()
    {
        explainWindow.SetActive(false);
    }

    public void MousePointerClassic()
    {
        explainWindow.SetActive(true);
        modeTitle.text = "클래식 모드";
        modeExplain.text = "별을 모두 획득하여 골인 지점을 통과하세요!\n상황에 맞게 다양한 스킬을 사용해 보세요";
    }

    public void MousePointerInfinite()
    {
        explainWindow.SetActive(true);
        modeTitle.text = "무한 모드";
        modeExplain.text = "무한으로 달려보세요!\n별을 먹으면 점수를 추가로 얻습니다";
    }

    public void MousePointerTimeAttack()
    {
        explainWindow.SetActive(true);
        modeTitle.text = "타임 어택";
        modeExplain.text = "제한 시간 내에 최대한 멀리 날아가세요!\n아이템을 통해 시간을 연장해보세요";
    }
}
