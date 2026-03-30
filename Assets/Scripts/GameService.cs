using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


    public class GameService : MonoBehaviour
    {


    private GameManagerService gameManagerService;
    private PlayerService playerService;

    private static GameService instance;
    public static GameService Instance { get { return instance; } }

    private void Awake()
    {
        SingletonCreation();

    }

    private void SingletonCreation()
    {
        if (instance = null)
        {
            instance = this;
        }
        else if (instance != null)
        {

            Destroy(gameObject);
        }
    }

    private void Start()
    {

        playerService = new PlayerService();
        gameManagerService = new GameManagerService();

    }


    


}

