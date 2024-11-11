using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Referencias")]
    [SerializeField] private ProgressionManager progressionManager;
    [SerializeField] private ElementSpawner elementSpawner;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameSettings gameSettings;
    
    private bool isGamePaused;
    public bool IsGameActive { get; private set; }
    private int currentLives;
    private const int INITIAL_LIVES = 3;
    private const int LIVES_REWARD_ADS = 2;
    
    public event Action OnGameStarted;
    public event Action OnGamePaused;
    public event Action OnGameResumed;
    public event Action OnGameOver;
    public event Action<int> OnLivesChanged;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void InitializeGame()
    {
        IsGameActive = false;
        Application.targetFrameRate = gameSettings.targetFrameRate;
        AudioManager.Instance?.Initialize(gameSettings.audioSettings);
        ResetLives();
    }
    
    public void StartGame()
    {
        IsGameActive = true;
        ResetLives();
        progressionManager.InitializeGame();
        elementSpawner.StartSpawning();
        OnGameStarted?.Invoke();
    }
    
    public void PauseGame()
    {
        if (!isGamePaused && IsGameActive)
        {
            isGamePaused = true;
            Time.timeScale = 0;
            OnGamePaused?.Invoke();
        }
    }
    
    public void ResumeGame()
    {
        if (isGamePaused && IsGameActive)
        {
            isGamePaused = false;
            Time.timeScale = 1;
            OnGameResumed?.Invoke();
        }
    }
    
    public void LoseLife()
    {
        currentLives--;
        OnLivesChanged?.Invoke(currentLives);
        
        if (currentLives <= 0)
        {
            GameOver();
        }
    }
    
    public void AddLivesFromAd()
    {
        currentLives += LIVES_REWARD_ADS;
        OnLivesChanged?.Invoke(currentLives);
        ResumeGame();
    }
    
    private void GameOver()
    {
        IsGameActive = false;
        OnGameOver?.Invoke();
        elementSpawner.StopSpawning();
    }
    
    private void ResetLives()
    {
        currentLives = INITIAL_LIVES;
        OnLivesChanged?.Invoke(currentLives);
    }
}