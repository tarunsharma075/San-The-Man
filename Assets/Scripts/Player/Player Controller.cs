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
    [SerializeField] private GameObject bulletInstance;
    [SerializeField] private  Transform spwanPoint;
    [SerializeField] private float cooldown;
    private float time;

    private void Awake()
    {

        playerView = playerView = GetComponentInChildren<PlayerView>();
        rb = this.GetComponent<Rigidbody2D>();
       

    }


    
    void Start()
    {
     if (isdead)
        {
            isdead = false;
        }
        ServiceLocator.Instance.playerService.SetPlayer(this);
        
    }

    
    void Update()
    {
        if (playermodel.IsKnocked) return;
        if (isdead) return;

        CheckCollision();
        Inputhandling();
        HandleWallSlide();
        HandleFlip();

        playermodel.Velocity = rb.velocity;
        playerView.UpdateAnimation(playermodel);

 

    }

    private void HandleWallSlide()
    {

        if (!CanWallSlide())
        {
            return;

        }
        else if (CanWallSlide())
        {
            float yModifer = playermodel.YInput < 0 ? 0.5f : 1;

            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * yModifer);
        }
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

    private void HandleAttack()
    {
        time -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Mouse0) && time <= 0)
        {
            playerView.Attack();
            time = cooldown;

        }
    }

    private  void  JumpButton()

    {
        
        if (playermodel.CurrentPlayerState==PlayerState.PlayerGrounded)
        {
            
            PlayerJump();
        }
        else if (
            playermodel.IsWallDetected&&playermodel.CurrentPlayerState==PlayerState.WallSliding) {

            WallJump();
            

        }



        else if (playermodel.CurrentPlayerState == PlayerState.PlayerAirborne && playermodel.CanDoubleJump)
        {
            DoubleJump();
            playermodel.CanDoubleJump= false;
        }
    }



    private void PlayerMovement(float xinput)
    {
        if (playermodel.CurrentPlayerState == PlayerState.PlayerKnocked ||
        playermodel.CurrentPlayerState == PlayerState.PlayerDead ||
        playermodel.CurrentPlayerState == PlayerState.WallSliding||
        playermodel.IsWallDetected||playermodel.CurrentPlayerState==PlayerState.WallJumping
       )
        {
            return;
        }

        rb.velocity = new Vector2(xinput * playermodel.Speed, rb.velocity.y);




    }

    private void PlayerJump()

    {
        
        
        rb.velocity = new Vector2(rb.velocity.x, playermodel.JumpForce);
       
    
    ServiceLocator.Instance.audioService.PlaySFX(SoundTypes.PlayerJump);
  

    }

    private void CheckCollision()
    {

        if (playermodel.CurrentPlayerState == PlayerState.WallJumping)
            return;
        playermodel.IsWallDetected = Physics2D.Raycast(
            transform.position,
            Vector2.right * playermodel.FacingDirection,
            playermodel.WallCheckDist,
            groundLayer
        );


        if (Physics2D.Raycast(
       transform.position,
       Vector2.down,
       playermodel.GroundCheckDist,
       groundLayer))


        {

            playermodel.CurrentPlayerState = PlayerState.PlayerGrounded;
            playermodel.CanDoubleJump = true;

        } else if (playermodel.IsWallDetected )
        {
            if (CanWallSlide()){
                playermodel.CurrentPlayerState = PlayerState.WallSliding;
            }


        }
        else
        {
            playermodel.CurrentPlayerState = PlayerState.PlayerAirborne;
        }
       
        




        }

    private bool CanWallSlide()
    {
        return playermodel.IsWallDetected && rb.velocity.y < 0;
        
    }

    private void DoubleJump()
    {

        playermodel.CanDoubleJump = false;
        ServiceLocator.Instance.audioService.PlaySFX(SoundTypes.PlayerJump);
        rb.velocity = new Vector2(rb.velocity.x, playermodel.DoubleJumpForce);
    }

    private void WallJump()
    {
        playermodel.CurrentPlayerState = PlayerState.WallJumping;
        playermodel.CanDoubleJump = true;
        rb.velocity = new Vector2(playermodel.WallJumpForce.x*-playermodel.FacingDirection, playermodel.WallJumpForce.y);
        ServiceLocator.Instance.audioService.PlaySFX(SoundTypes.PlayerJump);
        FlipPlayer();
       StopAllCoroutines();
       StartCoroutine(wallJumpRoutine());

    }

    


    private IEnumerator wallJumpRoutine()
    {
        
        yield  return new WaitForSeconds(playermodel.WallJumpDuration);
        playermodel.CurrentPlayerState = PlayerState.PlayerAirborne;

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
        Gizmos.color= playermodel.CurrentPlayerState==PlayerState.PlayerGrounded ? Color.green : Color.red;
        
        Gizmos.DrawLine(transform.position, new Vector2(this.transform.position.x, this.transform.position.y - playermodel.GroundCheckDist));

        Gizmos.color = playermodel.IsWallDetected ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, new Vector2(this.transform.position.x + (playermodel.WallCheckDist * playermodel.FacingDirection), this.transform.position.y));


    }


    public  void PlayerKnockBack()
    {
        playermodel.CurrentPlayerState = PlayerState.PlayerKnocked;
        StartCoroutine(KnockBackRoutine());

        
        rb.velocity = new Vector2(playermodel.KnockbackDistance.x * -playermodel.FacingDirection, playermodel.KnockbackDistance.y);

    }

    private IEnumerator KnockBackRoutine()
    {

       
        ServiceLocator.Instance.audioService.PlaySFX(SoundTypes.PlayerHit);
        playerView.PlayKnockedAnimation(playermodel.CurrentPlayerState);
        ServiceLocator.Instance.gamePlayservice.DecreaseHealth();
        yield return new WaitForSeconds(playermodel.KnockbackDuration);
        playermodel.CurrentPlayerState = PlayerState.PlayerAirborne;
       
        playerView.PlayKnockedAnimation(playermodel.CurrentPlayerState);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
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
      ServiceLocator.Instance.gamePlayservice.DecreaseHealth();

        yield return new WaitForSeconds(0.5f);

        GameManager.Instance.RespawnPlayer();

    }



public void TakeDamage()
    {
        
        PlayerKnockBack();
    }


    public GameObject GetPlayerObject()
    {
        return this.gameObject;
    }


  public void  FireBullet()
    {
        GameObject PeaBullet = Instantiate(bulletInstance, spwanPoint.transform.position, Quaternion.identity);
    }

    public void StunJump()
    {
        this.rb.velocity = new Vector2(playermodel.KnockbackDistance.x*playermodel.FacingDirection,  playermodel.KnockbackDistance.y);
        
    }
   
    

}

