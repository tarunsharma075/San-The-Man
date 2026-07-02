using UnityEditor;
using UnityEngine;

public class TurtleBehaviour : EnemyBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float playerdistance;


    private bool isplayerdetected = false;
    
   
    

    protected override void Awake()
    {
        base.Awake();
        
    }

    private void Update()
    {
      CheckPlayerCollision();

    }

    private void CheckPlayerCollision()
    {
        if (ServiceLocator.Instance.playerService.GetPlayerState() == PlayerState.Dead) return;
        Collider2D player = Physics2D.OverlapCircle(this.transform.position, playerdistance, playerLayer);

        if (player != null) {

            isplayerdetected = true;
        }
        else
        {
            isplayerdetected = false;
        }
        anim.SetBool("IsplayerDetected", isplayerdetected);


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isplayerdetected ? Color.green : Color.red;
        Gizmos.DrawWireSphere(this.transform.position, playerdistance);
    }
}

