using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Jobs;

public class TrapSaw : WorldObject
{
   [SerializeField] private Transform[] wayPoints;
    [SerializeField] private float speed;
    
    [SerializeField ]private int index;


    protected override void Start()
    {
        base.Start();
       
    }

   



    protected override void Update()
    {
        base.Update();
        //SawMovement();
    }

    private void SawMovement()
    {
        this.transform.position = Vector3.MoveTowards(
            this.transform.position,
            wayPoints[index].position,
            speed * Time.deltaTime
        );

        SwitchIndex();
    }


    private void SwitchIndex()
    {
        if (index == 1 &&
    Vector3.Distance(transform.position, wayPoints[(int)index].position) < 0.1f)
        {
            index = 0;  
        }else if (index == 1 &&
    Vector3.Distance(transform.position, wayPoints[(int)index].position) < 0.1f)
        {
            index = 1;
        }

    }


    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log(this.gameObject.name);
            ServiceLocator.Instance.playerService.TakeDamage();
        }


        if (collision.gameObject.CompareTag("Blocker")){
            Destroy(collision.gameObject);
        }
    }


   
}

