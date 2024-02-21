using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelect : MonoBehaviour
{
    public static GameObject startPlayer;

    [SerializeField] protected List<GameObject> playerPrefabs = new List<GameObject>();
    [SerializeField] protected GameObject status;

    private Animator playerSelect;
    private int selectPlayer;
    public static int characterNum;

    public void OnclickSelectNum1_btn()
    {
        startPlayer = playerPrefabs[0];
        selectPlayer = 1;
        SelectPlayer();
    }

    public void OnclickSelectNum2_btn()
    {
        startPlayer = playerPrefabs[1];
        selectPlayer = 2;
        SelectPlayer();
    }

    public void OnclickSelectNum3_btn()
    {
        startPlayer = playerPrefabs[2];
        selectPlayer = 3;
        SelectPlayer();
    }

    public void OnclickStart_btn()
    {
        characterNum = selectPlayer;
        selectPlayer = 0;
        SoundManager.instance.BGMStop();
        SceneManager.LoadScene(ModeSelect.playMode);
    }

    public void SelectPlayer()
    {
        playerSelect = GetComponent<Animator>();
        status.SetActive(true);
        if (selectPlayer != 0)
        {
            switch (selectPlayer)
            {
                case 1:
                    playerSelect.SetBool("Blue", true);
                    playerSelect.SetBool("Red", false);
                    playerSelect.SetBool("Yellow", false);
                    break;
                case 2:
                    playerSelect.SetBool("Blue", false);
                    playerSelect.SetBool("Red", true);
                    playerSelect.SetBool("Yellow", false);
                    break;
                case 3:
                    playerSelect.SetBool("Blue", false);
                    playerSelect.SetBool("Red", false);
                    playerSelect.SetBool("Yellow", true);
                    break;
            }
        }
        else
        {
            playerSelect.SetBool("Blue", false);
            playerSelect.SetBool("Red", false);
            playerSelect.SetBool("Yellow", false);
        }
    }
}
