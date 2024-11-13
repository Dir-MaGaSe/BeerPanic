using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private int difficulty = 1;

    private int currentScore = 0;
    private float bonusSpeed = 1;


    private void Awake()
    {
        if(GameManager.Instance == null)
        {
            GameManager.Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddPoints(int points)
    {
        currentScore += points;
        Debug.Log(currentScore);
    }

    public void CalculateSpeedBonus(float multiplier, bool isBonusActive)
    {
        if(isBonusActive)
        {
            bonusSpeed = multiplier;
        }
        else
        {
            bonusSpeed = 1;
        }
    }

    public float GetSpeedBonus() { return bonusSpeed; }
    public int GetDifficulty() { return difficulty; }
}
