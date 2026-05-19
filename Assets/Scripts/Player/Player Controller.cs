using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    
    private Rigidbody2D rb;
    private bool isdead= false;


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


        ServiceLocator.Instance.playerService.SetPlayer(this);
        if (isdead)
        {
            isdead = false;
        }
    }

    
    void Update()
    {
 if (playermodel.IsKnocked)
            return;
    
 if(isdead )return;

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
        
        Gizmos.DrawLine(transform.position, new Vector2(this.transform.position.x, this.transform.position.y - playermodel.GroundCheckDist));

        Gizmos.color = playermodel.IsWallDetected ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, new Vector2(this.transform.position.x + (playermodel.WallCheckDist * playermodel.FacingDirection), this.transform.position.y));


    }


    public  void PlayerKnockBack()
    {
        StartCoroutine(KnockBackRoutine());
        
        rb.velocity = new Vector2(playermodel.KnockbackDistance.x * -playermodel.FacingDirection, playermodel.KnockbackDistance.y);

    }

    private IEnumerator KnockBackRoutine()
    {
        playermodel.IsKnocked = true;
        playermodel.CanBeKnocked = false;
        playerView.PlayKnockedAnimation(playermodel);
        Debug.Log("Status of "+playermodel.IsKnocked);
        yield return new WaitForSeconds(playermodel.KnockbackDuration);
        playermodel.IsKnocked = false;
        playermodel.CanBeKnocked = true;
        playerView.PlayKnockedAnimation(playermodel);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Player collided with " + collision.name);
        if (collision.CompareTag("DeathZone"))
        {
           StartCoroutine(PlayreDie());
        }

        if (collision.CompareTag("Traps")){
            PlayerKnockBack();

        }
    }

    IEnumerator  PlayreDie()
    {
        isdead= true;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.gravityScale = 0;
        
        playerView.PlayerDeath();
        yield return new WaitForSeconds(0.5f);

        ServiceLocator.Instance.gameManager.RespawnPlayer();

    }



public void TakeDamage(float Damage)
    {
        PlayerKnockBack();
    }


    public GameObject GetPlayerObject()
    {
        return this.gameObject;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject, collision.gameObject);
    }
}

