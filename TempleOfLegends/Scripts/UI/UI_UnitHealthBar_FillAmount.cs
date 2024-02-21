using UnityEngine;
using UnityEngine.UI;

public class UI_UnitHealthBar_FillAmount : MonoBehaviour
{
    Image healthBar_Image;
    float value;

    void Start()
    {
        healthBar_Image = GetComponent<Image>();
        UIManager.instance.OnChangeUnitHealth += ChangeValue;
    }

    void Update()
    {
        //healthBar_Image.fillAmount = Mathf.Lerp(healthBar_Image.fillAmount, value, Time.deltaTime);
        healthBar_Image.fillAmount = value;

    }
    private void OnDestroy()
    {
        UIManager.instance.OnChangeUnitHealth -= ChangeValue;
    }

    public void ChangeValue(float current, float max)
    {
        value = current / max;
    }
}
