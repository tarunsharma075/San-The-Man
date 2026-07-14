using Unity.VisualScripting;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float bulletspeed;


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

        if (this.gameObject.CompareTag("EnemyBullet") && collision.gameObject.CompareTag("Player"))
        {

            Debug.Log(this.gameObject.name);
            Destroy(this.gameObject);
            ServiceLocator.Instance.playerService.TakeDamage();
            
        }

        if (this.gameObject.CompareTag("Shuriken") && collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(this.gameObject);




        }

        if (collision.gameObject.CompareTag("Blocker")&&this.gameObject.CompareTag("Shuriken"))
        {
            ServiceLocator.Instance.gamePlayservice.OnHitWithShuriken();
           

        }
        if (this.gameObject.CompareTag("Ground"))
        {
            Destroy(this.gameObject);




        }



    }
}