using UnityEngine;
using System;
using System.Collections.Generic;

public class ScoringManager : MonoBehaviour
{
    public static ScoringManager Instance { get; private set; }

    [Header("Configuración de Puntuación")]
    [SerializeField] private int baseComboMultiplier = 1;
    [SerializeField] private float comboTimeWindow = 2f;
    [SerializeField] private int maxComboChain = 10;

    [Header("Configuración de Combinaciones")]
    [SerializeField] private float combinationTimeWindow = 3f;
    [SerializeField] private int maxCombinationElements = 3;

    // Estado actual del sistema de puntuación
    private int currentScore;
    private int comboCount;
    private float currentMultiplier = 1f;
    private float lastCatchTime;
    
    // Sistema de combinaciones
    private List<FruitElement> currentCombination = new List<FruitElement>();
    private float lastFruitCatchTime;

    // Eventos
    public event Action<int> OnScoreChanged;
    public event Action<float> OnMultiplierChanged;
    public event Action<List<FruitElement>> OnCombinationComplete;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ProcessElementCatch(ElementBase element)
    {
        lastCatchTime = Time.time;
        
        // Procesar según el tipo de elemento
        if (element is FruitElement fruit)
        {
            ProcessFruitCatch(fruit);
        }
        else if (element is PowerUpElement powerUp)
        {
            ProcessPowerUpCatch(powerUp);
        }
        else if (element is ObstacleElement obstacle)
        {
            ProcessObstacleCatch(obstacle);
        }

        UpdateMultiplier();
        OnScoreChanged?.Invoke(currentScore);
    }

    private void ProcessFruitCatch(FruitElement fruit)
    {
        // Verificar tiempo para combo
        if (Time.time - lastCatchTime <= comboTimeWindow)
        {
            comboCount = Mathf.Min(comboCount + 1, maxComboChain);
        }
        else
        {
            comboCount = 1;
        }

        // Procesar combinación
        ProcessCombination(fruit);

        // Calcular puntos
        int points = CalculatePoints(fruit.basePoints);
        AddPoints(points);
    }

    private void ProcessCombination(FruitElement fruit)
    {
        if (Time.time - lastFruitCatchTime > combinationTimeWindow)
        {
            currentCombination.Clear();
        }

        currentCombination.Add(fruit);
        lastFruitCatchTime = Time.time;

        if (currentCombination.Count >= maxCombinationElements)
        {
            CheckAndRewardCombination();
        }
    }

    private void CheckAndRewardCombination()
    {
        //bool isValidCombination = true;
        FruitElement firstFruit = currentCombination[0];

        // Verificar si la combinación es válida
        foreach (string validCombination in firstFruit.validCombinations)
        {
            bool allFruitsMatch = currentCombination.TrueForAll(fruit => 
                Array.Exists(fruit.validCombinations, valid => valid == validCombination));

            if (allFruitsMatch)
            {
                // Recompensar combinación
                int combinationBonus = firstFruit.basePoints * 
                                     firstFruit.combinationMultiplier * 
                                     currentCombination.Count;
                AddPoints(combinationBonus);
                OnCombinationComplete?.Invoke(new List<FruitElement>(currentCombination));
                break;
            }
        }

        currentCombination.Clear();
    }

    private void ProcessPowerUpCatch(PowerUpElement powerUp)
    {
        // Los powerUps no afectan el combo pero dan puntos base
        AddPoints(powerUp.basePoints);
    }

    private void ProcessObstacleCatch(ObstacleElement obstacle)
    {
        // Los obstáculos rompen el combo y restan puntos
        ResetCombo();
        AddPoints(-obstacle.basePoints);
    }

    private int CalculatePoints(int basePoints)
    {
        return Mathf.RoundToInt(basePoints * currentMultiplier);
    }

    private void AddPoints(int points)
    {
        currentScore += points;
        currentScore = Mathf.Max(currentScore, 0); // Evitar puntuación negativa
        OnScoreChanged?.Invoke(currentScore);
    }

    private void UpdateMultiplier()
    {
        currentMultiplier = 1f + (comboCount - 1) * 0.1f;
        OnMultiplierChanged?.Invoke(currentMultiplier);
    }

    public void ResetCombo()
    {
        comboCount = 0;
        currentMultiplier = 1f;
        OnMultiplierChanged?.Invoke(currentMultiplier);
    }

    public void ResetScore()
    {
        currentScore = 0;
        ResetCombo();
        currentCombination.Clear();
        OnScoreChanged?.Invoke(currentScore);
    }
}

public partial class ElementBehavior
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ScoringManager.Instance.ProcessElementCatch(elementData);
            ReturnToPool();
        }
    }
}