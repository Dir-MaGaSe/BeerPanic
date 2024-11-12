using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motion : MonoBehaviour
{
    //Movimiento
    [Header("Configuracion")]
    [SerializeField] private float moveSpeed;
    [Range(0f, .5f)][SerializeField] private float smoothing;

    private Rigidbody2D rb;
    private float horizontalMove;
    private Vector3 speed = Vector3.zero;
    private bool isFlipped = false;

    //Animacion
    private Animator animator;

    private void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    private void Update() 
    {
        horizontalMove = (Input.GetAxisRaw("Horizontal") * moveSpeed) + GameManager.Instance.bonusSpeed;

        animator.SetFloat("Horizontal", Mathf.Abs(horizontalMove));
    }
    private void FixedUpdate() 
    {
        Move(horizontalMove * Time.fixedDeltaTime);
    }

    private void Move(float movement)
    {
        Vector3 targetSpeed = new Vector2(movement, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetSpeed, ref speed, smoothing);

        if (movement > 0 && isFlipped)
        {
            FlipPlayer();
        }
        else if (movement < 0 && !isFlipped)
        {
            FlipPlayer();
        }
    }

    private void FlipPlayer()
    {
        isFlipped = !isFlipped;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
