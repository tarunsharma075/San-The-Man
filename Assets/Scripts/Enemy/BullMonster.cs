using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BullMonster : EnemyBehaviour
{
    private bool isplayerdetected;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField]private float radius;
    [SerializeField] private BoxCollider2D HitBox;
    

   

    private void Awake()
    {
       
        base.Awake();
        anim= GetComponentInChildren<Animator>();
        SetHealth(1);
        currentState = EnemyState.Alive;

        UpdateHealthUI();
        Debug.Log(currentHealth);
    }

 

    
    void Update()
    {
        if(currentState== EnemyState.Dead)
        {
            return;
        }
       
        if (currentState == EnemyState.Stunned)
        {
            anim.enabled = false;
            return;
        }
        else
        {
            anim.enabled= true;
        }
        if (ServiceLocator.Instance.playerService.GetPlayerState() == PlayerState.Dead)
        {
            anim.SetBool("Attack", false);
            
           
        }
        if (currentState != EnemyState.Dead && currentState != EnemyState.Attacking)
        {
            
            base.Update();
            CheckPlayer();
            Attack();
        }
        
      
        
       
       
    }


    private void CheckPlayer()
    {
        isplayerdetected = Physics2D.OverlapCircle(this.transform.position,
            radius,
            playerLayer);

    }

    private void Attack()
    {
        if (isplayerdetected)
        {
            currentState = EnemyState.Attacking;
            rb.velocity = Vector2.zero;
            anim.SetBool("Attack", true);
        }
        else if(!isplayerdetected)
        {
            currentState = EnemyState.Alive;
            anim.SetBool("Attack", false);
        }
        
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();  

        Gizmos.color = isplayerdetected ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    protected override void HandleMovement()
    {
        if (currentState != EnemyState.Alive)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        base.HandleMovement();
    }


    public void EnableHitBox()
    {
        HitBox.enabled = true;

       
    }

    public void DisableHitBox()
    {
        HitBox.enabled = false;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ServiceLocator.Instance.gamePlayservice.PlayerDead();

        }
        else
        {
            base.OnTriggerEnter2D (collision);
        }
    }

    //public void ChangeHealthUI()
    //{
    //    greenHealthBar.fillAmount = Mathf.Clamp(currentHealth / enemyhealth, 0, 1);
    //}
}

