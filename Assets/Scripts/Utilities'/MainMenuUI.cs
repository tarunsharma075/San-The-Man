using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject aboutPanel;
    [SerializeField] private GameObject controlPanel;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private Button newGameButton;

    [SerializeField] private int gameplaySceneIndex = 1;

    private void Awake()
    {
       
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (aboutPanel != null) aboutPanel.SetActive(false);
        if (controlPanel != null) controlPanel.SetActive(false);
    }

    private void Start()
    {
        if (newGameButton != null)
        {
           
            newGameButton.onClick.RemoveAllListeners();
            newGameButton.onClick.AddListener(OnClickNewGameButton);
        }
        else
        {
            Debug.LogError("MainMenuUI: newGameButton reference not assigned in Inspector.");
        }
    }

    public void OnClickNewGameButton()
    {
        if (ServiceLocator.Instance != null && ServiceLocator.Instance.audioService != null)
        {
            ServiceLocator.Instance.audioService.PlaySFX(SoundTypes.ButtonClicked);
        }

        SceneManager.LoadScene(gameplaySceneIndex);
    }

    public void OnHitAboutPanel()
    {
        mainMenuPanel.SetActive(false);
        aboutPanel.SetActive(true);
    }

    public void OnHitControlPanel()
    {
        mainMenuPanel.SetActive(false);
        controlPanel.SetActive(true);
    }

    public void OnHitBackAbout()
    {
        aboutPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void OnHitBackControl()
    {
        controlPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}