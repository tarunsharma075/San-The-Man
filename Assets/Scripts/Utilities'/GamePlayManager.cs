using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlayManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI health;
    private int currentScore;
    [SerializeField]private int currentNumberFruits;
 [SerializeField]   private float  currentHealth=3;
    [SerializeField]Vector3 spawnOffset = new Vector3(0, 1f, 0);
    void Start()
    {
        spawnEnemies();
        
        UpdateHealthUI();
        

    }

    private void UpdateHealthUI()
    {
        health.text = "HEALTH: " + currentHealth.ToString();
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


 public void IncreaseScore(FruitType fruitType)
    {
        switch (fruitType)
        {
            case FruitType.Apple:
                currentScore += (int)FruitType.Apple;
                break;

            case FruitType.Banana:
                currentScore += (int)FruitType.Banana;
                break;

            case FruitType.Cherry:
                currentScore += (int)FruitType.Cherry;
                break;

            case FruitType.Kiwi:
                currentScore += (int)FruitType.Kiwi;
                break;

            case FruitType.Melon:
                currentScore += (int)FruitType.Melon;
                break;

            case FruitType.Orange:
                currentScore += (int)FruitType.Orange;
                break;

            case FruitType.Pineapple:
                currentScore += (int)FruitType.Pineapple;
                break;

            case FruitType.Strawberry:
                currentScore += (int)FruitType.Strawberry;
                break;
        }

        score.text = "SCORE: " + currentScore;

    }

    
    public void SetFruits(int numberOFFruits)
    {
        currentNumberFruits = numberOFFruits;
    }

    private void Update()
    {
        WinCondition();
        
       

    }

    public void WinCondition()
    {
        if(currentNumberFruits <= 0)
        {
            Debug.Log("You win!");
        }
    }

    public void DecreaseHealth()
    {
        currentHealth--;

        health.text = "HEALTH: " + currentHealth;

        if (currentHealth <= 0)
        {
            SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex
            );

            
        }
    }





}



