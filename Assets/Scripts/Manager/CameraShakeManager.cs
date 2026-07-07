using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.CodeDom.Compiler;

public class CameraShakeManager:GenericMonoSingleton<CameraShakeManager>
 {
    [SerializeField] private float impluseforce = 1;


   public void CameraShake(CinemachineImpulseSource impulseSource)
    {
        if (impulseSource == null)
        {
            Debug.LogError("Impulse Source is not assigned in the Inspector!");
            return;
        }
        impulseSource.GenerateImpulse(impluseforce);
    }


}

