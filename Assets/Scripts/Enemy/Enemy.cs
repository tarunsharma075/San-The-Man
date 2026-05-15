using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float movementSpeed;
    private bool isFacingRight = false;

    protected Animator anim;
    protected Rigidbody2D rb;
    protected int facingDirection = -1;
    [SerializeField]protected float idleDuration;
    protected float idleTimer;

    [Header("Collision")]
    [SerializeField] protected float groundDistance;
    [SerializeField] protected float wallDistance;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected LayerMask groundLayer;
    
    protected bool IsGrounded;
    protected bool IsWallDetected;



    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }


    protected void CheckCollision()
    {
        IsGrounded = Physics2D.Raycast(groundCheck.transform.position, Vector2.down, groundDistance, groundLayer);
        IsWallDetected = Physics2D.Raycast(groundCheck.transform.position, Vector2.right * facingDirection, wallDistance, groundLayer);
    }


protected virtual void Update()
    {
        idleTimer -= Time.deltaTime;
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("The collision"+" "+collision.gameObject.layer);
    }



    protected void HandleFlip(float Xvalue)
    {
        if (Xvalue < 0 && isFacingRight || !isFacingRight && Xvalue > 0)
        {
            FlipPlayer();
            idleTimer = idleDuration;
        }
    }

    protected virtual void HandleMovement()
    {
        if (idleTimer > 0) return;

        if (IsGrounded)
        {
            rb.velocity = new Vector2(movementSpeed * facingDirection, rb.velocity.y);
        }
    }
    protected void FlipPlayer()
    {
        facingDirection = facingDirection * -1;
        this.transform.Rotate(0, 180, 0);
        isFacingRight = !isFacingRight;
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = IsGrounded ? Color.green : Color.red;

        Gizmos.DrawLine(
            groundCheck.transform.position,
            new Vector2(
                groundCheck.transform.position.x,
                groundCheck.transform.position.y - groundDistance
            )
        );

        Gizmos.color = IsWallDetected ? Color.green : Color.red;

        Gizmos.DrawLine(
            groundCheck.position,
            new Vector2(
                groundCheck.transform.position.x + (wallDistance * facingDirection),
                groundCheck.transform.position.y
            )
        );
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();

        if (player == null) return;


        ServiceLocator.Instance.playerService.TakeDamage(1);

    }
}
