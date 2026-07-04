using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GamePlayManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private TextMeshProUGUI health;

    [SerializeField] private int currentNumberFruits;
    [SerializeField] private float currentHealth = 3;
    [SerializeField] Vector3 spawnOffset = new Vector3(0, 1f, 0);
    [SerializeField] private Image greenHealthBar;
    [SerializeField] private Image redHealthBar;
    [SerializeField] float maxhealth = 3;
    [SerializeField] private TextMeshProUGUI shurikenNumber;
    [SerializeField] private SpriteRenderer[] barriers;
    private int currentShurikenNumber = 0;
    private int blockersHit = 0;

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

    private void HealthUIManagment()
    {
        greenHealthBar.fillAmount = Mathf.Clamp(currentHealth / maxhealth, 0, 1);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (barriers[0].CompareTag("Blockers") && collision.CompareTag("Shuriken"))
        {

            blockersHit++;
            if (blockersHit < 3)
            {

                for (int i = 0; i < barriers.Length; i++)
                {

                    Color c = barriers[i].color;
                    c.a = blockersHit / 3;
                    barriers[i].color = c;

                    if (blockersHit >= 3)
                    {
                        foreach (var barrier in barriers)
                        {
                            barrier.enabled = false;
                        }
                    }
                }



            }


        }
    }
}





