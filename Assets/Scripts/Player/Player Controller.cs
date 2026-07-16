using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    
    private Rigidbody2D rb;
    


    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private PlayerModel playermodel;
    private PlayerView playerView;
    [SerializeField] private GameObject bulletInstance;
    [SerializeField] private  Transform spwanPoint;
    [SerializeField] private float cooldown;
    [SerializeField] private float jumpcooldown;
    [SerializeField] private float jumpbuttonclickDuration;

    [SerializeField] private GameObject rightside;
    [SerializeField] private GameObject lefttside;
    [SerializeField] private GameObject Upside;

    private GameObject currentActivesign;
    private float time;

    private void Awake()
    {

        playerView = playerView = GetComponentInChildren<PlayerView>();
        rb = this.GetComponent<Rigidbody2D>();

        playermodel.canWallJumpd = false;
        
    }


    
    void Start()
    {
     
        ServiceLocator.Instance.playerService.SetPlayer(this);
        
    }

    
    void Update()
    {
        if (playermodel.CurrentPlayerState == PlayerState.Reading)
        {
            StopPlayerForReading();
            return;
        }

        if (playermodel.CurrentPlayerState == PlayerState.EnemyOverJumping) return;
        CheckCollision();

        if (playermodel.CurrentPlayerState== PlayerState.PlayerKnocked) return;
        if (playermodel.CurrentPlayerState== PlayerState.Dead) return;
       

        
        Inputhandling();
        playermodel.Velocity = rb.velocity;
        playerView.UpdateAnimation(playermodel);
        HandleFlip();
        HandleWallSlide();
        
        HandleAttack();
        



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
        if (Input.GetKeyDown(KeyCode.RightAlt) && time <= 0)
        {
           FireBullet();
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
            playermodel.IsWallDetected&&playermodel.CurrentPlayerState==PlayerState.WallSliding && playermodel.canWallJumpd==true) {

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
        playermodel.CurrentPlayerState == PlayerState.Reading ||
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
        if (playermodel.CurrentPlayerState == PlayerState.PlayerKnocked)
            return;
        if (playermodel.CurrentPlayerState == PlayerState.Reading)
            return;
        playermodel.IsWallDetected = Physics2D.Raycast(
            transform.position,
            Vector2.right * playermodel.FacingDirection,
            playermodel.WallCheckDist,
            groundLayer
        );


        if (CheckPlayerGrounded())


        {

            playermodel.CurrentPlayerState = PlayerState.PlayerGrounded;
            playermodel.CanDoubleJump = true;

        }
        else if (playermodel.IsWallDetected)
        {
            if (CanWallSlide())
            {
                playermodel.CurrentPlayerState = PlayerState.WallSliding;
            }


        }
        else
        {
            playermodel.CurrentPlayerState = PlayerState.PlayerAirborne;
        }






    }

    private RaycastHit2D CheckPlayerGrounded()
    {
        return Physics2D.Raycast(
               transform.position,
               Vector2.down,
               playermodel.GroundCheckDist,
               groundLayer);
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


        StopAllCoroutines();
        StartCoroutine(KnockBackRoutine());

        
        
        

    }

    private IEnumerator KnockBackRoutine()
    {


        playermodel.CurrentPlayerState = PlayerState.PlayerKnocked;

        ServiceLocator.Instance.audioService.PlaySFX(SoundTypes.PlayerHit);

        ServiceLocator.Instance.gamePlayservice.DecreaseHealth();

        this.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
        rb.velocity = Vector2.zero;

        rb.AddForce(
            new Vector2(
                9 * -playermodel.FacingDirection,
                playermodel.KnockbackDistance.y),
            ForceMode2D.Impulse);

       SpriteRenderer playersprite = this.gameObject.GetComponentInChildren<SpriteRenderer>();    
        playersprite.color = Color.red;

        yield return new WaitForSeconds(0.2f);

        playermodel.CurrentPlayerState = PlayerState.PlayerGrounded;
        playersprite.color = Color.white;
        this.gameObject.GetComponent<CapsuleCollider2D>().enabled = true;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       


        if (collision.CompareTag("Traps")){
            PlayerKnockBack();

        }

       
    }

    public void   PlayreDie()
    {
        playermodel.CurrentPlayerState = PlayerState.Dead;
        Debug.Log(playermodel.CurrentPlayerState);
        CapsuleCollider2D bx= this.GetComponent<CapsuleCollider2D>();
        bx.enabled = false;
        SpriteRenderer playerSprite = this.GetComponentInChildren<SpriteRenderer>();    
        ColorUtility.TryParseHtmlString("#FF0000", out Color hitcolor);
        playerSprite.color = hitcolor;
        StopAllCoroutines();
        StartCoroutine(PlayerDieSequence() );
       




    }

    IEnumerator PlayerDieSequence()
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);
        rb.velocity = Vector2.down * 5f;

        yield return new WaitForSeconds(1f);
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
        if (ServiceLocator.Instance.gamePlayservice.GetNumberOfShurikens() > 0)
        {

            
            GameObject PeaBullet = Instantiate(bulletInstance, spwanPoint.transform.position, Quaternion.identity);
            BulletBehaviour shuriken = PeaBullet.GetComponent<BulletBehaviour>();
            shuriken.SetDirection(playermodel.FacingDirection);
            ServiceLocator.Instance.gamePlayservice.DecreaseNumberOFShurikens();
        }
        }

        public void StunJump()
    {

        StopAllCoroutines();

        StartCoroutine(StunJumpRoutine());
       
        
    }
   
    private IEnumerator StunJumpRoutine()
    {
        
        playermodel.CurrentPlayerState = PlayerState.EnemyOverJumping;
        this.rb.velocity = new Vector2(5.5f* playermodel.FacingDirection, 5);
        yield return new WaitForSeconds(0.7f);
        
        
            playermodel.CurrentPlayerState = PlayerState.PlayerGrounded;
            Debug.Log(playermodel.CurrentPlayerState);
        
    }

    private void Respawn()
    {
        GameManager.Instance.RespawnPlayer();
    }


    public PlayerState GetPlayerCurrentState() {


       
        return playermodel.CurrentPlayerState;
    
    
    }

    public void StartReading()
    {
        StopAllCoroutines();
        playermodel.CurrentPlayerState = PlayerState.Reading;
        StopPlayerForReading();
    }

    private void StopPlayerForReading()
    {
        rb.velocity = Vector2.zero;
        playermodel.XInput = 0;
        playermodel.YInput = 0;
        playermodel.Velocity = Vector2.zero;
        playerView.UpdateAnimation(playermodel);
    }

    public void StopReading()
    {
        if (playermodel.CurrentPlayerState != PlayerState.Reading)
        {
            return;
        }

        playermodel.CurrentPlayerState = CheckPlayerGrounded()
            ? PlayerState.PlayerGrounded
            : PlayerState.PlayerAirborne;
    }

    public void UnlockWallJump() {


        playermodel.canWallJumpd = true;

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("DeathZone"))
        {
            GameManager.Instance.RespawnPlayer();
        }
    }

public void ActivateDirectionSign(PlayerDirection direction)
    {
        
       
        Vector3 spawnpoint;
        

        switch (direction)
        {
            case PlayerDirection.Left:
                spawnpoint = this.transform.position - Vector3.left*2;
                currentActivesign = Instantiate(lefttside,spawnpoint,Quaternion.identity);
                


                break;
            case PlayerDirection.Right:
                spawnpoint = this.transform.position + Vector3.right * 2;
                currentActivesign = Instantiate(lefttside, spawnpoint, Quaternion.identity);
                

                break;
            case PlayerDirection.Up:


                spawnpoint = this.transform.position + Vector3.up * 2;
                currentActivesign = Instantiate(Upside, spawnpoint, Quaternion.identity);
                 
                break;
        }

       
    }


  public void SetCurrentDirectionaDeactivate()
    {
        Destroy(currentActivesign);
        currentActivesign = null;
    }


    public void CallPlayerEndSequence()
    {
        playermodel.CurrentPlayerState = PlayerState.PlayerDead;
        
        playerView.CaveEndingSequence();
    }

    public void FadePlayerSpriteToZero(float duration)
    {
        StopCoroutine(nameof(FadePlayerSpriteRoutine));
        StartCoroutine(FadePlayerSpriteRoutine(duration));
    }

    public void FadePlayerSpriteToZero()
    {
        FadePlayerSpriteToZero(1.5f);
    }

    private IEnumerator FadePlayerSpriteRoutine(float duration)
    {
        SpriteRenderer playerSprite = GetComponentInChildren<SpriteRenderer>();
        if (playerSprite == null)
        {
            yield break;
        }

        Color startColor = playerSprite.color;
        float startAlpha = startColor.a;
        float timer = 0f;

        if (duration <= 0)
        {
            startColor.a = 0;
            playerSprite.color = startColor;
            yield break;
        }

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / duration);
            Color currentColor = playerSprite.color;
            currentColor.a = Mathf.Lerp(startAlpha, 0, progress);
            playerSprite.color = currentColor;
            yield return null;
        }

        Color finalColor = playerSprite.color;
        finalColor.a = 0;
        playerSprite.color = finalColor;
    }
    

}

