using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FruitController : MonoBehaviour
{

   [SerializeField]private FruitType fruitType;
    private Animator anim;
    //[SerializeField] private GameObject vfxGameObject;
    private static int fruitCount = 0;


    private void Awake()
    {
        anim=  gameObject.GetComponentInChildren<Animator>();

        fruitCount = 0;


    }

    private void Start()
    {
        SetRandomFruit();
        fruitCount++;
        ServiceLocator.Instance.gamePlayservice.SetNumberofFruits(fruitCount);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() != null)
        {
            Debug.Log("player collieded with fruit");
           
            ServiceLocator.Instance.audioService.PlaySFX(SoundTypes.FruitCollect);
            fruitCount--;
            ServiceLocator.Instance.gamePlayservice.SetNumberofFruits(fruitCount);
            Destroy(this.gameObject);  
            
            //GameObject vfx=  Instantiate(vfxGameObject, this.transform.position,Quaternion.identity);
            //Destroy(vfx,.5f);
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
