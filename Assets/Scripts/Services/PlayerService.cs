
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

    public void PlayerDie()
    {
        playerController.PlayreDie();
    }


    public PlayerState GetPlayerState() {


        if (playerController == null)
            return PlayerState.NotSpwaned;


        return playerController.GetPlayerCurrentState();

    }

    

}
