using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CaveMouthTrigger : MonoBehaviour
{
    [SerializeField] private int collectRelicInstructionGroupIndex = 1;
    [SerializeField] private int defeatBullInstructionGroupIndex = 2;
    private bool isEnding;

    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.gameObject.CompareTag("Player"))
        {
            TryUseCaveMouth();
        }
    }

    private void TryUseCaveMouth()
    {
        if (isEnding || ServiceLocator.Instance.gamePlayservice.IsInstructionOpen())
        {
            return;
        }

        if (!ServiceLocator.Instance.gamePlayservice.IsJungleRelicCollected())
        {
            ServiceLocator.Instance.gamePlayservice.ShowInstruction(collectRelicInstructionGroupIndex);
            return;
        }

        if (!ServiceLocator.Instance.gamePlayservice.IsBullDead())
        {
            ServiceLocator.Instance.gamePlayservice.ShowInstruction(defeatBullInstructionGroupIndex);
            return;
        }

        StartCoroutine(Cavetrigger());
    }

    private IEnumerator Cavetrigger()
    {
        isEnding = true;
        ServiceLocator.Instance.audioService.Stopbgm();
        ServiceLocator.Instance.audioService.PlaySFX(SoundTypes.PlayerWins);
        ServiceLocator.Instance.playerService.TriggerPlayerEnd();
        yield return new WaitForSeconds(5f);
        ServiceLocator.ResetInstance();
        SceneManager.LoadScene(0);
    }


}
  
