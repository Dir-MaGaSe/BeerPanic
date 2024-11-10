using UnityEngine;

public partial class ElementBehavior : MonoBehaviour
{
    private ElementBase elementData;
    private ObjectPool<GameObject> pool;
    private Rigidbody2D rb;
    
    public void Initialize(ElementBase data, ObjectPool<GameObject> objectPool)
    {
        elementData = data;
        pool = objectPool;
        rb = GetComponent<Rigidbody2D>();
        
        // Configurar velocidad de caída
        rb.velocity = Vector2.down * elementData.fallSpeed;
    }
    
    private void Update()
    {
        // Verificar si está fuera de la pantalla
        if (transform.position.y < -.1f) // Ajustar según necesidades
        {
            ReturnToPool();
        }
    }
    
    public void ReturnToPool()
    {
        if (pool != null)
        {
            pool.Return(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.collider.CompareTag("Ground"))
        {
            ReturnToPool();
        }
    }
}