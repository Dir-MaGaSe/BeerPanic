using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtGoal, txtCurrentScore;
    [SerializeField] private GameObject hudPanel, pausePanel, gameoverPanel, winPanel;

    private void Update()
    {
        txtCurrentScore.text = "SCORE: " + GameManager.Instance.GetCurrentScore();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        hudPanel.SetActive(false);
        pausePanel.SetActive(true);
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        hudPanel.SetActive(true);
    }
}
