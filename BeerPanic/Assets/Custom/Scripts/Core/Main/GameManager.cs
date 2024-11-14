using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private int difficulty = 1, levelGoal;

    private int currentScore = 0, currentLives = 3;
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

    private void Start() 
    {
        ResetScore();
        ResetLives();
        bonusSpeed = 1;
    }

    public void AddPoints(int points)
    {
        int tempPoints = points;
        if(difficulty >= 7)
        {
            if(points > 0){tempPoints = points / 2; }
            if(points < 0){tempPoints = points * 2; }
        }
        
        currentScore += tempPoints;

        if(currentScore < 0)
        {
            currentScore = 0;
        }
    }

    public void CalculateSpeedBonus(float multiplier, bool isBonusActive)
    {
        if(isBonusActive)
        {
            bonusSpeed = multiplier;
        }
    }

    public void TakeDamage(int damage)
    {
        int temporaryLife = currentLives - damage;
        
        if(temporaryLife < 0)
        {
            currentLives = 0;
        }
        else
        {
            currentLives = temporaryLife;
        }
    }

    public void ResetScore() { currentScore = 0; }
    public void ResetLives() { currentLives = 3; }

    public float GetSpeedBonus() { return bonusSpeed; }
    public int GetDifficulty() { return difficulty; }
    public int GetCurrentScore() { return currentScore; }
    public int GetCurrentLives() { return currentLives; }
    public int GetLevelGoal() { return levelGoal; }
}
