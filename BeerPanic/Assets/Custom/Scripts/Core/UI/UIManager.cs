using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtGoal, txtCurrentScore, txtLives;
    [SerializeField] private GameObject hudPanel, pausePanel, gameoverPanel, winPanel;

    private void Awake() 
    {
        pausePanel.SetActive(false);
        gameoverPanel.SetActive(false);
        winPanel.SetActive(false);
        hudPanel.SetActive(true);

        GameManager.Instance.ResetScore();
        GameManager.Instance.ResetLives();
    }
    private void Start() 
    {
        txtGoal.text = "GOAL: " + GameManager.Instance.GetLevelGoal();
    }
    private void Update()
    {
        txtCurrentScore.text = "SCORE: " + GameManager.Instance.GetCurrentScore();
        txtLives.text = "LIVES: " + GameManager.Instance.GetCurrentLives();

        if(GameManager.Instance.GetCurrentLives() <= 0) { GameOver(); }
        if(GameManager.Instance.GetCurrentScore()
            >= GameManager.Instance.GetLevelGoal()) { Win(); }
    }

    public void ExitLevel()
    {
        Time.timeScale = 1f;  

        // Cambio de escena al menu      
    }
    public void GameOver()
    {
        Time.timeScale = 0f;
        hudPanel.SetActive(false);
        gameoverPanel.SetActive(true); 
    }
    public void GoNextLevel()
    {
        Time.timeScale = 1f;
        
        // Cambio de escena al siguiente nivel     
    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
        hudPanel.SetActive(false);
        pausePanel.SetActive(true);
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;

        pausePanel.SetActive(false);
        gameoverPanel.SetActive(false);
        winPanel.SetActive(false);
        hudPanel.SetActive(true);

        GameManager.Instance.ResetScore();
        GameManager.Instance.ResetLives();
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        hudPanel.SetActive(true);
    }
    public void Win()
    {
        Time.timeScale = 0f;
        hudPanel.SetActive(false);
        winPanel.SetActive(true); 
    }
}
