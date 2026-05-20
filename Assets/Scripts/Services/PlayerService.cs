
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

    public void TakeDamage(float damage)
    {
        if (playerController == null) return;

        playerController.TakeDamage(damage);
    }

    public GameObject GetPlayer()
    {
        return playerController.GetPlayerObject();
    }


    
}
