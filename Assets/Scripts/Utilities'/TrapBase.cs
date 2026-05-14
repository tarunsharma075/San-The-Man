using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBase : MonoBehaviour

{

    [SerializeField] protected float damage;
    private Rigidbody2D rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    protected virtual void Start() { }

    protected virtual void Update() { }

    


}
