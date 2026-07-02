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

    protected override void Awake()
    {
       
        base.Awake();
        timer = cooldownTime;
        attackHitbox.enabled = false;
        currentHealth = 3;
    }


    protected override void Update()
    {
      


        base.Update();
        if (ServiceLocator.Instance.playerService.GetPlayerState() == PlayerState.Dead) return;
        CheckPlayerCollision();
        Attack();
        chasePlayer();


    }
   

    public  void CheckPlayerCollision()
    {
      
        isPlayerDetected = Physics2D.Raycast(center.transform.position, 
            Vector2.right * facingDirection, 
            playerDistance, 
            playerLayer);
       

    }


    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = isPlayerDetected ? Color.green : Color.red;
        Gizmos.DrawLine(center.transform.position, new Vector2(center.transform.position.x + (facingDirection * playerDistance), 
            center.transform.position.y));

       ;


    }

    private void Attack()
    {
        if (isPlayerDetected)
        {
            timer -= Time.deltaTime;

            Transform player = ServiceLocator.Instance.playerService.GetPlayer().transform;

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
        if (!shouldcchasePlayer || ServiceLocator.Instance==null|| IsGrounded) return;

        Vector2 targetPosition = new Vector2(ServiceLocator.Instance.playerService.GetPlayer().transform.position.x
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
            
            anim.SetTrigger("Stun");
            changeColourOnhit();
            ServiceLocator.Instance.audioService.PlaySFX(SoundTypes.EnemyStun);
            this.gameObject.GetComponent<AttackingMushroom>().enabled= false;
            stunCollider.enabled = false;
            attackHitbox.enabled = false;
            this.gameObject.layer = LayerMask.NameToLayer("StunnedEnemy");



        }
       
    }

   

}


