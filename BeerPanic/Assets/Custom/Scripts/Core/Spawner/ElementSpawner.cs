using UnityEngine;
using System.Collections.Generic;

public class ElementSpawner : MonoBehaviour
{
    [Header("Configuración de Spawn")]
    [SerializeField] private float minSpawnX = -2.5f;
    [SerializeField] private float maxSpawnX = 2.5f;
    [SerializeField] private float spawnY = 6f;
    
    [Header("Referencias")]
    [SerializeField] private List<FruitElement> fruitPool;
    [SerializeField] private List<PowerUpElement> powerUpPool;
    [SerializeField] private List<ObstacleElement> obstaclePool;
    [SerializeField] private DifficultySettings[] difficultySettings;
    
    private ObjectPool<GameObject> gameObjectPool;
    private float nextSpawnTime;
    private int currentDifficultyLevel = 1;
    private bool isSpawning;
    private DifficultySettings currentSettings;
    
    private void Awake()
    {
        ValidateElements();
        InitializeObjectPool();
    }
    
    private void ValidateElements()
    {
        // Validar que tengamos los elementos requeridos según el documento técnico
        Debug.AssertFormat(fruitPool.Count >= 10, "Se requieren al menos 10 tipos de frutas. Actual: {0}", fruitPool.Count);
        Debug.AssertFormat(powerUpPool.Count >= 2, "Se requieren al menos 2 tipos de potenciadores. Actual: {0}", powerUpPool.Count);
        Debug.AssertFormat(obstaclePool.Count >= 2, "Se requieren al menos 2 tipos de obstáculos. Actual: {0}", obstaclePool.Count);
    }
    
    private void InitializeObjectPool()
    {
        int poolSize = CalculateOptimalPoolSize();
        gameObjectPool = new ObjectPool<GameObject>(CreatePooledElement,
                                                    OnGetElementFromPool,
                                                    OnReturnElementToPool,
                                                    OnDestroyPoolElement,
                                                    poolSize);
    }
    
    private int CalculateOptimalPoolSize()
    {
        // Calcular basado en la tasa de spawn más rápida posible
        float minSpawnDelay = difficultySettings[difficultySettings.Length - 1].minSpawnDelay;
        int maxSimultaneousElements = Mathf.CeilToInt(spawnY / (minSpawnDelay * Physics2D.gravity.magnitude));
        return (fruitPool.Count + powerUpPool.Count + obstaclePool.Count) * maxSimultaneousElements;
    }
    
    private GameObject CreatePooledElement()
    {
        GameObject element = new GameObject("Pooled Element");
        element.SetActive(false);
        
        // Configurar componentes básicos
        var spriteRenderer = element.AddComponent<SpriteRenderer>();
        spriteRenderer.sortingLayerName = "Gameplay";
        spriteRenderer.sortingOrder = 1;
        
        var rb = element.AddComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.freezeRotation = true;
        
        var collider = element.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        
        var behavior = element.AddComponent<ElementBehavior>();
        
        return element;
    }
    
    private void OnGetElementFromPool(GameObject element)
    {
        element.SetActive(true);
    }
    
    private void OnReturnElementToPool(GameObject element)
    {
        element.SetActive(false);
    }
    
    private void OnDestroyPoolElement(GameObject element)
    {
        Destroy(element);
    }
    
    public void StartSpawning()
    {
        isSpawning = true;
        nextSpawnTime = Time.time + currentSettings.minSpawnDelay;
    }
    
    public void StopSpawning()
    {
        isSpawning = false;
    }
    
    private void Update()
    {
        if (!isSpawning || !GameManager.Instance.IsGameActive) return;
        
        if (Time.time >= nextSpawnTime)
        {
            SpawnRandomElement();
            CalculateNextSpawnTime();
        }
    }
    
    private void SpawnRandomElement()
    {
        float random = Random.value * 100f;
        ElementBase elementToSpawn = null;
        
        if (random < currentSettings.fruitProbability)
        {
            elementToSpawn = fruitPool[Random.Range(0, fruitPool.Count)];
        }
        else if (random < currentSettings.fruitProbability + currentSettings.powerUpProbability)
        {
            elementToSpawn = powerUpPool[Random.Range(0, powerUpPool.Count)];
        }
        else
        {
            elementToSpawn = obstaclePool[Random.Range(0, obstaclePool.Count)];
        }
        
        if (elementToSpawn != null)
        {
            SpawnElement(elementToSpawn);
        }
    }
    
    private void SpawnElement(ElementBase element)
    {
        GameObject pooledObject = gameObjectPool.Get();
        if (pooledObject != null)
        {
            ConfigureElement(pooledObject, element);
        }
    }
    
    private void ConfigureElement(GameObject obj, ElementBase element)
    {
        // Configurar posición
        float randomX = Random.Range(minSpawnX, maxSpawnX);
        obj.transform.position = new Vector3(randomX, spawnY, 0);
        
        // Configurar componentes
        var spriteRenderer = obj.GetComponent<SpriteRenderer>();
        var rb = obj.GetComponent<Rigidbody2D>();
        var behavior = obj.GetComponent<ElementBehavior>();
        var collider = obj.GetComponent<BoxCollider2D>();
        
        // Configurar visual
        spriteRenderer.sprite = element.elementSprite;
        
        // Configurar físicas ajustadas a la dificultad
        rb.velocity = Vector2.zero;
        rb.mass = element.mass;
        rb.drag = element.linearDrag;
        rb.gravityScale = element.gravityScale * currentSettings.fallSpeedMultiplier;
        
        // Ajustar collider al sprite
        collider.size = element.elementSprite.bounds.size;
        
        // Configurar comportamiento
        behavior.Initialize(element, gameObjectPool);
    }
    
    private void CalculateNextSpawnTime()
    {
        float randomDelay = Random.Range(currentSettings.minSpawnDelay, currentSettings.maxSpawnDelay);
        nextSpawnTime = Time.time + (randomDelay / currentSettings.spawnRateMultiplier);
    }
    
    public void SetDifficultyLevel(int level)
    {
        currentDifficultyLevel = Mathf.Clamp(level, 1, difficultySettings.Length);
        currentSettings = difficultySettings[currentDifficultyLevel - 1];
        
        // Notificar cambio de dificultad para elementos activos
        /*var activeElements = FindObjectsOfType<ElementBehavior>();
        foreach (var element in activeElements)
        {
            element.UpdateDifficultySettings(currentSettings);
        }*/
    }
    
    public void ClearActiveElements()
    {
        var activeElements = FindObjectsOfType<ElementBehavior>();
        foreach (var element in activeElements)
        {
            element.ReturnToPool();
        }
    }
}