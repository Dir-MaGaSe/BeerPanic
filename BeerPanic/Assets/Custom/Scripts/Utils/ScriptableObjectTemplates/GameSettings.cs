using UnityEngine;

[CreateAssetMenu(fileName = "NewGameSettings", menuName = "BeerPanic/Settings/Game Settings")]
public class GameSettings : ScriptableObject
{
    [Header("Rendimiento")]
    public int targetFrameRate = 30;
    public bool useVSync = false;
    
    [Header("Audio")]
    public AudioSettings audioSettings;
    
    [Header("Dificultad")]
    public DifficultySettings[] difficultyLevels;
}