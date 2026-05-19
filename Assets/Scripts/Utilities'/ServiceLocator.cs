using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class ServiceLocator : GenericMonoSingleton<ServiceLocator>
{


   public  GameManager  gameManager { get; private set; }
    public PlayerService playerService { get; private set; }


    //Dependencies
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject checkpointPrefab;
    

    private void Start()
    {
        playerService = new PlayerService();
       gameManager= new GameManager();
    }


}

