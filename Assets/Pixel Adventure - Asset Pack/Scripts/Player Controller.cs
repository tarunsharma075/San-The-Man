using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    private Rigidbody2D rb;
    private Animator anim;

    //PlayerMovement
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float doubleJump;

    //PlayerStates
    [SerializeField] private bool isFacingRight = true;
    [SerializeField] private bool isAirBorne = false;
    [SerializeField] private bool canDoubleJump = true;

    //CheckCollision
    [SerializeField]private float groundCheckDistance;
    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded;
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
        HandleAirborne();
        HandleFlip();
        PlayerAnimation();
    }

    private void Inputhandling()
    {
        float Xinput = Input.GetAxisRaw("Horizontal");
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
        }else if (canDoubleJump)
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

    private void PlayerMovement(float xinput)=> rb.velocity = new Vector2(xinput * speed, rb.velocity.y);

    private void PlayerJump() => rb.velocity = new Vector2(rb.velocity.x, jumpForce);

    private void CheckCollision() => isGrounded = Physics2D.Raycast(this.transform.position, Vector2.down, groundCheckDistance, groundLayer);

    private void DoubleJump()=> rb.velocity = new Vector2(rb.velocity.x, doubleJump);
    private void PlayerAnimation()
    {

        anim.SetFloat("Xvelocity", rb.velocity.x);
        anim.SetFloat("Yvelocity", rb.velocity.y);
        anim.SetBool("IsGrounded", isGrounded);
    }


    private void HandleFlip()
    {
        if(isFacingRight && rb.velocity.x < 0 || !isFacingRight && rb.velocity.x > 0)
        {
            FlipPlayer();
        }
    }
   private void FlipPlayer()
    {
       this.transform.Rotate(0, 180, 0);
        isFacingRight = !isFacingRight;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawRay(this.transform.position, Vector2.down * groundCheckDistance);

    }

}

