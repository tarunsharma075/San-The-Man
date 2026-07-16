using Unity.VisualScripting;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool hasHit;

    [SerializeField] private float bulletspeed;
    [SerializeField] private float blockerHitParticleScale = 2f;


    private float direction ;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector2.right * direction * bulletspeed;
    }

    public void SetDirection(float direction)
    {
        this.direction = direction;
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHit)
        {
            return;
        }

        if (this.gameObject.CompareTag("EnemyBullet") && collision.gameObject.CompareTag("Player"))
        {

            hasHit = true;
            Debug.Log(this.gameObject.name);
            Destroy(this.gameObject);
            ServiceLocator.Instance.playerService.TakeDamage();
            return;
            
        }

        if (this.gameObject.CompareTag("Shuriken") && collision.gameObject.CompareTag("Enemy"))
        {
            hasHit = true;
            Destroy(this.gameObject);
            return;
        }

        if (collision.gameObject.CompareTag("Blocker")&&this.gameObject.CompareTag("Shuriken"))
        {
            hasHit = true;
            SpawnBlockerHitParticle(collision);
            ServiceLocator.Instance.gamePlayservice.OnHitWithShuriken();
            ServiceLocator.Instance.audioService.PlaySFX(SoundTypes.BlockerHit);
            Destroy(this.gameObject);
            return;
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            hasHit = true;
            Destroy(this.gameObject);
            return;
        }



    }

    private void SpawnBlockerHitParticle(Collider2D blockerCollider)
    {
        ParticleSystem blockerHitParticle = ServiceLocator.Instance.gamePlayservice.GetLeafPartcileSystem();
        if (blockerHitParticle == null)
        {
            Debug.Log("Blocker hit particle is not assigned in GamePlayManager");
            return;
        }

        Vector2 hitPosition = blockerCollider.ClosestPoint(transform.position);
        ParticleSystem spawnedParticle = Instantiate(
            blockerHitParticle,
            hitPosition,
            Quaternion.identity);

        spawnedParticle.transform.localScale *= blockerHitParticleScale;

        ParticleSystem.MainModule main = spawnedParticle.main;
        Destroy(spawnedParticle.gameObject, main.duration + main.startLifetime.constantMax);
    }
}
