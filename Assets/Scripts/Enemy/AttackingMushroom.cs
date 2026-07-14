using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingMushroom : EnemyBehaviour
{
    [SerializeField] private float playerDistance;
    private bool isPlayerDetected;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform center;
    private float timer = 0;
    [SerializeField] protected float cooldownTime;
    [SerializeField] private Collider2D attackHitbox;
    [SerializeField] private Collider2D stunCollider;
    private bool shouldcchasePlayer = false;
    private Transform detectedPlayer;
    
    protected override void Awake()
    {
       
        base.Awake();
        timer = cooldownTime;
        attackHitbox.enabled = false;
       
    }


    protected override void Update()
    {

        if (currentState == EnemyState.Dead) return;

        base.Update();
        if (ServiceLocator.Instance.playerService.GetPlayerState() == PlayerState.Dead) return;
        CheckPlayerCollision();
        Attack();
        chasePlayer();


    }
   

    public  void CheckPlayerCollision()
    {
        Vector2 detectionCenter = center != null ? center.position : transform.position;
        isPlayerDetected = TryGetDetectedPlayer(detectionCenter, playerDistance, playerLayer, out detectedPlayer);

        if (isPlayerDetected)
        {
            FaceTarget(detectedPlayer);
        }

    }


    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = isPlayerDetected ? Color.green : Color.red;
        Vector3 detectionCenter = center != null ? center.position : transform.position;
        Gizmos.DrawWireSphere(detectionCenter, playerDistance);

       ;


    }

    private void Attack()
    {
        if (isPlayerDetected)
        {
            timer -= Time.deltaTime;

            Transform player = detectedPlayer != null
                ? detectedPlayer
                : ServiceLocator.Instance.playerService.GetPlayer().transform;

            float distanceToPlayer = Mathf.Abs(player.position.x - transform.position.x);

           

            if (distanceToPlayer > 2f)
            {
                shouldcchasePlayer = true;
               
            }
            else
            {
                shouldcchasePlayer = false;

                this.rb.velocity= Vector2.zero;
                anim.SetFloat("Xvelocity", 0);

                if (timer <= 0)
                {
                    anim.SetTrigger("Attack");
                    timer = cooldownTime;



                }
            }
            
          ;
        }
     
    }



    public void EnableAttackHitbox()
    {
       
        attackHitbox.enabled = true;
        Debug.Log("attack hitbox enabled");
    }

    public void DisableAttackHitbox()
    {
        
        attackHitbox.enabled = false;
        Debug.Log("attack hitbox disabled");
    }

    protected override  void  OnTriggerEnter2D(Collider2D collision)
    {



        if (collision.CompareTag("Player") && collision.IsTouching(stunCollider))
        {
            if (collision.GetComponent<PlayerController>().transform.position.y > stunCollider.transform.position.y)
            {
                Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
                if (rb.velocity.y<0)
                {
                    ServiceLocator.Instance.audioService.PlaySFX(SoundTypes.EnemyOverJump);
                    StunDamage();
                }
            }
        }
        else
        {
            base.OnTriggerEnter2D (collision);
        }
        

    }



    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }

    private void chasePlayer()
    {
        if (!shouldcchasePlayer || ServiceLocator.Instance==null || detectedPlayer == null) return;
        if (!IsGrounded)
        {
            FlipPlayer();
        }

        Vector2 targetPosition = new Vector2(detectedPlayer.position.x
            , rb.position.y);

        rb.MovePosition(Vector2.MoveTowards(
            rb.position,
            targetPosition,
            movementSpeed * Time.fixedDeltaTime
        ));
    }
    protected override void StunDamage()
    {
        base.StunDamage();
        if (currentHealth <= 0)
        {
            ServiceLocator.Instance.gamePlayservice.IncreaseShurikenNumberByValue(2);

            StartCoroutine(MushroomEnd());



        }
       
    }

    private IEnumerator MushroomEnd()
    {
        currentState = EnemyState.Dead;
        this.rb.velocity = Vector2.zero;
        anim.SetTrigger("Stun");
        changeColourOnhit();
        ServiceLocator.Instance.audioService.PlaySFX(SoundTypes.EnemyStun);
        yield return new WaitForSecondsRealtime(0.5f);
        anim.SetTrigger("Die");
        stunCollider.enabled = false;
        attackHitbox.enabled = false;
        yield return new WaitForSecondsRealtime(0.5f);
        Destroy(this.gameObject);
        
         
    }
   

}


