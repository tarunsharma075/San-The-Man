using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerView : MonoBehaviour
{

    private Animator anim;
    private PlayerController playerController;

    void Awake()
    {
        anim = GetComponent<Animator>();
        playerController = GetComponentInParent<PlayerController>();
    }



    public  void UpdateAnimation(PlayerModel playermodel)
    {
        anim.SetFloat("Xvelocity",
        playermodel.CurrentPlayerState == PlayerState.PlayerGrounded ? playermodel.Velocity.x : 0
 );
        anim.SetFloat("Yvelocity", playermodel.Velocity.y);
        anim.SetBool("IsGrounded", playermodel.CurrentPlayerState == PlayerState.PlayerGrounded);
       
        anim.SetBool("IswallDetected", playermodel.CurrentPlayerState==PlayerState.WallSliding);
        
    }


    public void PlayKnockedAnimation(PlayerState currentplayerstate)
    {
        anim.SetBool("Knocked", currentplayerstate==PlayerState.PlayerKnocked);
    }

    //public void PlayerDeath()
    //{
    //    anim.SetTrigger("Death");
    //}

   public void Attack()
    {
        anim.SetTrigger("Attack");  
    }
    public void SpawnBullet()
    {
        playerController.FireBullet();
    }

}
