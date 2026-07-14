using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : GenericMonoSingleton<Enemy>
{
    [SerializeField] protected float movementSpeed;
    [SerializeField] private bool isFacingRight = false;

    [NonSerialized]protected Animator anim;
    [NonSerialized] protected Rigidbody2D rb;
    [SerializeField] protected int facingDirection;
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

    protected float enemyMaxhealth = 3;
    protected float currentHealth;
    protected EnemyState currentState;
    protected SpriteRenderer currentEnemySprite;



   [SerializeField] protected Image greenhealthbar;

    [SerializeField] protected Image redHeathbar;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = enemyMaxhealth;
        sr = GetComponent<SpriteRenderer>();
        currentState = EnemyState.Alive;
        currentEnemySprite = this.gameObject.GetComponent<SpriteRenderer>();
    }


    protected void CheckCollision()
    {
        IsGrounded = Physics2D.Raycast(groundCheck.transform.position, Vector2.down, groundDistance, groundLayer);
        IsWallDetected = Physics2D.Raycast(groundCheck.transform.position, Vector2.right * facingDirection, wallDistance, groundLayer);
    }

    protected bool TryGetDetectedPlayer(float detectionRadius, LayerMask playerLayer, out Transform detectedPlayer)
    {
        return TryGetDetectedPlayer(transform.position, detectionRadius, playerLayer, out detectedPlayer);
    }

    protected bool TryGetDetectedPlayer(Vector2 detectionCenter, float detectionRadius, LayerMask playerLayer, out Transform detectedPlayer)
    {
        Collider2D detectedCollider = Physics2D.OverlapCircle(detectionCenter, detectionRadius, playerLayer);
        detectedPlayer = detectedCollider != null ? detectedCollider.transform : null;
        return detectedPlayer != null;
    }

    protected void FaceTarget(Transform target)
    {
        if (target == null)
        {
            return;
        }

        float directionToTarget = target.position.x - transform.position.x;
        if (Mathf.Abs(directionToTarget) < 0.05f)
        {
            return;
        }

        bool shouldFaceRight = directionToTarget > 0;
        if (shouldFaceRight && facingDirection < 0 || !shouldFaceRight && facingDirection > 0)
        {
            FlipPlayer();
        }
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
            Debug.Log(gameObject.name);
        }

        if (collision.gameObject.CompareTag("Shuriken"))
        {
            currentHealth--;
            UpdateGreenHealthbar();
            if (collision.CompareTag("Shuriken"))
            {
               
                UpdateGreenHealthbar();

                if (currentHealth <= 0)
                {
                    
                    BossEnd();      
                }
                else
                {
                   
                    StartCoroutine(BossHitWithShuriken());
                }
            }
        }

    }



    private IEnumerator BossHitWithShuriken()
    {
        sr.color= Color.red;
        currentState = EnemyState.Stunned;
        this.rb.velocity= Vector2.zero;
        yield return new WaitForSeconds(0.5f);
        sr.color = Color.white;
        currentState = EnemyState.Alive;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
     

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log(this.gameObject.name);
            ServiceLocator.Instance.playerService.TakeDamage();
        }

        if (collision.gameObject.CompareTag("PlayerBullet"))
        {

            TakeDamage();
        }
    }

    protected virtual void SetHealth(float newHealth) { 
    
      enemyMaxhealth = newHealth;
        currentHealth = enemyMaxhealth;
        
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
        Debug.Log("Enter in damage sign routine");
        float timer = 0f;

        while (timer < 0.5f)
        {
            sign.transform.position += Vector3.up * 2f * Time.deltaTime;
            rb.velocity = Vector2.zero;
            anim.SetFloat("Xvelocity", 0);

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

    protected void BossEnd()
    {


        currentState = EnemyState.Dead;
        UpdateGreenHealthbar();
       
        StartCoroutine(BossEnemyEnd());
           

        
    }

    private IEnumerator BossEnemyEnd()
    {
        
        
        sr.color= Color.gray;
        ServiceLocator.Instance.playerService.SetCurrentDirectionSign(PlayerDirection.Right);
        Debug.Log("called by bossenemyend");
        rb.velocity = Vector2.up * 2;
        anim.enabled = false;
        yield return new WaitForSeconds(1f);
        ServiceLocator.Instance.playerService.DeactivateCurrentActiveSign();
        rb.velocity = Vector2.down * 10;
        yield return new WaitForSeconds(3f);
      
        Destroy(this.gameObject);
    }


    protected void UpdateHealthUI()
    {
        greenhealthbar.fillAmount= Mathf.Clamp(currentHealth / enemyMaxhealth, 0, 1);
        redHeathbar.fillAmount = Mathf.Clamp(currentHealth / enemyMaxhealth, 0, 1);
    }

    protected void UpdateGreenHealthbar() => greenhealthbar.fillAmount = Mathf.Clamp(currentHealth / enemyMaxhealth, 0, 1);

    

}
