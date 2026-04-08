using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapsBehaviour : MonoBehaviour
{
    [Header("Falling Platfronm")]
    private Vector3[] wayPoints;
    [SerializeField] private float fallingPlatformSpeed;
    [SerializeField] private float travelDistance;
    private int wayPointIndex = 0;
    [SerializeField] private float fallingPlatfromFallingSpeed;
    private Animator fallingPlatfromAnimator;
    private bool isPlatfromFalling = false;
    private bool isPlayerOnPlatform = false;
    private Vector3 originalPosition;






    [SerializeField] private float Damage;
    [SerializeField] private TrapsTypes currentTrap;
    

    private void Start()
    {
        originalPosition = transform.position;
        if(currentTrap == TrapsTypes.falling)
        {
            fallingPlatfromAnimator= gameObject.GetComponent<Animator>();
        }

        SetUpWaypoints();
    }


    void Update()
    {

        switch (currentTrap)
        {
            case TrapsTypes.Simple:
            
            {
                    

            }
            break;

             case TrapsTypes.Saw:
                {

                }
                break;

                case TrapsTypes.falling:
                {
                    if (isPlayerOnPlatform)
                    {
                        PlatformFalling();
                    }
                    
                    else {
                        HandleFallingPlatformMovement();
                    }
                }
                 break;

            default:

                {

                    Debug.Log("No trap is selectd");
                }
                break;


        }
        
    }

    private void SetUpWaypoints() { 
    
     wayPoints= new Vector3[2];
     float yoffset = travelDistance / 2;
        wayPoints[0] = transform.position + new Vector3(0, yoffset, 0);
        wayPoints[1] = transform.position +new Vector3(0, -yoffset, 0);


    }

    private void HandleFallingPlatformMovement() { 
    
        transform.position=  Vector3.MoveTowards(transform.position, wayPoints[wayPointIndex], fallingPlatformSpeed * Time.deltaTime);

        if(Vector2.Distance(transform.position, wayPoints[wayPointIndex]) < 0.1f)
        {
            wayPointIndex++;

            if(wayPointIndex >= wayPoints.Length)
            {
                wayPointIndex = 0;
            }
        }


    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null && currentTrap == TrapsTypes.falling)
        {
            isPlayerOnPlatform = true;
            isPlatfromFalling = true;
            fallingPlatfromAnimator.SetBool("isPlayerOnPlatform", isPlayerOnPlatform);

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
       
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null && currentTrap == TrapsTypes.falling)
            {
                isPlatfromFalling = false;
                isPlayerOnPlatform = false; 

                
                transform.position = originalPosition;
                fallingPlatfromAnimator.SetBool("isPlayerOnPlatform", isPlayerOnPlatform);
                wayPointIndex = 0; 
                
                SetUpWaypoints();  
            }
        }
    }

    private void PlatformFalling()
    {
        
        transform.position = new Vector2(this.transform.position.x, transform.position.y - fallingPlatfromFallingSpeed * Time.deltaTime);
    }




}
