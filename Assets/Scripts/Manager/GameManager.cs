
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneTemplate;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager:GenericMonoSingleton<GameManager>
{

   [SerializeField] private GameObject player;
   [SerializeField] private GameObject Checkpoint;
   [SerializeField] private Button newGameButton;
   
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
        Debug.Log("Scene Loaded");
        if (scene.buildIndex != 0)
        {
            RespawnPlayer();
        }
    }

    public void RespawnPlayer()
    {
        Debug.Log("Respawn called");
        if (currentPlayer != null)
         {
                Object.Destroy(currentPlayer);
                currentPlayer = null;
         }

            
         currentPlayer = Instantiate(
                player,
                Checkpoint.transform.position,
                Quaternion.identity
          );
        

    }

    public void OnClickNewGameButton()
    {
        SceneManager.LoadScene(1);
    }
}