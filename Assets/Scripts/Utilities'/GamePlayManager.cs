using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.WSA;
public class GamePlayManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject[] prefabs;
    

    [SerializeField] private int currentNumberFruits;
    [SerializeField] private float currentHealth = 3;
    [SerializeField] Vector3 spawnOffset = new Vector3(0, 1f, 0);
    [SerializeField] private Image greenHealthBar;
    [SerializeField] private Image redHealthBar;
    [SerializeField] float maxhealth = 3;
    [SerializeField] private TextMeshProUGUI shurikenNumber;
    [SerializeField] private GameObject barrierForLevelOne;
    [SerializeField] private GameObject[] relics;
    [SerializeField] private spikeController Spike;    
    [SerializeField] private int currentShurikenNumber = 0;

   
    
    



    private float blockersHit = 0;

    void Awake()
    {
        spawnEnemies();
        NumberOfShurikens();

        currentHealth = maxhealth;
        

        greenHealthBar.fillAmount = Mathf.Clamp(currentHealth / maxhealth, 0, 1);
        redHealthBar.fillAmount = Mathf.Clamp(currentHealth / maxhealth, 0, 1);
    }


    private void spawnEnemies()
    {


        for (int i = 0; i < spawnPoints.Length; i++)
        {
            int randomEnemey = Random.Range(0, prefabs.Length);
            GameObject currentSpawnEnemy = Instantiate(prefabs[randomEnemey],
            spawnPoints[i].position,
            Quaternion.identity);



        }
    }



    public void SetFruits(int numberOFFruits)
    {
        currentNumberFruits = numberOFFruits;
    }

    private void Update()
    {
        NumberOfShurikens();
        WinCondition();



    }

    public void WinCondition()
    {
        if (currentNumberFruits <= 0)
        {
            Debug.Log("You win!");
        }
    }

    public void DecreaseHealth()
    {
        currentHealth--;

        HealthUIManagment();

        if (currentHealth <= 0)
        {
            SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex
            );


        }
    }

    private void NumberOfShurikens()
    {
        if (currentShurikenNumber < 0)
        {
            currentShurikenNumber = 0;
        }


        shurikenNumber.text = currentShurikenNumber.ToString();
    }


    public int IncreaseNumberofShurikens()
    {

        return currentShurikenNumber++;

    }

    public int DecreaseNumberofShurikens()
    {
        return currentShurikenNumber--;
    }

    public int GetCurrentShurikenNumber()
    {
        return currentShurikenNumber;
    }



    public void LoadCurrentLevel()
    {
        SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex
            );

    }


    public void PlayerDead()
    {
        StartCoroutine(PlayerDeathScene());
    }

    IEnumerator PlayerDeathScene()
    {
        currentHealth = 0;
        HealthUIManagment();
        ServiceLocator.Instance.playerService.PlayerDie();
        yield return new WaitForSeconds(1f);
        LoadCurrentLevel();

    }

    // player health UI update
    private void HealthUIManagment()
    {
        greenHealthBar.fillAmount = Mathf.Clamp(currentHealth / maxhealth, 0, 1);

    }

    public void OnHitShuriken()
    {


        blockersHit++;
        Debug.Log("BlockersHit: " + blockersHit);
        if (blockersHit < 3)
        {


            SpriteRenderer barrierSprite = barrierForLevelOne.GetComponent<SpriteRenderer>();


            Color c = barrierSprite.color;
                c.a = (3f - blockersHit) / 3f;
                barrierSprite.color = c;


            



        }

        if (blockersHit >= 3)
        {
            Destroy(barrierForLevelOne);

        }


    }


    public void SpikeActivated()
    {
        
        
     Spike.ActivateSpike();

    }

  //enemy health UI pdate  

    public void UpdateEnemeyUI(Image enemyGreenHealthBar,int maxhealth,int currenthealth)
    {
        enemyGreenHealthBar.fillAmount= Mathf.Clamp(maxhealth/ currenthealth, 0, 1);
    }
  

  
}






