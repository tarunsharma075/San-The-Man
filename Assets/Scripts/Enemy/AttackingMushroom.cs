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
    private bool shouldcchasePlayer = false;

    protected override void Awake()
    {
       
        base.Awake();
        timer = cooldownTime;
        attackHitbox.enabled = false;
        
    }


    protected override void Update()
    {
      


        base.Update();
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
        Debug.Log("Attack hitbox enabled");
        attackHitbox.enabled = true;
    }

    public void DisableAttackHitbox()
    {
        Debug.Log("Attack hitbox disabled");
        attackHitbox.enabled = false;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        

        base.OnTriggerEnter2D(collision);
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

}


