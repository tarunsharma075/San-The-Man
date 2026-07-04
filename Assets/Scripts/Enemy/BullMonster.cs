using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BullMonster : EnemyBehaviour
{
    private bool isplayerdetected;
    private BullState currentState;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField]private float radius;
    [SerializeField] private BoxCollider2D HitBox;
    

    public enum  BullState
    {
        Dead,
        Alive,
        Attacking,
        
        
    }

    private void Awake()
    {
       
        base.Awake();
        anim= GetComponentInChildren<Animator>();
        SetHealth(5);
        currentState = BullState.Alive;
        //greenHealthBar.fillAmount = Mathf.Clamp(currentHealth / enemyhealth, 0, 1);
        //redHealthBar.fillAmount = Mathf.Clamp(currentHealth/enemyhealth,0,1);
    }

 

    
    void Update()
    {
        if (ServiceLocator.Instance.playerService.GetPlayerState() == PlayerState.Dead)
        {
            anim.SetBool("Attack", false);
            
           
        }
        if (currentState != BullState.Dead && currentState != BullState.Attacking)
        {
            
            base.Update();
        }
        
        CheckPlayer();
        Attack();
        
       
       
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
            currentState = BullState.Attacking;
            rb.velocity = Vector2.zero;
            anim.SetBool("Attack", true);
        }
        else if(!isplayerdetected)
        {
            currentState = BullState.Alive;
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
        if (currentState != BullState.Alive)
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
    }

    //public void ChangeHealthUI()
    //{
    //    greenHealthBar.fillAmount = Mathf.Clamp(currentHealth / enemyhealth, 0, 1);
    //}
}

