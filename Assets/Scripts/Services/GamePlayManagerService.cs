using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePlayManagerService
{
    private GamePlayManager gamePlayManager;

    public GamePlayManagerService(GamePlayManager gamePlayManager)
    {
        this.gamePlayManager = gamePlayManager;
    }

    public void SetNumberofFruits(int currentNumberOffruits)
    {

        gamePlayManager.SetFruits(currentNumberOffruits);


    }


    public void DecreaseHealth()
    {
        gamePlayManager.DecreaseHealth();
    }



  
    public int DecreaseNumberOFShurikens()
    {
        return gamePlayManager.DecreaseNumberofShurikens();
    }


    public float GetNumberOfShurikens()
    {
        return gamePlayManager.GetCurrentShurikenNumber();
    }
    public void PlayerDead()
    {
        gamePlayManager.PlayerDead();
    }

    public void OnHitWithShuriken()
    {
        gamePlayManager.OnHitShuriken();
    }

    public void ActivateSpikes()
    {
        gamePlayManager.SpikeActivated();

    }


    public void UpdateEnemyHealthUI(Image enemyGreenHealthbar, int maxhealth, int currenthealth)
    {
        gamePlayManager.UpdateEnemeyUI(enemyGreenHealthbar, maxhealth, currenthealth);



    }

    public void IncreasePlayerHealth()
    {
        gamePlayManager.IncreasePlayerHealth();
    }

    public void IncreaseShurikenNumberByValue(int value)
    {
        gamePlayManager.IncreaseShurikenNumberByValue((int)value);
    }

    public ParticleSystem GetLeafPartcileSystem()
    {
        return gamePlayManager.GetLeafPartcileSystem();



    }


    public void ActivatespikeCamera()
    {
        gamePlayManager.ActivateSpikeCamera();
    }


    public void ActivatePlayerCamera()
    {
        gamePlayManager.ActivatePlayerCamera();
    }

}
