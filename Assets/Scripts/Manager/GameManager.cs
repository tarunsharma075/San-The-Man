
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneTemplate;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager:GenericMonoSingleton<GameManager>
{

    [SerializeField]  private GameObject player;
    private GameObject Checkpoint;
   [SerializeField] private Button newGameButton;
    private float currentHealth;
     private GameObject currentPlayer;

   protected override void Awake()
    {
        base.Awake();
        
    }

    private void Start()
    {
        newGameButton.onClick.AddListener(OnClickNewGameButton);

       
    }


  

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        if (scene.buildIndex != 0)
        {
            RespawnPlayer();
        }

        
    }

    public void RespawnPlayer()
    {
        // Guard: catch missing prefab reference early
        if (player == null)
        {
            Debug.LogError("Player prefab is null! Assign a PREFAB (not a scene object) to the 'player' field in GameManager.");
            return;
        }

        if (currentPlayer != null)
        {
            Destroy(currentPlayer);
            currentPlayer = null;
        }

        Checkpoint = GameObject.FindGameObjectWithTag("Checkpoint");

        if (Checkpoint == null)
        {
            Debug.LogError("Checkpoint not found in the scene.");
            return;
        }

        currentPlayer = Instantiate(player, Checkpoint.transform.position, Quaternion.identity);
    }

    public void OnClickNewGameButton()
    {
        SceneManager.LoadScene(1);
    }
}