using TMPro;
using UnityEngine;

public class ModeSelect : MonoBehaviour
{
    public static int playMode;

    public MainMenu mainUI;

    [SerializeField] protected GameObject explainWindow;

    [SerializeField] protected TextMeshProUGUI modeTitle;
    [SerializeField] protected TextMeshProUGUI modeExplain;

    public void OnclickClassicMode_btn()
    {
        mainUI.SelectWindow();
        playMode = 1;
    }

    public void OnclickInfiniteMode_btn()
    {
        mainUI.SelectWindow();
        playMode = 2;
    }

    public void OnclickTimeAttackMode_btn()
    {
        mainUI.SelectWindow();
        playMode = 3;
    }

    public void MouseExit()
    {
        explainWindow.SetActive(false);
    }

    public void MousePointerClassic()
    {
        explainWindow.SetActive(true);
        modeTitle.text = "Ŭ���� ���";
        modeExplain.text = "���� ��� ȹ���Ͽ� ���� ������ ����ϼ���!\n��Ȳ�� �°� �پ��� ��ų�� ����� ������";
    }

    public void MousePointerInfinite()
    {
        explainWindow.SetActive(true);
        modeTitle.text = "���� ���";
        modeExplain.text = "�������� �޷�������!\n���� ������ ������ �߰��� ����ϴ�";
    }

    public void MousePointerTimeAttack()
    {
        explainWindow.SetActive(true);
        modeTitle.text = "Ÿ�� ����";
        modeExplain.text = "���� �ð� ���� �ִ��� �ָ� ���ư�����!\n�������� ���� �ð��� �����غ�����";
    }
}
