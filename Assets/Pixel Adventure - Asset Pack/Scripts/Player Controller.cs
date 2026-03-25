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


    //knocking


    [SerializeField] private bool isknocked;
    [SerializeField] private bool canKnocked;
    [SerializeField] private Vector2 knockedDistance;
    [SerializeField] private float knockedDuration;

    //cyotejumping

    [SerializeField] private float CyotejumpWindow = 0.5f;
    private float cyoteJumpActivated = -1;


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
 if (isknocked)
            return;


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
        bool cyotejumpAvliable = Time.time <cyoteJumpActivated + CyotejumpWindow; 

        if (isGrounded || cyotejumpAvliable)
        {
            if (cyotejumpAvliable)
            {
                Debug.Log("cyote jump is used");
            }
            PlayerJump();
        }
        else if (isWallDetected&& !isGrounded) {

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


            if (rb.velocity.y <= 0)
            {
                Debug.Log("cyote jump activated");
                ActivateCyoteJump();
                
            }
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
        StopAllCoroutines();
        StartCoroutine(wallJumpRoutine());

    }

    private void ActivateCyoteJump()=> cyoteJumpActivated = Time.time;
    private void CancelCyoyeJump() => cyoteJumpActivated = Time.time - 1;



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


    public  void PlayerKnockBack()
    {
        StartCoroutine(KnockBackRoutine());
        anim.SetTrigger("Knocked");
        rb.velocity = new Vector2(knockedDistance.x * -facingDirection, knockedDistance.y);

    }

    private IEnumerator KnockBackRoutine()
    {
        isknocked = true;
        canKnocked = false;
        yield return new WaitForSeconds(knockedDuration);
        isknocked = false;
        canKnocked = true;
    }

}

