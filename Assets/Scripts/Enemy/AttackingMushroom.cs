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
                Vector2 targetPosition = new Vector2(
                    player.position.x,
                    rb.position.y
                );

                rb.MovePosition(Vector2.MoveTowards(
                    rb.position,
                    targetPosition,
                    movementSpeed * Time.deltaTime*2
                ));
            }
            else
            {
                this.rb.velocity= Vector2.zero;
                anim.SetFloat("Xvelocity", 0);

                if (timer <= 0)
                {
                    anim.SetTrigger("Attack");
                    timer = cooldownTime;



                }
            }
            
            //Debug.Log("X Velocity: " + rb.velocity.x);
            //Debug.Log("Distance to Player: " + distanceToPlayer);
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

}


