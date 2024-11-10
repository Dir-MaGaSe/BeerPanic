using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI multiplierText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button continueButton;

    [SerializeField] private ProgressionManager progressionManager;
    
    private void Start()
    {
        SubscribeToEvents();
        SetupButtons();
    }
    
    private void SubscribeToEvents()
    {
        progressionManager.OnScoreChanged += UpdateScore;
        progressionManager.OnMultiplierChanged += UpdateMultiplier;
        GameManager.Instance.OnLivesChanged += UpdateLives;
        progressionManager.OnLevelChanged += UpdateLevel;
        GameManager.Instance.OnGameOver += ShowGameOver;
    }
    
    private void SetupButtons()
    {
        continueButton.onClick.AddListener(() => GameManager.Instance.AddLivesFromAd());
    }
    
    private void UpdateScore(int score)
    {
        scoreText.text = $"Puntos: {score}";
    }
    
    private void UpdateMultiplier(float multiplier)
    {
        multiplierText.text = $"x{multiplier:F1}";
    }
    
    private void UpdateLives(int lives)
    {
        livesText.text = $"Vidas: {lives}";
    }
    
    private void UpdateLevel(int level)
    {
        levelText.text = $"Nivel {level}";
    }
    
    private void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
    }
    
    private void OnDestroy()
    {
        if (progressionManager != null)
        {
            progressionManager.OnScoreChanged -= UpdateScore;
            progressionManager.OnMultiplierChanged -= UpdateMultiplier;
            GameManager.Instance.OnLivesChanged -= UpdateLives;
            progressionManager.OnLevelChanged -= UpdateLevel;
            GameManager.Instance.OnGameOver -= ShowGameOver;
        }
    }
}