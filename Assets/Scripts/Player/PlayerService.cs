
using JetBrains.Annotations;
using Unity.VisualScripting;

public class PlayerService 
{


    private PlayerController playerController;
    public PlayerService(PlayerController playerController)
    {
        this.playerController= playerController;



    }



    public void TakeDamage(float damage)
    {
        playerController.TakeDamage(damage);
    }


   

}
