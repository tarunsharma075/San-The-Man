using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : TrapBase
{
    [SerializeField] private float speed;
    [SerializeField] private float travelDistance;
    [SerializeField] private float fallSpeed;

    private Vector3[] wayPoints;
    private int index;
    private bool isPlayerOnPlatform;

    protected override void Start()
    {
        base.Start();
        SetUpWaypoints();
    }

    protected override void Update()
    {
        if (isPlayerOnPlatform)
        {
            transform.position = new Vector2(
                transform.position.x,
                transform.position.y - fallSpeed * Time.deltaTime
            );
        }
        else
        {
            MovePlatform();
        }
    }

    private void SetUpWaypoints()
    {
        wayPoints = new Vector3[2];
        float offset = travelDistance / 2;

        wayPoints[0] = transform.position + Vector3.up * offset;
        wayPoints[1] = transform.position - Vector3.up * offset;
    }

    private void MovePlatform()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            wayPoints[index],
            speed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, wayPoints[index]) < 0.1f)
        {
            index = (index + 1) % wayPoints.Length;
        }
    }

     protected   override void  OnTriggerEnter2D(Collider2D collision)
    {


        PlayerController player = collision.GetComponent<PlayerController>();


        if (player == null) return;
        {
            if (player != null)
            {
                isPlayerOnPlatform = true;
            }
        }
    }

    private void  OnTriggerExit2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            isPlayerOnPlatform = false;
        }
    }
}
