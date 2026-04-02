using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerView : MonoBehaviour
{

    private Animator anim;


    void Awake()
    {
        anim = GetComponent<Animator>();
    }



    public  void UpdateAnimation(PlayerModel playermodel)
    {
        anim.SetFloat("Xvelocity", playermodel.Velocity.x);
        anim.SetFloat("Yvelocity", playermodel.Velocity.y);
        anim.SetBool("IsGrounded", playermodel.IsGrounded);
        anim.SetBool("IswallDetected", playermodel.IsWallDetected);
    }


    public void PlayKnockedAnimation()
    {
        anim.SetTrigger("Knocked");
    }

    public void PlayerDeath()
    {
        anim.SetTrigger("Death");
    }


}
