using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedBackground : MonoBehaviour
{
    private MeshRenderer meshrendrer;
    [SerializeField] private Vector2 meshMovement;

    private void Awake()
    {
        meshrendrer = GetComponent<MeshRenderer>();
    }


    void Update()
    {
        meshrendrer.material.mainTextureOffset += meshMovement * Time.deltaTime;
    }
}



