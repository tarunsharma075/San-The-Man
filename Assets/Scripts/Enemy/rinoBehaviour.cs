using UnityEngine;

public class rinoBehaviour : EnemyBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float playerdistance;
   

    private bool isplayerdetected;
    private Transform target;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        target = ServiceLocator.Instance.playerService.GetPlayer().transform;
    }

    protected override void Update()
    {
        base.Update();

        isplayerdetected = Physics2D.Raycast(
            transform.position,
            Vector2.right * facingDirection,
            playerdistance,
            playerLayer
        );

        CheckPlayerCollision();
        MovementTowardsPlayer();

        if (IsWallDetected)
        {
            FlipPlayer();
        }
    }

    private void MovementTowardsPlayer()
    {
        if (isplayerdetected)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                target.position,
                movementSpeed * Time.deltaTime
            );

            if (this.transform.position.x < 0.5)
            {
                //hit animation by rino
            }
        }
        
    }

    


    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = isplayerdetected ? Color.red : Color.green;

        Gizmos.DrawLine(
            transform.position,
            new Vector2(
                transform.position.x + (playerdistance * facingDirection),
                transform.position.y
            )
        );
    }


    private void CheckPlayerCollision()
    {
        isplayerdetected= Physics2D.Raycast(
            transform.position,
            Vector2.right * facingDirection,
            playerdistance,
            playerLayer
        ); 
    }
}