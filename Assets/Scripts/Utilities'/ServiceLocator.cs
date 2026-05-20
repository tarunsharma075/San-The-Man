using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServiceLocator : GenericMonoSingleton<ServiceLocator>
{


   
    public PlayerService playerService { get; private set; }
    public GamePlayManagerService gamePlayservice { get; private set; }



    [SerializeField] private GamePlayManager gamePlayManager;


    private void Awake()
    {
        base.Awake();
        playerService = new PlayerService();
        

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
        gamePlayManager = FindObjectOfType<GamePlayManager>();

        gamePlayservice = new GamePlayManagerService(gamePlayManager);
    }

}

