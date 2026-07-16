using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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


    [SerializeField] private GameObject bull;
    [SerializeField] private GameObject relic;
    [SerializeField] private GameObject []blocker;
    [SerializeField] private ParticleSystem leafburst;

    [SerializeField] private CinemachineVirtualCamera playercamera;
    [SerializeField] private CinemachineVirtualCamera spikecamera;
    [SerializeField] private DialougeInteraction dialougeInteraction;

    private float blockersHit = 0;
    private Coroutine instructionRoutine;
    public bool IsInstructionOpen { get; private set; }
    public bool IsJungleRelicCollected { get; private set; }
    public bool IsBullDead => bull == null;

    void Awake()
    {
        spawnEnemies();
        NumberOfShurikens();

        currentHealth = maxhealth;
        

        greenHealthBar.fillAmount = Mathf.Clamp(currentHealth / maxhealth, 0, 1);
        redHealthBar.fillAmount = Mathf.Clamp(currentHealth / maxhealth, 0, 1);
    }


    private void Start()
    {
        if (ServiceLocator.Instance != null)
            ServiceLocator.Instance.RegisterGamePlayManager(this);

        if (dialougeInteraction == null)
        {
            dialougeInteraction = FindObjectOfType<DialougeInteraction>();
        }

        if (dialougeInteraction == null)
        {
            GameObject dialogueObject = new GameObject("Runtime Dialogue Interaction");
            dialougeInteraction = dialogueObject.AddComponent<DialougeInteraction>();
        }
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
        if (bull == null && relic == null)
        {
            
                for (int i = 0; i < blocker.Length; i++)
                {
                    blocker[i].SetActive(false);
                }
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
  
    public void IncreasePlayerHealth()
    {
        if(currentHealth== maxhealth)
        {
            return;
        }

        currentHealth++;
        HealthUIManagment();
    }


    public void IncreaseShurikenNumberByValue(int value)
    {
        currentShurikenNumber += value;
    }
  

   public ParticleSystem GetLeafPartcileSystem()
    {
        return leafburst;
    }


    public void ActivateSpikeCamera()
    {
        playercamera.Priority = 10;
        spikecamera.Priority = 20;
    }

    public void ActivatePlayerCamera()
    {
        playercamera.Priority = 20;
        spikecamera.Priority = 10;
    }

    public void ShowInstruction(int instructionGroupIndex)
    {
        ShowInstructionAfterDelay(instructionGroupIndex, 0f);
    }

    public void ShowInstructionAfterDelay(int instructionGroupIndex, float delay)
    {
        if (dialougeInteraction == null)
        {
            dialougeInteraction = FindObjectOfType<DialougeInteraction>();
        }

        if (dialougeInteraction == null)
        {
            GameObject dialogueObject = new GameObject("Runtime Dialogue Interaction");
            dialougeInteraction = dialogueObject.AddComponent<DialougeInteraction>();
        }

        if (IsInstructionOpen || instructionRoutine != null)
        {
            return;
        }

        instructionRoutine = StartCoroutine(ShowInstructionRoutine(instructionGroupIndex, delay));
    }

    private IEnumerator ShowInstructionRoutine(int instructionGroupIndex, float delay)
    {
        Debug.Log("Instruction requested: " + instructionGroupIndex);

        if (delay > 0)
        {
            yield return new WaitForSeconds(delay);
        }

        Debug.Log("Instruction opening: " + instructionGroupIndex);
        ServiceLocator.Instance.playerService.StartReading();
        IsInstructionOpen = true;

        if (!dialougeInteraction.StartInstruction(instructionGroupIndex))
        {
            EndInstruction();
        }
    }

    public void EndInstruction()
    {
        instructionRoutine = null;
        IsInstructionOpen = false;
        ServiceLocator.Instance.playerService.StopReading();
    }

    public void MarkJungleRelicCollected()
    {
        IsJungleRelicCollected = true;
    }
   
}






