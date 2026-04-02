
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneTemplate;
using UnityEngine;

public class GameManager
{

    private GameObject player;
    private GameObject Checkpoint;
    private GameObject currentPlayer;
    public GameManager(GameObject player, GameObject checkpoint)
    {

        this.player = player;
        Checkpoint = checkpoint;
        
        RespawnPlayer();
    }


    public void RespawnPlayer()
    {
       if (currentPlayer != null)
         {
                Object.Destroy(currentPlayer);
                currentPlayer = null;
         }

            
         currentPlayer = Object.Instantiate(
                player,
                Checkpoint.transform.position,
                Quaternion.identity
          );
        

    }
}