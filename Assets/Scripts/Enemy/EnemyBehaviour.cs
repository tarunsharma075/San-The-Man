using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour :Enemy
{
    
   protected override void Awake()
    {
        base.Awake();
    }


    protected override void Update()
    {
        CheckPlayerState1();
        base.Update();
        HandleMovement();
        anim.SetFloat("Xvelocity", rb.velocity.x);
        CheckCollision();
        if (!IsGrounded || IsWallDetected)
        {
            if (idleTimer <= 0) // Only flip if not already in idle
            {
                FlipPlayer();
                idleTimer = idleDuration;
                rb.velocity = Vector2.zero;
            }
        }

    }

    private static void CheckPlayerState1()
    {

        PlayerState state = ServiceLocator.Instance.playerService.GetPlayerState();

        if(state==PlayerState.NotSpwaned|| state == PlayerState.Dead)
        {
            return;
        }
    }
}
