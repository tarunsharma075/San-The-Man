using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingTrap :TrapBase


{
    [SerializeField] private Rigidbody2D swingRB;
    [SerializeField] private float swingForce;
    private Vector2 pushvector;

    private void Start()
    {
        pushvector = new Vector2(swingForce, 0);
        swingRB.AddForce(pushvector, ForceMode2D.Impulse);
    }
}
