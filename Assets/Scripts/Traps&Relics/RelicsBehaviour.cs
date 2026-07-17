using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Cinemachine;
using TMPro;
public  class RelicsBehaviour:WorldObject
{
  private CinemachineImpulseSource impulseSource;
    [SerializeField] private float spikeCameraDuration = 0.8f;
    [SerializeField] private float directionSignDuration = 5f;


    RelicState currentState;
    protected override void Awake()
    {
      base.Awake();
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(this.gameObject.CompareTag("JungleRelic")&& collision.gameObject.CompareTag("Player"))
        {
            currentRelicState= RelicState.Collected;
            currentState = currentRelicState;
            anim.SetTrigger("End");
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;  
            if (currentState == RelicState.Collected)
            {
                EndOfJungleAnimation();
            }
        }
    }

    public void EndOfJungleAnimation()
    {

        CameraShakeManager.Instance.CameraShake(impulseSource);
        StopAllCoroutines();
        StartCoroutine(EndJungleRelic());
    }

   private IEnumerator EndJungleRelic()
    {
        ServiceLocator.Instance.gamePlayservice.MarkJungleRelicCollected();
        ServiceLocator.Instance.gamePlayservice.ActivatespikeCamera();
        ServiceLocator.Instance.gamePlayservice.IncreaseShurikenNumberByValue(4);
        
        ServiceLocator.Instance.gamePlayservice.ActivateSpikes();

        yield return new WaitForSeconds(spikeCameraDuration);
        ServiceLocator.Instance.gamePlayservice.ActivatePlayerCamera();
        

        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }

        ServiceLocator.Instance.playerService.PlayerWallJumpUnlocked();
        ServiceLocator.Instance.playerService.SetCurrentDirectionSign(PlayerDirection.Up);
        ServiceLocator.Instance.gamePlayservice.WallJumpUnlock();
        yield return new WaitForSeconds(directionSignDuration);

        ServiceLocator.Instance.gamePlayservice.WallJumpInfoStops();
        ServiceLocator.Instance.playerService.DeactivateCurrentActiveSign();

        if (this.gameObject.CompareTag("JungleRelic"))
        {
            Destroy(this.gameObject);
        }
    }


}
    

