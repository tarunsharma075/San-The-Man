
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerService
{
    private PlayerController playerController;

    public void SetPlayer(PlayerController player)
    {
        playerController = player;
    }

    public void TakeDamage()
    {
        Debug.Log("PlayerDamage is called");
        if (playerController == null) return;

        playerController.TakeDamage();
    }

    public GameObject GetPlayer()
    {
        return playerController.GetPlayerObject();
    }


    
}
