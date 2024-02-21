using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class UI_FloatAmount_Text : MonoBehaviour
{
    TextMeshProUGUI healthBar_Text;
    string value;

    void Start()
    {
        healthBar_Text = GetComponent<TextMeshProUGUI>();
        UIManager.instance.OnChangeUnitHealth += ChangeValue;
    }
    void Update()
    {
        healthBar_Text.text = value;

    }
    private void OnDestroy()
    {
        UIManager.instance.OnChangeUnitHealth -= ChangeValue;
    }

    public void ChangeValue(float current, float max)
    {
        value = $"{current} / {max}";
    }
}
