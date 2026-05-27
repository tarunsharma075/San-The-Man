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
        anim.SetFloat("Xvelocity", playermodel.Velocity.x);
        anim.SetFloat("Yvelocity", playermodel.Velocity.y);
        anim.SetBool("IsGrounded", playermodel.IsGrounded);
        anim.SetBool("IswallDetected", playermodel.IsWallDetected);
    }


    public void PlayKnockedAnimation(PlayerModel playermodel)
    {
        anim.SetBool("Knocked", playermodel.IsKnocked==true);
    }

    public void PlayerDeath()
    {
        anim.SetTrigger("Death");
    }

   public void Attack()
    {
        anim.SetTrigger("Attack");  
    }
    public void SpawnBullet()
    {
        playerController.FireBullet();
    }

}
