using UnityEngine;
using System.Collections.Generic;
using System;

public class ProgressionManager : MonoBehaviour
{
    // Removido el singleton ya que GameManager debe ser el único punto de control global
    
    [Header("Configuración")]
    [SerializeField] private List<DifficultySettings> difficultyLevels;
    
    [Header("Referencias")]
    [SerializeField] private ElementSpawner spawner;
    [SerializeField] private UIManager uiManager;
    
    private int currentScore;
    private int highScore;
    private int currentLevel = 1;
    private int consecutiveCaptures;
    private float currentMultiplier = 1f;
    private const int MAX_COMBO = 10;
    
    public event Action<int> OnScoreChanged;
    public event Action<float> OnMultiplierChanged;
    public event Action<int> OnLevelChanged;
    
    private void Start()
    {
        LoadProgress();
    }
    
    public void InitializeGame()
    {
        currentScore = 0;
        consecutiveCaptures = 0;
        currentMultiplier = 1f;
        
        UpdateUI();
        spawner.SetDifficultyLevel(currentLevel);
    }
    
    public void HandleElementCatch(ElementBase element, bool isCorrectCatch)
    {
        if (!GameManager.Instance.IsGameActive) return;
        
        if (isCorrectCatch)
        {
            ProcessCorrectCatch(element);
            PlayCatchSound(element);
        }
        else
        {
            ProcessIncorrectCatch();
            AudioManager.Instance.PlayEffect("incorrect_catch");
            GameManager.Instance.LoseLife();
        }
        
        UpdateUI();
        CheckLevelProgression();
    }
    
    private void PlayCatchSound(ElementBase element)
    {
        string soundId = element switch
        {
            FruitElement => "fruit_catch",
            PowerUpElement => "powerup_catch",
            ObstacleElement => "obstacle_hit",
            _ => "default_catch"
        };
        AudioManager.Instance.PlayEffect(soundId);
    }
    
    private void ProcessCorrectCatch(ElementBase element)
    {
        consecutiveCaptures = Mathf.Min(consecutiveCaptures + 1, MAX_COMBO);
        currentMultiplier = 1f + (consecutiveCaptures * 0.1f);
        
        DifficultySettings currentSettings = difficultyLevels[currentLevel - 1];
        float basePoints = element.basePoints * currentSettings.scoreMultiplier;
        
        // Aplicar multiplicadores
        int pointsToAdd = Mathf.RoundToInt(basePoints * currentMultiplier);
        
        // Bonus por combo si aplica
        if (consecutiveCaptures >= currentSettings.comboThreshold)
        {
            pointsToAdd = Mathf.RoundToInt(pointsToAdd * currentSettings.comboBonusMultiplier);
            AudioManager.Instance.PlayEffect("combo_bonus");
        }
        
        AddPoints(pointsToAdd);
    }
    
    private void ProcessIncorrectCatch()
    {
        consecutiveCaptures = 0;
        currentMultiplier = 1f;
        OnMultiplierChanged?.Invoke(currentMultiplier);
    }
    
    private void AddPoints(int points)
    {
        currentScore += points;
        if (currentScore > highScore)
        {
            highScore = currentScore;
            SaveProgress();
        }
        OnScoreChanged?.Invoke(currentScore);
    }
    
    private void CheckLevelProgression()
    {
        if (currentLevel < difficultyLevels.Count)
        {
            DifficultySettings nextLevel = difficultyLevels[currentLevel];
            if (currentScore >= nextLevel.scoreToUnlock)
            {
                UnlockNextLevel();
            }
        }
    }
    
    private void UnlockNextLevel()
    {
        currentLevel = Mathf.Min(currentLevel + 1, difficultyLevels.Count);
        spawner.SetDifficultyLevel(currentLevel);
        OnLevelChanged?.Invoke(currentLevel);
        SaveProgress();
        
        AudioManager.Instance.PlayEffect("level_up");
    }
    
    public int GetCurrentLevel() => currentLevel;
    public int GetHighScore() => highScore;
    public float GetCurrentMultiplier() => currentMultiplier;
    public int GetCurrentScore() => currentScore;
    
    private void SaveProgress()
    {
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.SetInt("UnlockedLevel", currentLevel);
        PlayerPrefs.Save();
    }
    
    private void LoadProgress()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        currentLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
    }
    
    private void UpdateUI()
    {
        OnScoreChanged?.Invoke(currentScore);
        OnMultiplierChanged?.Invoke(currentMultiplier);
    }
}