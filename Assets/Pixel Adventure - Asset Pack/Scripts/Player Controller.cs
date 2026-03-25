using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    private Rigidbody2D rb;
    private Animator anim;

    //PlayerMovement
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float doubleJump;
    float Xinput = 0;
    float Yinput = 0;

    //PlayerStates
    [SerializeField] private bool isFacingRight = true;
    [SerializeField] private bool isAirBorne = false;
    [SerializeField] private bool canDoubleJump = true;
    private float facingDirection = 1f;

    //CheckCollision
    [SerializeField]private float groundCheckDistance;
    [SerializeField]private float wallCheckDistance;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isWallDetected;



    // Wall Interaction

    [SerializeField] private float walljumpduration = 0.6f;
    [SerializeField] private Vector2 walljumpForce;
    [SerializeField] private bool isWallJumping;





    private void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
        anim=  this.GetComponentInChildren<Animator>();
    }
    void Start()
    {
        
    }

    
    void Update()
    {
        CheckCollision();
        Inputhandling();
        HandleWallSlide();
        HandleAirborne();
        HandleFlip();
        
        PlayerAnimation();
    }

    private void HandleWallSlide()
    {
        bool canWeSlide = isWallDetected && rb.velocity.y < 0;
        if (!canWeSlide)
            return;

        float yModifer= Yinput < 0 ? 0.5f :1 ;

        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * yModifer);
    }

    private void Inputhandling()
    {
         Xinput = Input.GetAxisRaw("Horizontal");
        Yinput = Input.GetAxisRaw("Vertical");
        PlayerMovement(Xinput);
        if (Input.GetKeyDown(KeyCode.Space))
        {
           JumpButton();

        }
       
    }

    private  void  JumpButton()
    {
        if (isGrounded)
        {
            PlayerJump();
        }
        else if (isWallDetected) {

            WallJump();
        
        }



        else if (canDoubleJump)
        {
            DoubleJump();
            canDoubleJump = false;
        }
    }

    private void HandleAirborne()
    {
        if (isGrounded && isAirBorne)
        {
            isAirBorne = false;
            canDoubleJump = true;
        }
        else if(!isGrounded && !isAirBorne)
        {
            isAirBorne = true;
        }
    }

    private void PlayerMovement(float xinput)
    {
        if (isWallDetected)
            return;

        if (isWallJumping)

            return;
        rb.velocity = new Vector2(xinput * speed, rb.velocity.y);
    }

    private void PlayerJump() => rb.velocity = new Vector2(rb.velocity.x, jumpForce);

    private void CheckCollision()
    {
        isGrounded = Physics2D.Raycast(this.transform.position, Vector2.down, groundCheckDistance, groundLayer);
        isWallDetected= Physics2D.Raycast(this.transform.position, Vector2.right * facingDirection, wallCheckDistance, groundLayer);
    }

    private void DoubleJump()
    {
        isWallJumping = false;
        canDoubleJump = false;
        rb.velocity = new Vector2(rb.velocity.x, doubleJump);
    }

    private void WallJump()
    {
        canDoubleJump = true;
        rb.velocity = new Vector2(walljumpForce.x*-facingDirection, walljumpForce.y);
        FlipPlayer();
        StartCoroutine(wallJumpRoutine());

    }


    private IEnumerator wallJumpRoutine()
    {
        isWallJumping = true;
        yield  return new WaitForSeconds(walljumpduration);
        isWallJumping = false;
    }
    private void PlayerAnimation()
    {

        anim.SetFloat("Xvelocity", rb.velocity.x);
        anim.SetFloat("Yvelocity", rb.velocity.y);
        anim.SetBool("IsGrounded", isGrounded);
        anim.SetBool("IswallDetected", isWallDetected);
    }


    private void HandleFlip()
    {
        if(Xinput<0 && isFacingRight  || !isFacingRight && Xinput > 0)
        {
            FlipPlayer();
        }
    }
   private void FlipPlayer()
    {
        facingDirection = facingDirection * -1;
       this.transform.Rotate(0, 180, 0);
        isFacingRight = !isFacingRight;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawRay(this.transform.position, Vector2.down * groundCheckDistance);
        Gizmos.DrawRay(this.transform.position, Vector2.right * facingDirection * wallCheckDistance);

    }

}

