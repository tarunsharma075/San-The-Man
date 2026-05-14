using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerModel 
{

    [Header("Movement Settings")]
    public float Speed = 5f;
    public float JumpForce = 12f;
    public float DoubleJumpForce = 10f;

    [Header("Wall Settings")]
    public float WallJumpDuration = 0.6f;
    public Vector2 WallJumpForce = new Vector2(7f, 14f);

    [Header("Collision Settings")]
    public float GroundCheckDist = 0.6f;
    public float WallCheckDist = 0.5f;
    

    [Header("Knockback Settings")]
    public Vector2 KnockbackDistance = new Vector2(7f, 5f);
    public float KnockbackDuration = 0.4f;





     public bool IsGrounded { get; set; }
     public bool IsWallDetected { get; set; }
    public bool IsAirborne { get; set; }

     public bool IsFacingRight { get; set; } = true;
     public float FacingDirection { get; set; } = 1f;

    public bool CanDoubleJump { get; set; } = true;
    public bool IsWallJumping { get; set; }
    public float XInput { get; set; }   
    public float YInput { get; set; }

    

    public bool IsKnocked { get; set; }
    public bool CanBeKnocked { get; set; } = true;

    public Vector2 Velocity { get; set; }


}
