using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMushroom :Enemy
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


    private void HandleMovement()
    {
        if (idleTimer > 0) return;

        if (IsGrounded)
        {
            rb.velocity = new Vector2(movementSpeed * facingDirection, rb.velocity.y);
        }
    }

}
