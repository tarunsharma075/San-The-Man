using Unity.VisualScripting;
using UnityEngine;

public class FallingPlatform : WorldObject
{
    [SerializeField] private float speed;
    [SerializeField] private float travelDistance;
    [SerializeField] private float fallSpeed;

   
    private Vector2[] wayPoints;
    [SerializeField]private int index;
    private bool isPlayerOnPlatform;
   

    protected override void Awake()
    {
       anim= gameObject.GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        SetUpWaypoints();
    }

    private void Update()
    {
        if (isPlayerOnPlatform)
        {
            this.transform.position -= Vector3.down * fallSpeed * Time.deltaTime;

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
        Vector2 startPosition = this.transform.position;

        wayPoints[0] = startPosition + Vector2.up * offset;
        wayPoints[1] = startPosition + Vector2.down * offset;
    }

    private void MovePlatform()
    {
        transform.position = Vector2.MoveTowards(
    transform.position,
    wayPoints[index],
    speed * Time.deltaTime
);

        if (Vector2.Distance(this.transform.position, wayPoints[index]) < 0.1f)
        {
            index = (index + 1) % wayPoints.Length;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOnPlatform = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isPlayerOnPlatform = false;
    }
}