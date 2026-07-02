using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OrcMonster : EnemyBehaviour
{
    private bool isplayerdetected;
    private OrcState currentState;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField]private float radius;
    [SerializeField] private BoxCollider2D HitBox;

    public enum  OrcState
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
        currentState = OrcState.Alive;
    }

 

    
    void Update()
    {
        if (ServiceLocator.Instance.playerService.GetPlayerState() == PlayerState.Dead)
        {
            anim.SetBool("FirstAttack", false);
            
            return;
        }
        if (currentState != OrcState.Dead && currentState != OrcState.Attacking)
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
            currentState = OrcState.Attacking;
            rb.velocity = Vector2.zero;
            anim.SetBool("FirstAttack", true);
        }
        else if(!isplayerdetected)
        {
            currentState = OrcState.Alive;
            anim.SetBool("FirstAttack", false);
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
        if (currentState != OrcState.Alive)
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
            //Debug.Log("Player Died");
        }
    }
}

