using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBase : MonoBehaviour

{

    [SerializeField] protected float damage;
    protected Rigidbody2D rb;
    [SerializeField] private TrapsTypes trap;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    protected virtual void Start() { }

    protected virtual void Update() { }


    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ServiceLocator.Instance.playerService.TakeDamage(damage);

            Debug.Log(
                "Player took damage from: " + gameObject.name
            );
        }
    }

}
