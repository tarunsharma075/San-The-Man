using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerService 
{
    private GameManager gameManager;
    private GameObject playerprefab;
    private GameObject checkpoint;
    public GameManagerService(GameObject playerprefab, GameObject checkpoint)
    {
        this.playerprefab = playerprefab;
            this.checkpoint = checkpoint;
        gameManager = new GameManager(playerprefab,checkpoint);
        
    }

    public void RespawnPlayer()
    {
        gameManager.RespawnPlayer();
    }
}
