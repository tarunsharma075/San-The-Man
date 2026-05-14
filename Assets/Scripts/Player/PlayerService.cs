
using JetBrains.Annotations;
using Unity.VisualScripting;

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
}
