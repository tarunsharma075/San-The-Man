using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Birds : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] float currentdirection = -1;
    [SerializeField] GameObject endzone;
    private void Update()
    {
       
            

            transform.Translate(Vector3.right *currentdirection* speed * Time.deltaTime);
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeathZone"))
        {
            this.transform.position = new Vector3(endzone.transform.position.x,
                   this.transform.position.y,
                   this.transform.position.z);
        }
    }
}

