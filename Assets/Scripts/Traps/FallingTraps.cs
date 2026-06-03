using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float travelDistance;
    [SerializeField] private float fallSpeed;

    private Rigidbody2D rb;
    private Vector2[] wayPoints;
    private int index;
    private bool isPlayerOnPlatform;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        SetUpWaypoints();
    }

    private void FixedUpdate()
    {
        if (isPlayerOnPlatform)
        {
            Vector2 newPosition = rb.position + Vector2.down * fallSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);
        }
        else
        {
            MovePlatform();
        }
    }

    private void SetUpWaypoints()
    {
        wayPoints = new Vector2[2];

        float offset = travelDistance / 2f;
        Vector2 startPosition = rb.position;

        wayPoints[0] = startPosition + Vector2.up * offset;
        wayPoints[1] = startPosition + Vector2.down * offset;
    }

    private void MovePlatform()
    {
        Vector2 newPosition = Vector2.MoveTowards(
            rb.position,
            wayPoints[index],
            speed * Time.fixedDeltaTime
        );

        rb.MovePosition(newPosition);

        if (Vector2.Distance(rb.position, wayPoints[index]) < 0.1f)
        {
            index = (index + 1) % wayPoints.Length;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() == null) return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y < -0.5f)
            {
                isPlayerOnPlatform = true;
                return;
            }
        }

        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground") || isPlayerOnPlatform)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.gravityScale = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            isPlayerOnPlatform = false;
        }
    }
}