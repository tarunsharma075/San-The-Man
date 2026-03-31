using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


    public class ServiceLocator : GenericMonoSingleton<ServiceLocator>
    {


    private GameManagerService gameManagerService;
    private PlayerService playerService;

    private void Awake()
    {
        playerService = new PlayerService();
        gameManagerService = new GameManagerService();
    }
    private void Start()
    {
        

    }


    


}

