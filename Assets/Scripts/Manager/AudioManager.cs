using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : GenericMonoSingleton<AudioManager>
{

    [SerializeField] private AudioSource backgroundScore;
    [SerializeField] private AudioSource gameSFXSounds;

    [SerializeField] private AudioClip backgroundTheme;
    [SerializeField] private AudioClip playerJump;
    [SerializeField] private AudioClip playerHit;
    [SerializeField] private AudioClip FruitCollected;
    [SerializeField] private AudioClip playerDeath;
    [SerializeField] private AudioClip buttonClicked;
    [SerializeField] private AudioClip enemyOverJump;
    [SerializeField] private AudioClip enemystun;
    [SerializeField] private AudioClip playerWins;
    [SerializeField] private AudioClip blockerHit;
    protected override void Awake()
    {
        base.Awake();
        AssignClip();
    }

   
 

        private void Start()
        {
        if (Instance != this) return;

        Debug.Log("AudioManager Start called, playing bg music. Clip: " + backgroundScore.clip);
        backgroundScore.Play();


    }


    

    private void AssignClip()
    {
        if (backgroundScore == null || backgroundTheme == null)
        {
            Debug.LogError("backgroundScore or backgroundTheme is not assigned in the Inspector!");
            return;
        }
        backgroundScore.clip = backgroundTheme;
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
                    gameSFXSounds.PlayOneShot(playerDeath);
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
                    if (buttonClicked == null)
                    {
                        Debug.Log("Button clicked sound is null");
                        return;
                    }
                    gameSFXSounds.PlayOneShot(buttonClicked);
                    break;
                }

            case SoundTypes.EnemyOverJump:
                {
                    if (enemyOverJump== null)
                    {
                        Debug.Log("Enemy jump over  sound is null");
                        return;
                    }
                    gameSFXSounds.PlayOneShot(enemyOverJump);
                    break;
                }

                case SoundTypes.EnemyStun:
                {
                    if (enemystun == null)
                    {
                        Debug.Log("Enemy stun sound is null");
                        return;
                    }
                    gameSFXSounds.PlayOneShot(enemystun);
                    break;
                }

            case SoundTypes.PlayerWins:
                {

                    if(playerWins == null)
                    {
                        Debug.Log( "sound is null");
                        return;
                    }
                    gameSFXSounds.PlayOneShot(playerWins);
                    break;
                }


            case SoundTypes.BlockerHit:

                {

                    if (blockerHit == null)
                    {
                        Debug.Log("sound is null");
                        return;
                    }
                    gameSFXSounds.PlayOneShot(blockerHit);
                    break;
                }

            default: {

                    Debug.Log("There is no sound assigned to this sound type");
                    break;
                }

        }
    }


    public void StopBgm()
    {
        backgroundScore.Stop();
    }


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (ServiceLocator.Instance != null)
            ServiceLocator.Instance.RegisterAudioManager(this);

        if (scene.buildIndex == 0)
        {
            backgroundScore.Play();
        }
    }
}

