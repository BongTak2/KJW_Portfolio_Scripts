using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class UI_LevelUp : MonoBehaviour
{
    Character character;

    Transform aPBtn;
    Transform aSBtn;
    Transform dPBtn;
    int count;
    private void Start()
    {
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        aPBtn = transform.GetChild(0);
        aSBtn = transform.GetChild(1);
        dPBtn = transform.GetChild(2);
        count = 1;
    }
    private void Update()
    {
        if (character.AtkPowerLevel >= 5)
        {
            aPBtn.gameObject.SetActive(false);
        }
        if (character.AtkSpeedLevel >= 5)
        {
            aSBtn.gameObject.SetActive(false);
        }
        if (character.DefPowerLevel >= 5)
        {
            dPBtn.gameObject.SetActive(false);
        }

        if (character.statUpWindow)
        {
            if (character.AtkPowerLevel < 5)
            {
                aPBtn.gameObject.SetActive(true);
            }
            if (character.AtkSpeedLevel < 5)
            {
                aSBtn.gameObject.SetActive(true);
            }
            if (character.DefPowerLevel < 5)
            {
                dPBtn.gameObject.SetActive(true);
            }
            character.statUpWindow = false;
            count++;
        }

        if (aPBtn.gameObject.activeSelf)
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    OnClickAtkPower();
                }
            }
        }

        if (aSBtn.gameObject.activeSelf)
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    OnClickAtkSpeed();
                }
            }
        }

        if (dPBtn.gameObject.activeSelf)
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    OnClickDefPower();
                }
            }
        }

    }

    public void OnClickAtkPower()
    {
        character.AtkPowerLevel++;
        if (count == 1)
        {
            AllClose(false);
        }
        count--;
    }

    public void OnClickAtkSpeed()
    {
        character.AtkSpeedLevel++;
        if (count == 1)
        {
            AllClose(false);
        }
        count--;
    }

    public void OnClickDefPower()
    {
        character.DefPowerLevel++;
        if (count == 1)
        {
            AllClose(false);
        }
        count--;
    }

    private void AllClose(bool _active)
    {
        aPBtn.gameObject.SetActive(_active);
        aSBtn.gameObject.SetActive(_active);
        dPBtn.gameObject.SetActive(_active);
    }
}
