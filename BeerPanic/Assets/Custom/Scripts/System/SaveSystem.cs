using UnityEngine;
using System;

public class SaveSystem : MonoBehaviour
{
    private const string SAVE_KEY = "GameSave";
    private const string HIGH_SCORE_KEY = "HighScore";
    private const string UNLOCKED_LEVEL_KEY = "UnlockedLevel";
    
    [Serializable]
    private class SaveData
    {
        public int highScore;
        public int unlockedLevel;
        public float musicVolume;
        public float effectsVolume;
    }
    
    public static void SaveGame(int highScore, int unlockedLevel, float musicVolume, float effectsVolume)
    {
        SaveData data = new SaveData
        {
            highScore = highScore,
            unlockedLevel = unlockedLevel,
            musicVolume = musicVolume,
            effectsVolume = effectsVolume
        };
        
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();
    }
    
    public static bool LoadGame(out int highScore, out int unlockedLevel, out float musicVolume, out float effectsVolume)
    {
        string json = PlayerPrefs.GetString(SAVE_KEY, "");
        
        if (string.IsNullOrEmpty(json))
        {
            highScore = 0;
            unlockedLevel = 1;
            musicVolume = 1f;
            effectsVolume = 1f;
            return false;
        }
        
        SaveData data = JsonUtility.FromJson<SaveData>(json);
        highScore = data.highScore;
        unlockedLevel = data.unlockedLevel;
        musicVolume = data.musicVolume;
        effectsVolume = data.effectsVolume;
        return true;
    }
    
    public static void ResetSave()
    {
        PlayerPrefs.DeleteKey(SAVE_KEY);
        PlayerPrefs.Save();
    }
}