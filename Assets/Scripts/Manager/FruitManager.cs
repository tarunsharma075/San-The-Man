using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitManager : MonoBehaviour
{

   [SerializeField]private FruitType fruitType;
   [SerializeField] private Animator anim;
   

    private void Awake()
    {
        anim=  gameObject.GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        SetRandomFruit();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() != null)
        {
            Debug.Log("player collieded with fruit");
            Destroy(this.gameObject);        }
    }


    private void SetRandomFruit()
    {
        int randomindex = Random.Range(0, System.Enum.GetValues(typeof(FruitType)).Length);
        anim.SetFloat("RandomFruit", randomindex);
        fruitType = (FruitType)randomindex;
    }
}
