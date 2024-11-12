using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int currentScore = 0;
    public float bonusSpeed = 0;


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
    }

    public void CalculateSpeedBonus(float bonus, bool isBonusActive)
    {
        if(isBonusActive)
        {
            bonusSpeed = bonus;
        }
        else
        {
            bonusSpeed = 0;
        }
    }
}
