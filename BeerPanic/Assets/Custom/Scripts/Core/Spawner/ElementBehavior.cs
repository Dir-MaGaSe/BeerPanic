using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class ElementBehavior : MonoBehaviour
{
    private ElementBase elementData; // Almacena el scriptable asociado
    private Rigidbody2D rb; // Componente de física
    private SpriteRenderer imageRender; // Render para el sprite del elemento

    private AudioClip soundEffect; // Efecto de sonido
    private ObjectPool<GameObject> pool;

    private IEnumerator effectCoroutine; //Corutina para el manejo de powerups y obstaculos
    
    private void OnEnable() 
    {
        // Inicializa el elemento cuando se activa
        if (elementData != null) { Initialize(elementData, pool); }
    }

    public void Initialize(ElementBase data, ObjectPool<GameObject> objectPool)
    {
        rb = GetComponent<Rigidbody2D>();
        imageRender = GetComponent<SpriteRenderer>();

        pool = objectPool; // Almacena la referencia de la pool
        elementData = data; // Guarda la referencia al scriptable
        
        imageRender.sprite = data.elementSprite; // Asigna el sprite del scriptable
        soundEffect = data.elementAudio; // Almacena el efecto de sonido del scriptable

        effectCoroutine = null;
    }
    
    private void FixedUpdate() 
    {
        // Aplica la velocidad de caída definida en el scriptable
        rb.velocity = Vector2.down * elementData.fallSpeed;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Realiza las acciones necesarias según el tipo de elemento
            switch (elementData)
            {
                case PowerUpElement powerUp:
                    if(effectCoroutine == null)
                    {
                        effectCoroutine = ApplyEffectsCoroutine(powerUp.effectDuration, powerUp.effectMultiplier);
                        StartCoroutine(effectCoroutine);
                    }
                    else
                    {
                        StopCoroutine(effectCoroutine);
                        effectCoroutine = ApplyEffectsCoroutine(powerUp.effectDuration, powerUp.effectMultiplier);
                        StartCoroutine(effectCoroutine);
                    }

                    break;
                    
                case ObstacleElement obstacle:
                    if(effectCoroutine == null)
                    {
                        effectCoroutine = ApplyEffectsCoroutine(obstacle.effectDuration, obstacle.penaltyMultiplier);
                        StartCoroutine(effectCoroutine);
                    }
                    else
                    {
                        StopCoroutine(effectCoroutine);
                        effectCoroutine = ApplyEffectsCoroutine(obstacle.effectDuration, obstacle.penaltyMultiplier);
                        StartCoroutine(effectCoroutine);
                    }

                    GameManager.Instance.AddPoints(obstacle.basePoints);
                    GameManager.Instance.TakeDamage(1);
                    
                    break;

                case FruitElement fruit:
                    GameManager.Instance.AddPoints(fruit.basePoints);
                    break;

                default:
                    Debug.LogWarning("Tipo de elemento no reconocido");
                    break;
            }

            AudioManager.Instance.PlayEffect(soundEffect);
        }
        
        if (!other.gameObject.CompareTag("SpawnObject"))
        {
            ReturnToPool();
        }
    }

    private IEnumerator ApplyEffectsCoroutine(float duration, float multiplier)
    {
        GameManager.Instance.CalculateSpeedBonus(multiplier, true);
        yield return new WaitForSeconds(duration);
        GameManager.Instance.CalculateSpeedBonus(multiplier, false);
        effectCoroutine = null;
    }

    // Función para devolver el objeto a la pool
    private void ReturnToPool()
    {
        if (pool != null)
        {
            pool.Release(gameObject);
        }
        else
        {
            Debug.LogWarning("Pool no asignada para este objeto");
        }
    }
}