using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SpawnManager : MonoBehaviour
{
    [Header("Configuración de Spawn")]
    [SerializeField] Transform borderLeft;  // Límite izquierdo de la pantalla para el spawn
    [SerializeField] Transform borderRight; // Límite derecho de la pantalla para el spawn
    [SerializeField] private float spawnInterval = 1f; // Intervalo base de spawn
    
    [Header("Referencias")]
    [SerializeField] private GameObject prefabObject;  // Prefab base para los elementos
    [SerializeField] private List<FruitElement> fruitPool;     // Lista de posibles frutas a spawnear
    [SerializeField] private List<PowerUpElement> powerUpPool; // Lista de potenciadores
    [SerializeField] private List<ObstacleElement> obstaclePool; // Lista de obstáculos
    
    private ObjectPool<GameObject> gameObjectPool; // Pool de objetos
    private float nextSpawnTime; // Tiempo siguiente de spawn
    private int difficultyLevel;  // Nivel de dificultad que afecta las probabilidades de spawn
    
    private void Start()
    {
        InitializeObjectPool(); // Inicializa el pool de objetos
        nextSpawnTime = Time.time + spawnInterval; // Calcula el primer spawn
        difficultyLevel = GameManager.Instance.GetDifficulty();

        if(difficultyLevel < 1) { difficultyLevel = 1; }
        if(difficultyLevel >= 7) { difficultyLevel = 7; }
    }
    
    private void Update()
    {
        if (Time.time >= nextSpawnTime) // Verifica si es tiempo de spawn
        {
            SpawnRandomElement();
            CalculateNextSpawnTime(); // Recalcula el tiempo para el próximo spawn
        }
    }
    
    private void InitializeObjectPool()
    {
        // Inicializa el pool de objetos con un tamaño ajustado al número de elementos
        int poolMaxSize = (fruitPool.Count + powerUpPool.Count + obstaclePool.Count) * 3;
        gameObjectPool = new ObjectPool<GameObject>(() => 
        {
            return Instantiate(prefabObject); // Crea un nuevo objeto de pool
        }, poolObject => 
        {
            poolObject.gameObject.SetActive(true); // Activa el objeto al obtenerlo del pool
        }, poolObject => 
        {
            poolObject.gameObject.SetActive(false); // Desactiva el objeto al devolverlo al pool
        }, poolObject => 
        {
            Destroy(poolObject.gameObject); // Destruye el objeto si se excede el tamaño del pool
        }, true, 15, poolMaxSize); // Configuración inicial del pool
    }
    
    private void SpawnRandomElement()
    {
        // Selecciona aleatoriamente un tipo de elemento basado en la dificultad
        float random = Random.value;
        ElementBase elementToSpawn = null;
        
        // Ajustar probabilidades según dificultad
        float fruitProb = .9f - (difficultyLevel * 0.085f);
        float powerUpProb = 0.035f;
        
        if (random < fruitProb)
        {
            elementToSpawn = fruitPool[Random.Range(0, fruitPool.Count)];
        }
        else if (random < fruitProb + powerUpProb)
        {
            elementToSpawn = powerUpPool[Random.Range(0, powerUpPool.Count)];
        }
        else
        {
            elementToSpawn = obstaclePool[Random.Range(0, obstaclePool.Count)];
        }
        
        if (elementToSpawn != null && Random.value <= elementToSpawn.spawnProbability)
        {
            SpawnElement(elementToSpawn); // Genera el elemento con la configuración del scriptable
        }
    }
    
    private void SpawnElement(ElementBase data)
    {
        // Obtiene un objeto del pool y lo configura
        GameObject pooledObject = gameObjectPool.Get();
        ElementBehavior behavior = pooledObject.GetComponent<ElementBehavior>();
        if (pooledObject != null)
        {
            behavior.Initialize(data, gameObjectPool); // Inicializa el elemento con sus datos específicos
            ConfigurePosition(pooledObject); // Configura la posición inicial del spawn
        }
    }
    
    private void ConfigurePosition(GameObject obj)
    {
        // Configura una posición aleatoria entre los límites establecidos
        float randomX = Random.Range(borderLeft.position.x, borderRight.position.x);
        obj.transform.position = new Vector3(randomX, transform.position.y, 0);
    }
    
    private void CalculateNextSpawnTime()
    {
        // Calcula el próximo tiempo de spawn según el nivel de dificultad
        float spawnRate = spawnInterval * (1f - (difficultyLevel * 0.115f));
        nextSpawnTime = Time.time + spawnRate;
    }
}