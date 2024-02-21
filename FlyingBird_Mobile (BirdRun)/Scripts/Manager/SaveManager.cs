using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Score
{
    public static List<int> infiniteScore = new List<int>();
    public static List<int> timeAttackScore = new List<int>();

    public static Dictionary<int, int> infiniteScoreCharacterMapping = new Dictionary<int, int>();
    public static Dictionary<int, int> timeAttackScoreCharacterMapping = new Dictionary<int, int>();

    public const int rankingLength = 5;    
}

public static class Sound
{
    public static float bgmVolume;
    public static float sfxVolume;

    public static float tempbgmVolume;
    public static float tempsfxVolume;
}

public static class ToggleKey
{
    public static bool moveDistanceBool;
    public static bool buttonChangeBool;
}

public class SaveData
{
    public int[] infiniteModeScore = new int[Score.rankingLength];
    public int[] timeAttackModeScore = new int[Score.rankingLength];

    public float bgmVolume = 0.4f;
    public float sfxVolume = 0.25f;

    public float tempbgmVolume;
    public float tempsfxVolume;

    public bool moveDistance = true;
    public bool buttonChange = false;
}

public static class SaveManager
{
    private static readonly string _path;

    public static string Path => _path ?? Application.persistentDataPath + "/data.json";

    public static void Save(SaveData saveData)
    {
        if (!File.Exists(Path))
        {
            File.Create(Path).Close();
        }
        File.WriteAllText(Path, JsonUtility.ToJson(saveData));
    }

    public static SaveData Load()
    {
        if (!File.Exists(Path))
        {
            return new SaveData();
        }
        return JsonUtility.FromJson<SaveData>(File.ReadAllText(Path));
    }
}
