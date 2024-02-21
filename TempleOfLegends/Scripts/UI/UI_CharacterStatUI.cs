using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct UI_Character_Info
{
    public int level;
    public float exp;
    public float maxExp;

    public float currentHp;
    public float maxHp;

    public float currentMana;
    public float maxMana;

    public float atkPower;
    public float atkSpeed;
    public float defPower;

    public int currentWeaponBullet;
    public int subWeaponBullet;
}

public class UI_CharacterStatUI : MonoBehaviour
{
    public static UI_CharacterStatUI instance;

    public UI_Character_Info character;

    public TextMeshProUGUI health_Text;
    public TextMeshProUGUI mana_Text;

    public TextMeshProUGUI atkPower_Text;
    public TextMeshProUGUI atkSpeed_Text;
    public TextMeshProUGUI defPower_Text;

    public TextMeshProUGUI level_Text;

    public TextMeshProUGUI currentWeapon_Text;
    public TextMeshProUGUI subWeapon_Text;

    public TextMeshProUGUI spell_Heal_Text;
    public TextMeshProUGUI spell_Flash_Text;
    public TextMeshProUGUI skill_R_Text;
    public TextMeshProUGUI skill_Q_Text;
    public TextMeshProUGUI deathRespawn_Text;

    public Image healthImage;
    public Image manaImage;
    public Image expImage;

    public Image spell_Heal_Image;
    public Image spell_Flash_Image;
    public Image skill_R_Image;
    public Image skill_Q_Image;
    public Image deathRespawn_Image;

    public void Initialize()
    {
        this.Singleton(ref instance);
    }

    void Update()
    {
        if (character.maxHp > 0 && character.maxMana > 0)
            GetUnitHp();
    }

    public void GetUnitHp()
    {
        health_Text.text = $"{character.currentHp:F0} / {character.maxHp}";
        mana_Text.text = $"{character.currentMana:F0} / {character.maxMana}";

        atkPower_Text.text = $"{character.atkPower:##0.#}";
        atkSpeed_Text.text = $"{character.atkSpeed:##0.###}";
        defPower_Text.text = $"{character.defPower:##0.#}";

        level_Text.text = $"{character.level}";

        if (character.currentWeaponBullet == -1)
        {
            currentWeapon_Text.text = 0.ToString();
        }
        else
        {
            currentWeapon_Text.text = $"{character.currentWeaponBullet}";
        }

        if (character.currentWeaponBullet == -1)
        {
            subWeapon_Text.text = 0.ToString();
        }
        else
        {
            subWeapon_Text.text = $"{character.subWeaponBullet}";
        }

        if (character.level < 5)
        {
            skill_R_Text.enabled = false;
            skill_R_Image.color = new Color(0.3f, 0.3f, 0.3f);
        }
        else
        {
            coolTimeImage(TimeManager.instance.UltReady, skill_R_Text, skill_R_Image, TimeManager.instance.ultCool);
        }

        coolTimeImage(TimeManager.instance.DeathRespawn_Ready, deathRespawn_Text, deathRespawn_Image, TimeManager.instance.deathRespawn_Cool);
        coolTimeImage(TimeManager.instance.Skill_Q_Ready, skill_Q_Text, skill_Q_Image, TimeManager.instance.skill_Q_Cool);
        coolTimeImage(TimeManager.instance.Spell_Heal_Ready, spell_Heal_Text, spell_Heal_Image, TimeManager.instance.spell_Heal_Cool);
        coolTimeImage(TimeManager.instance.Spell_Flash_Ready, spell_Flash_Text, spell_Flash_Image, TimeManager.instance.spell_Flash_Cool);

        healthImage.fillAmount = Mathf.Lerp(healthImage.fillAmount, character.currentHp / character.maxHp, Time.deltaTime);
        manaImage.fillAmount = character.currentMana / character.maxMana;
        expImage.fillAmount = character.exp / character.maxExp;
    }

    private void coolTimeImage(bool con, TextMeshProUGUI text, Image image, float coolTime)
    {
        if (con)
        {
            text.enabled = false;
            image.color = new Color(1f, 1f, 1f);
        }
        else
        {
            text.enabled = true;
            image.color = new Color(0.5f, 0.5f, 0.5f);
            text.text = $"{coolTime:F0}";
        }
    }
}
