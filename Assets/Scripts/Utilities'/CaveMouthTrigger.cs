using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CaveMouthTrigger : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Cavetrigger());
        }
    }


    private IEnumerator Cavetrigger()
    {
        ServiceLocator.Instance.audioService.Stopbgm();
        ServiceLocator.Instance.audioService.PlaySFX(SoundTypes.PlayerWins);
        ServiceLocator.Instance.playerService.TriggerPlayerEnd();
        yield return new WaitForSeconds(5f);
        ServiceLocator.ResetInstance();
        SceneManager.LoadScene(0);
    }


}
  
