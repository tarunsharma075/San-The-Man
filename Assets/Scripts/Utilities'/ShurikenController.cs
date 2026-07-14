using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenController : MonoBehaviour
{
    // Start is called before the first frame update

    private Animator anim;
    private BoxCollider2D bx;

    void Start()
    {
        anim = this.GetComponent<Animator>();
        bx= this.GetComponent<BoxCollider2D>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            
            ServiceLocator.Instance.gamePlayservice.IncreaseShurikenNumberByValue(1);
            ServiceLocator.Instance.audioService.PlaySFX(SoundTypes.FruitCollect);
            anim.SetTrigger("Hit");
            bx.enabled = false;
            Debug.Log("Trigger");

        }
    }

    public void OnDestroy()
    {
        Destroy(this.gameObject);
    }
}


