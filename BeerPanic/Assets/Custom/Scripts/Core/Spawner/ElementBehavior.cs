using System.Collections;
using UnityEngine;

public class ElementBehavior : MonoBehaviour
{
    public ElementBase elementData; // Almacena el scriptable asociado
    private Rigidbody2D rb; // Componente de física
    private SpriteRenderer imageRender; // Render para el sprite del elemento

    private IEnumerator corutinePowerUp;
    
    private void OnEnable() 
    {
        // Inicializa el elemento cuando se activa
        if (elementData != null) { Initialize(elementData); }
    }

    public void Initialize(ElementBase data)
    {
        // Configura el elemento según el scriptable recibido
        rb = GetComponent<Rigidbody2D>();
        imageRender = GetComponent<SpriteRenderer>();
        
        imageRender.sprite = data.elementSprite; // Asigna el sprite del scriptable
        elementData = data; // Guarda la referencia al scriptable para uso futuro
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
            if (elementData.elementType == ElementBase.ElementType.Fruit)
            {
                GameManager.Instance.AddPoints(elementData.basePoints);
            }
            if (elementData.elementType == ElementBase.ElementType.PowerUp)
            {
                /*PowerUpElement newPowerUp = elementData;
                if(corutinePowerUp == null)
                {
                    corutinePowerUp = PlayerSpeedBonusCoroutine(elementData.)
                }*/
            }
            if (elementData.elementType == ElementBase.ElementType.Obstacle)
            {
                GameManager.Instance.AddPoints(elementData.basePoints);


            }
        }
        
        if (!other.gameObject.CompareTag("SpawnObject"))
        {
            this.gameObject.SetActive(false);
        }
    }

    private IEnumerator PlayerSpeedBonusCoroutine(float bonus, float duration)
    {
        GameManager.Instance.CalculateSpeedBonus(bonus, true);
        yield return new WaitForSeconds(duration);
        GameManager.Instance.CalculateSpeedBonus(bonus, false);
    }
}