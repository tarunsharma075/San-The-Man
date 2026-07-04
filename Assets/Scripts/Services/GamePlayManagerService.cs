using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlayManagerService 
{
   private GamePlayManager gamePlayManager;

public GamePlayManagerService(GamePlayManager gamePlayManager)
{
        this.gamePlayManager = gamePlayManager;
}

public void SetNumberofFruits(int currentNumberOffruits) { 
    
   gamePlayManager.SetFruits(currentNumberOffruits);


    }


public void DecreaseHealth()
    {
        gamePlayManager.DecreaseHealth();
    }



public int IncreaseNumberOFShurikens()
    {
        return gamePlayManager.IncreaseNumberofShurikens();
    }

    public int DecreaseNumberOFShurikens()
    {
        return gamePlayManager.DecreaseNumberofShurikens();
    }

    public void PlayerDead()
    {
        gamePlayManager.PlayerDead();
    }
     
   public void OnHitWithShuriken()
    {
        gamePlayManager.OnHitShuriken();
    }


}
