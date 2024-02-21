using UnityEngine;
using UnityEngine.UI;

public class UI_WeaponImage : MonoBehaviour
{
    public Image[] images;

    public Sprite[] sprites;

    private Character player;

    private void Start()
    {
        player = SpawnManager.player.GetComponent<Character>();
    }
    private void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            if (player.CheckCurrentWeapon((WeaponType)i))
            {
                images[0].sprite = sprites[i];
            }

            if (player.CheckSubWeapon((WeaponType)i))
            {
                images[1].sprite = sprites[i];
            }
        }
    }
    public void WeaponImage(int weaponType, int spriteType)
    {
        images[weaponType].sprite = sprites[spriteType];
    }
}
