using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    private Rigidbody2D rb;


    ////PlayerMovement
    //[SerializeField] private float speed;
    //[SerializeField] private float jumpForce;
    //[SerializeField] private float doubleJump;
    //float Xinput = 0;
    //float Yinput = 0;

    ////PlayerStates

    //private float facingDirection = 1f;

    ////CheckCollision
    //[SerializeField]private float groundCheckDistance;
    //[SerializeField]private float wallCheckDistance;
    //[SerializeField] private LayerMask groundLayer;
    //[SerializeField] private bool isGrounded;
    //[SerializeField] private bool isWallDetected;



    //// Wall Interaction

    //[SerializeField] private float walljumpduration = 0.6f;
    //[SerializeField] private Vector2 walljumpForce;
    //[SerializeField] private bool isWallJumping;


    ////knocking


    //[SerializeField] private bool isknocked;
    //[SerializeField] private bool canKnocked;
    //[SerializeField] private Vector2 knockedDistance;
    //[SerializeField] private float knockedDuration;

    ////cyotejumping

    //[SerializeField] private float CyotejumpWindow = 0.5f;
    //private float cyoteJumpActivated = -1;


    [SerializeField] private LayerMask groundLayer;
   [SerializeField] private PlayerModel playermodel;
    private PlayerView playerView;

    private void Awake()
    {
        
        playerView = playerView = GetComponentInChildren<PlayerView>();
        rb = this.GetComponent<Rigidbody2D>();

    }
    void Start()
    {
        
    }

    
    void Update()
    {
 if (playermodel.IsKnocked)
            return;
    

        Inputhandling();
        CheckCollision();
        HandleWallSlide();
        HandleAirborne();
        HandleFlip();
        playermodel.Velocity= rb.velocity;
        playerView.UpdateAnimation(playermodel);
        
    }

    private void HandleWallSlide()
    {
        bool canWeSlide = playermodel.IsWallDetected&& rb.velocity.y < 0;
        if (!canWeSlide)
            return;

        float yModifer= playermodel.YInput < 0 ? 0.5f :1 ;

        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * yModifer);
    }

    private void Inputhandling()
    {
         playermodel.XInput = Input.GetAxisRaw("Horizontal");
        playermodel.YInput = Input.GetAxisRaw("Vertical");
        PlayerMovement(playermodel.XInput);
        if (Input.GetKeyDown(KeyCode.Space))
        {
           JumpButton();

        }
       
    }

    private  void  JumpButton()

    {
        
        if (playermodel.IsGrounded)
        {
            
            PlayerJump();
        }
        else if (playermodel.IsWallDetected&& !playermodel.IsGrounded) {

            WallJump();
        
        }



        else if (playermodel.CanDoubleJump)
        {
            DoubleJump();
            playermodel.CanDoubleJump= false;
        }
    }

    private void HandleAirborne()
    {
        if (playermodel.IsGrounded && playermodel.IsAirborne)
        {
            playermodel.IsAirborne = false;
            playermodel.CanDoubleJump= true;
        }
        else if(!playermodel.IsGrounded && !playermodel.IsAirborne)
        {
            playermodel.IsAirborne = true;


            
        }
    }

    private void PlayerMovement(float xinput)
    {
        if (playermodel.IsWallDetected)
            return;

        if (playermodel.IsWallJumping)

            return;
        rb.velocity = new Vector2(xinput * playermodel.Speed ,rb.velocity.y);
    }

    private void PlayerJump() => rb.velocity = new Vector2(rb.velocity.x, playermodel.JumpForce);

    private void CheckCollision()
    {
        playermodel.IsGrounded = Physics2D.Raycast(this.transform.position, Vector2.down, playermodel.GroundCheckDist, groundLayer);
        playermodel.IsWallDetected= Physics2D.Raycast(this.transform.position, Vector2.right * playermodel.FacingDirection, playermodel.WallCheckDist,groundLayer);
    }

    private void DoubleJump()
    {
        StopCoroutine(wallJumpRoutine());
        playermodel.IsWallJumping= false;
        playermodel.CanDoubleJump = false;
        rb.velocity = new Vector2(rb.velocity.x, playermodel.DoubleJumpForce);
    }

    private void WallJump()
    {
        playermodel.CanDoubleJump = true;
        rb.velocity = new Vector2(playermodel.WallJumpForce.x*-playermodel.FacingDirection, playermodel.WallJumpForce.y);
        FlipPlayer();
        StopAllCoroutines();
        StartCoroutine(wallJumpRoutine());

    }

    //private void ActivateCyoteJump()=> cyoteJumpActivated = Time.time;
    //private void CancelCyoyeJump() => cyoteJumpActivated = Time.time - 1;



    private IEnumerator wallJumpRoutine()
    {
        playermodel.IsWallJumping = true;
        yield  return new WaitForSeconds(playermodel.WallJumpDuration);
        playermodel.IsWallJumping = false;
    }


    private void HandleFlip()
    {
        if(playermodel.XInput<0 && playermodel.IsFacingRight  || !playermodel.IsFacingRight && playermodel.XInput > 0)
        {
            FlipPlayer();
        }
    }
   private void FlipPlayer()
    {
        playermodel.FacingDirection = playermodel.FacingDirection* -1;
       this.transform.Rotate(0, 180, 0);
        playermodel.IsFacingRight = !playermodel.IsFacingRight;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color= playermodel.IsGrounded ? Color.green : Color.red;
        Gizmos.color= playermodel.IsWallDetected ? Color.green: Color.red;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - playermodel.GroundCheckDist));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (playermodel.WallCheckDist * playermodel.FacingDirection), transform.position.y));


    }


    public  void PlayerKnockBack()
    {
        StartCoroutine(KnockBackRoutine());
        playerView.PlayKnockedAnimation();
        rb.velocity = new Vector2(playermodel.KnockbackDistance.x * -playermodel.FacingDirection, playermodel.KnockbackDistance.y);

    }

    private IEnumerator KnockBackRoutine()
    {
        playermodel.IsKnocked = true;
        playermodel.CanBeKnocked = false;
        
        yield return new WaitForSeconds(playermodel.KnockbackDuration);
        playermodel.IsKnocked = false;
        playermodel.CanBeKnocked = true;
        
    }

}

