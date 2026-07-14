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

            StartCoroutine(FruitEnd());
            
           
        }
    }


    private IEnumerator FruitEnd() {



        ServiceLocator.Instance.audioService.PlaySFX(SoundTypes.FruitCollect);
        ServiceLocator.Instance.gamePlayservice.IncreasePlayerHealth();
        fruitCount--;
        ServiceLocator.Instance.gamePlayservice.SetNumberofFruits(fruitCount);
        anim.SetTrigger("End");
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);


    }

    private void SetRandomFruit()
    {
        int randomindex = Random.Range(0, System.Enum.GetValues(typeof(FruitType)).Length);
        anim.SetFloat("RandomFruit", randomindex);
        fruitType = (FruitType)randomindex;
        
    }


    public void Destroyme()=> Destroy(this.gameObject);


    
}
