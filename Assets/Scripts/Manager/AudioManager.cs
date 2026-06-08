using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : GenericMonoSingleton<AudioManager>
{

    private AudioSource backgroundScore;
    private AudioSource gameSFXSounds;

    [SerializeField] private AudioClip backgroundTheme;
    [SerializeField] private AudioClip playerJump;
    [SerializeField] private AudioClip playerHit;
    [SerializeField] private AudioClip FruitCollected;
    [SerializeField] private AudioClip playerDeath;
    [SerializeField] private AudioClip ButtonClicked;

    private void Awake()
    {
        backgroundScore = GetComponent<AudioSource>();
        gameSFXSounds = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (backgroundScore != null && backgroundTheme != null)
        {
            backgroundScore.Play();
        }
        else
        {
            if (backgroundScore == null)
            {
                Debug.Log("backgroubd Audio Source is null");
            }
            else if (backgroundTheme == null)
            {
                Debug.Log("background theme is null");
            }
        }


    }

    public void PlaySFXSounds(SoundTypes soundToUsed)
    {
        if(gameSFXSounds == null)
        {
            Debug.Log("Game SFX Audio Source is null");
            return;
        }
        switch (soundToUsed)
        {
            case SoundTypes.PlayerJump:
                {
                    if(playerJump == null)
                    {
                        Debug.Log("Player Jump sound is null");
                        return;
                    }
                    gameSFXSounds.PlayOneShot(playerJump);
                    break;
                }

            case SoundTypes.PlayerDeath: {
                    if(playerDeath == null)
                    {
                        Debug.Log("Player Hit sound is null");
                        return;
                    }
                    gameSFXSounds.PlayOneShot(playerHit);
                    break;
                }

            case SoundTypes.PlayerHit:
                {
                    if (playerHit == null)
                    {
                        Debug.Log("Player Hit sound is null");
                        return;
                    }
                    gameSFXSounds.PlayOneShot(playerHit);

                    break;
                }

                case SoundTypes.FruitCollect:
                {
                    if (FruitCollected == null)
                    {
                        Debug.Log("Fruit collect sound is null");
                        return;
                    }
                    gameSFXSounds.PlayOneShot(FruitCollected);
                    break;
                }

                case SoundTypes.ButtonClicked:
                {
                    if (ButtonClicked == null)
                    {
                        Debug.Log("Button clicked sound is null");
                        return;
                    }
                    gameSFXSounds.PlayOneShot(ButtonClicked);
                    break;
                }
            default: {

                    Debug.Log("There is no sound assigned to this sound type");
                    break;
                }

        }
    }
}

