using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrunkAttack : EnemyBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform firePoint;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float playerdistance;
     private float timer;
    [SerializeField] private float cooldownTime;

    private bool isplayerdetected = false;
    [SerializeField] private float startingtime;
    private Transform detectedPlayer;
    [SerializeField] private BoxCollider2D hitBox;

    
    
    protected override void Awake()
    {
       base.Awake();
    }

    protected override void Update()
    {

        if (IsGameplayPausedForInstruction())
        {
            StopEnemyMovement();
            return;
        }

        if (currentState == EnemyState.Dead || currentState == EnemyState.Stunned) return;
        if (ServiceLocator.Instance.playerService.GetPlayerState() == PlayerState.Dead) return;
        startingtime -= Time.deltaTime;
        if (startingtime < 0)
        {
            base.Update();
            isplayerdetected = TryGetDetectedPlayer(playerdistance, playerLayer, out detectedPlayer);

            if (isplayerdetected)
            {
                FaceTarget(detectedPlayer);
                Attack();
            }
        }

    }


    private void Attack()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            anim.SetTrigger("TrunkAttack"); // fires once, resets automatically
            timer = cooldownTime;
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = isplayerdetected ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, playerdistance);
    }

     public void Shoot()
    {
        Debug.Log("Shoot");
       GameObject Bullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
        Bullet.GetComponent<BulletBehaviour>().SetDirection(facingDirection);

    }


    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (currentState == EnemyState.Dead || currentState == EnemyState.Stunned) return;
        
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();

            if (collision.transform.position.y > transform.position.y &&
          playerRb.velocity.y < 0)
            {

                if (currentHealth == 1)
                {
                    
                    TrunkEnd();
                    return;
                }

                base.StunDamage();
                
                StartCoroutine(StunTrunkBehaviour());
            }
            
           
        }
    }


    private IEnumerator StunTrunkBehaviour()
    {

        currentState = EnemyState.Stunned;
        this.anim.enabled = false;
        yield return new WaitForSeconds(0.5f);
        currentState = EnemyState.Alive;
        this.anim.enabled = true;


    }

   private void TrunkEnd()
    {
        trunkEndSequence();
    }

    private void trunkEndSequence()
    {
        StartCoroutine(treeend());
        
    }


    private IEnumerator treeend()
    {
        ServiceLocator.Instance.playerService.EnemyOverStunJump();
        ServiceLocator.Instance.gamePlayservice.IncreaseShurikenNumberByValue(1);
        
        this.rb.velocity = Vector2.zero;
        currentState = EnemyState.Dead;
        anim.SetTrigger("Die");
        hitBox.enabled = false;
        ServiceLocator.Instance.audioService.PlaySFX(SoundTypes.EnemyStun);
      
        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D (collision);
    }

   

}
