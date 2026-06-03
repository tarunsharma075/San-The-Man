
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.IO.LowLevel.Unsafe;
using Cinemachine;
public class GameManager:GenericMonoSingleton<GameManager>
{

    [SerializeField]  private GameObject player;
    private GameObject Checkpoint;
   [SerializeField] private Button newGameButton;
    
     private GameObject currentPlayer;

    private CinemachineVirtualCamera virtualCamera;

    protected override void Awake()
    {
        base.Awake();
        
    }

    private void Start()
    {
        newGameButton.onClick.AddListener(OnClickNewGameButton);

       
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
        
        if (scene.buildIndex != 0)
        {
            SetVirtualCamera();
            RespawnPlayer();
        }

        
    }

    public void RespawnPlayer()
    {
        // Guard: catch missing prefab reference early
        if (player == null)
        {
            Debug.LogError("Player prefab is null! Assign a PREFAB (not a scene object) to the 'player' field in GameManager.");
            return;
        }

        if (currentPlayer != null)
        {
            Destroy(currentPlayer);
            currentPlayer = null;
        }

        Checkpoint = GameObject.FindGameObjectWithTag("Checkpoint");

        if (Checkpoint == null)
        {
            Debug.LogError("Checkpoint not found in the scene.");
            return;
        }

        currentPlayer = Instantiate(player, Checkpoint.transform.position, Quaternion.identity);
        SetCameraFollow();
        
    }

    public void OnClickNewGameButton()
    {
        SceneManager.LoadScene(1);
    }

    private void SetVirtualCamera()
    {
        GameObject vcamObject = GameObject.FindGameObjectWithTag("virtualCamera");

        if (vcamObject == null)
        {
            Debug.LogError("Virtual Camera not found.");
            return;
        }

        virtualCamera = vcamObject.GetComponent<CinemachineVirtualCamera>();

        if (virtualCamera == null)
        {
            Debug.LogError("CinemachineVirtualCamera component not found.");
        }
    }

    private void SetCameraFollow()
    {
        if (virtualCamera == null)
        {
            SetVirtualCamera();
        }

        if (virtualCamera == null || currentPlayer == null)
        {
            return;
        }

        virtualCamera.Follow = currentPlayer.transform;
        virtualCamera.LookAt = null;
        virtualCamera.PreviousStateIsValid = false;

        //Debug.Log("Camera now following: " + virtualCamera.Follow.name);
    }


}