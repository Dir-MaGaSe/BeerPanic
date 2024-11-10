using UnityEngine;

[CreateAssetMenu(fileName = "NewLevel", menuName = "BeerPanic/Progression/DifficultySettings")]
public class DifficultySettings : ScriptableObject
{
    [Header("Configuración de Nivel")]
    public string levelName;
    public int levelNumber;
    public int scoreToUnlock;
    
    [Header("Modificadores de Spawn")]
    public float spawnRateMultiplier = 1f;
    public float minSpawnDelay = 1f;
    public float maxSpawnDelay = 5f;
    public float fallSpeedMultiplier = 1f;
    [Range(0f, 1f)]
    public float fruitProbability = 0.7f;
    [Range(0f, 1f)]
    public float powerUpProbability = 0.15f;
    [Range(0f, 1f)]
    public float obstacleProbability = 0.15f;
    
    [Header("Configuración de Puntuación")]
    public float scoreMultiplier = 1f;
    public int comboThreshold = 3;
    public float comboBonusMultiplier = 1.5f;
    
}