
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
        
        if (playerController == null) return;

        playerController.TakeDamage();
    }

    public GameObject GetPlayer()
    {
        return playerController.GetPlayerObject();
    }

    public void EnemyOverStunJump()
    {
        playerController.StunJump();
    }

}
