using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikeController : MonoBehaviour
{
    public enum SpikeState
    {
        moving,
        stopped,
    }

    private Rigidbody2D rb;
    [SerializeField] private SpikeState currentState = SpikeState.stopped;
    [SerializeField] float speed;
    private void Awake()
    {
        rb= GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        if(currentState==SpikeState.moving)
        {
            rb.velocity= Vector2.up*speed;
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Blocker"))
        {
            currentState = SpikeState.stopped;
            this.rb.velocity = Vector2.zero;
            StopAllCoroutines();
            StartCoroutine(DestroyPlatform());
            


        }

        IEnumerator DestroyPlatform()
        {
            ParticleSystem leaf = Instantiate(ServiceLocator.Instance.gamePlayservice.GetLeafPartcileSystem(),
                collision.gameObject.transform.position,
                Quaternion.identity);
            ServiceLocator.Instance.audioService.PlaySFX(SoundTypes.BlockerHit);
            yield return new WaitForSeconds(1f);
            Destroy(leaf);
            Destroy(collision.gameObject);
            currentState = SpikeState.moving;
        }

        if (collision.gameObject.CompareTag("Player"))
        {

            ServiceLocator.Instance.gamePlayservice.PlayerDead();
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            currentState= SpikeState.stopped;
            rb.velocity = Vector2.zero;
        }
    }
    public void ActivateSpike()
    {
        currentState = SpikeState.moving;
    }
}
