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
         base.Update();
        HandleMovement();
        anim.SetFloat("Xvelocity", rb.velocity.x);
        CheckCollision();
        if (!IsGrounded || IsWallDetected)
        {
            FlipPlayer();
            idleTimer = idleDuration;
            rb.velocity = Vector2.zero;
        }
       
    }

    // just for test this comment is useless






}
