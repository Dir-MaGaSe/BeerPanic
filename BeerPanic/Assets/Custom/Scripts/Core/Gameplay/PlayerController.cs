using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float baseMovementSpeed = 5f;
    [SerializeField] private float movementSmoothness = 0.1f;
    
    [Header("Colisión")]
    [SerializeField] private Vector2 baseCatchArea = new Vector2(1f, 0.5f);
    
    private Rigidbody2D rb;
    private BoxCollider2D catchCollider;
    private float currentMovementSpeed;
    private Vector2 movement;
    private bool isMovementModified;
    private float movementModifierEndTime;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        catchCollider = GetComponent<BoxCollider2D>();
        ResetModifiers();
    }
    
    private void Update()
    {
        float horizontalInput = 0;
        /*if (Input.GetKey(KeyCode.LeftArrow) || UIManager.Instance.IsLeftButtonPressed)
            horizontalInput = -1;
        else if (Input.GetKey(KeyCode.RightArrow) || UIManager.Instance.IsRightButtonPressed)
            horizontalInput = 1;
            
        movement = new Vector2(horizontalInput, 0);*/
        
        CheckModifiersExpiration();
    }
    
    private void FixedUpdate()
    {
        Vector2 targetVelocity = movement * currentMovementSpeed;
        rb.velocity = Vector2.Lerp(rb.velocity, targetVelocity, movementSmoothness);
    }
    
    public void ApplySpeedModifier(float multiplier, float duration)
    {
        currentMovementSpeed = baseMovementSpeed * multiplier;
        isMovementModified = true;
        movementModifierEndTime = Time.time + duration;
    }
    
    public void ApplyCatchAreaModifier(float multiplier, float duration)
    {
        catchCollider.size = baseCatchArea * multiplier;
        StartCoroutine(ResetCatchAreaAfterDuration(duration));
    }
    
    private IEnumerator ResetCatchAreaAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        catchCollider.size = baseCatchArea;
    }
    
    private void CheckModifiersExpiration()
    {
        if (isMovementModified && Time.time >= movementModifierEndTime)
        {
            ResetModifiers();
        }
    }
    
    private void ResetModifiers()
    {
        currentMovementSpeed = baseMovementSpeed;
        catchCollider.size = baseCatchArea;
        isMovementModified = false;
    }
}