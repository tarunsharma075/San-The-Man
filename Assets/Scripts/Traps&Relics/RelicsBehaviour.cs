using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Cinemachine;
public  class RelicsBehaviour:WorldObject
{
  private CinemachineImpulseSource impulseSource;


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
        StartCoroutine(EndJungleRelic());
    }

   private IEnumerator EndJungleRelic()
    {
        ServiceLocator.Instance.playerService.PlayerWallJumpUnlocked();
        yield return new WaitForSeconds(1f);
        ServiceLocator.Instance.gamePlayservice.ActivateSpikes();



        if (this.gameObject.CompareTag("JungleRelic"))
        {
            this.gameObject.SetActive(false);
        }
    }


}
    

