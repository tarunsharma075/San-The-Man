using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public enum PlatformState
{
    Still,
    Falling,
    Moving,
    Respawning,
}
public class FallingPlatform :MonoBehaviour
{


    [SerializeField]private PlatformState currentstate;
    [SerializeField] private float fallingSpeed;
    [SerializeField] private float movingSpeed;
    [SerializeField] private GameObject[] points;
    [SerializeField] private BoxCollider2D groundCollider;
    private BoxCollider2D bx;
    private int movingpoint = 0;
    private Vector3 currentPosition;


    private void Awake()
    {

        bx = GetComponent<BoxCollider2D>();
        if (currentstate== PlatformState.Still)
        {
            movingSpeed = 0;
        }
        currentPosition = this.transform.position;
        groundCollider.enabled = false;
    }
    private void Update()
    {
        switch (currentstate)
        {
            case PlatformState.Still:
                break;

            case PlatformState.Moving:
                PlatformMovement();
                break;

            case PlatformState.Falling:
                transform.Translate(Vector3.down * fallingSpeed * Time.deltaTime);
                break;

            case PlatformState.Respawning:
                break;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") &&
    currentstate == PlatformState.Still)
        {
            currentstate = PlatformState.Falling;
        }

        


        if (collision.gameObject.CompareTag("Ground") &&
    currentstate == PlatformState.Falling)
        {
            groundCollider.enabled = true;
            StartCoroutine(OnGroundTouch());
        }
    }


    private void PlatformMovement()
    {

        transform.position = Vector3.MoveTowards(
    transform.position,
    points[movingpoint].transform.position,
    movingSpeed * Time.deltaTime
);

        ChangeIndex();
    }


    private void ChangeIndex()
    {
        if(Vector3.Distance(this.transform.position, points[movingpoint].transform.position) < 0.1)
        {
            if (movingpoint == 1)
            {
                movingpoint = 0;
            }
            else
            {
                movingpoint = 1;
            }
        }
    }


    private IEnumerator OnGroundTouch()
    {
       
        bx.enabled = false;
        

        yield return new WaitForSeconds(1f);

        transform.position = currentPosition;
        currentstate = PlatformState.Still;
        groundCollider.enabled = false;
        bx.enabled = true;
    }
}