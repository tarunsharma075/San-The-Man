using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Jobs;

public class TrapSaw : TrapBase
{
    private Vector3[] wayPoints;
    [SerializeField] private float speed;
    [SerializeField] private float ditsance;
    private float offset;
    private int index;
    [SerializeField] private Vector3 moveDirection;


    protected override void Start()
    {
        base.Start();
        SetUpWayPoints();
    }

    private void SetUpWayPoints()
    {
        offset = ditsance / 2;
        wayPoints = new Vector3[2];
        Vector3 direction = moveDirection.normalized;

        wayPoints[0] = transform.position + direction * offset;
        wayPoints[1] = transform.position - direction * offset;
    }


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        SawMovement();
    }

    private void SawMovement()
    {
        this.transform.position = Vector3.MoveTowards(
            this.transform.position,
            wayPoints[index],
            speed * Time.deltaTime
        );

        SwitchIndex();
    }


    private void SwitchIndex()
    {
        if (Vector2.Distance(transform.position, wayPoints[index]) < 0.1f)
        {
            index = (index + 1) % wayPoints.Length;
        }
    }
}