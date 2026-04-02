using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitManager : MonoBehaviour
{

   [SerializeField]private FruitType fruitType;
    private Animator anim;
    [SerializeField] private GameObject vfxGameObject;
   

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
            Destroy(this.gameObject);  
            
            GameObject vfx=  Instantiate(vfxGameObject, this.transform.position,Quaternion.identity);
            Destroy(vfx,.5f);
        }
    }


    private void SetRandomFruit()
    {
        int randomindex = Random.Range(0, System.Enum.GetValues(typeof(FruitType)).Length);
        anim.SetFloat("RandomFruit", randomindex);
        fruitType = (FruitType)randomindex;
    }


    public void Destroyme()=> Destroy(this.gameObject);
}
