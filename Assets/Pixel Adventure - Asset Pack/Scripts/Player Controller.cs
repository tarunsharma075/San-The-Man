using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Player Movement")] 
   [SerializeField] private float speed;
   [SerializeField] private float jumpForce;

    [Header("Collision Check")]
    [SerializeField]private float groundCheckDistance;
    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded;
    private void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
        anim=  this.GetComponentInChildren<Animator>();
    }
    void Start()
    {
        
    }

    
    void Update()
    {
        CheckCollision();
        Inputhandling();
        PlayerAnimation();
    }

    private void Inputhandling()
    {
        float Xinput = Input.GetAxisRaw("Horizontal");
        PlayerMovement(Xinput);
        if (Input.GetKeyDown(KeyCode.Space)&& isGrounded)
        {
            Debug.Log("space is pressed");
            PlayerJump();
        }
       
    }

    private void PlayerMovement(float xinput)
    {
        rb.velocity= new Vector2(xinput * speed, rb.velocity.y);

    }
    private void PlayerJump()
    {
        Debug.Log("Player Jumped");
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void PlayerAnimation()
    {

        anim.SetFloat("Xvelocity", rb.velocity.x);
    }

    private void CheckCollision()
    {
        isGrounded = Physics2D.Raycast(this.transform.position,Vector2.down,groundCheckDistance,groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawRay(this.transform.position, Vector2.down * groundCheckDistance);

    }

}

