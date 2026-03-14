using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        
    }

    
    void Update()
    {
        Inputhandling(); 
    }

    private void Inputhandling()
    {
        float Xinput = Input.GetAxisRaw("Horizontal");
        PlayerMovement(Xinput);
    }

    private void PlayerMovement(float xinput)
    {
        rb.velocity= new Vector2(xinput * speed, rb.velocity.y);
    }
}
