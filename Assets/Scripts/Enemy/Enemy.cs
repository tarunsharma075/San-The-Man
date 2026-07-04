using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float movementSpeed;
    [SerializeField] private bool isFacingRight = false;

    [NonSerialized]protected Animator anim;
    [NonSerialized] protected Rigidbody2D rb;
    [SerializeField] protected int facingDirection = -1;
    [SerializeField]protected float idleDuration;
    protected float idleTimer;

    [Header("Collision")]
    [SerializeField] protected float groundDistance;
    [SerializeField] protected float wallDistance;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected GameObject decreaseHealthSign;
    
    protected SpriteRenderer sr;

    protected bool IsGrounded;
    protected bool IsWallDetected;

    protected float enemyhealth = 3;
    protected float currentHealth;
    protected EnemyState currentState;
    



    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = enemyhealth;
        sr = GetComponent<SpriteRenderer>();
        currentState = EnemyState.Alive;
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
        else if (IsWallDetected)
        {
            FlipPlayer();
            idleTimer = idleDuration;

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


    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            
            ServiceLocator.Instance.playerService.TakeDamage();
        }


    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
     

        if (collision.gameObject.CompareTag("Player"))
        {
            ServiceLocator.Instance.playerService.TakeDamage();
        }

        if (collision.gameObject.CompareTag("PlayerBullet"))
        {

            TakeDamage();
        }
    }

    protected virtual void SetHealth(float newHealth) { 
    
      enemyhealth = newHealth;
        currentHealth = enemyhealth;
        
    }

    private void TakeDamage()
    {
       
        StunDamage();
        if (currentHealth <=0)
        {
           Destroy(this.gameObject);
        }

    }
    protected virtual void StunDamage()
    {
        ServiceLocator.Instance.playerService.EnemyOverStunJump();
       
        changeColourOnhit();
        currentHealth--;
        GameObject sign = Instantiate(
            decreaseHealthSign,
            transform.position,
            Quaternion.identity
        );

        
        
        StartCoroutine(DamageSignRoutine(sign));
    }

    private IEnumerator DamageSignRoutine(GameObject sign)
    {
        float timer = 0f;

        while (timer < 0.5f)
        {
            sign.transform.position += Vector3.up * 2f * Time.deltaTime;
            this.rb.velocity = Vector2.zero;
            anim.SetFloat("Xvelocity",0);

            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(sign);
        sr.color = Color.white;
    }


    public void changeColourOnhit()
    {
        ColorUtility.TryParseHtmlString("#FF0000", out Color hitcolor);
        sr.color = hitcolor;
    }


   
}
