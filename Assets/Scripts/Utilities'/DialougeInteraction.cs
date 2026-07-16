using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialougeInteraction : MonoBehaviour
{

    [System.Serializable]
    public class InstructionGroup
    {
        [TextArea]
        public string[] instructions;
    }

    [SerializeField] private GameObject instructionBox;
    [SerializeField] private TextMeshProUGUI instructionText;
    [SerializeField] private Button nextButton;
    [SerializeField] private Vector3 playerOffset = new Vector3(0, 2f, 0);
    [SerializeField] private Camera worldCamera;

    private Queue<string> sentences;
    private bool isReading;
    private bool isInitialized;
    private Canvas parentCanvas;
    private RectTransform canvasRect;
    private RectTransform instructionBoxRect;

    [SerializeField]
    private InstructionGroup[] instructionGroups;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        sentences = new Queue<string>();

        EnsureDefaultInstructionGroups();
        EnsureInstructionUI();

        if (instructionBox != null)
        {
            instructionBoxRect = instructionBox.GetComponent<RectTransform>();
            parentCanvas = instructionBox.GetComponentInParent<Canvas>();
            canvasRect = parentCanvas != null ? parentCanvas.GetComponent<RectTransform>() : null;
            instructionBox.SetActive(false);
        }

        if (nextButton != null)
        {
            nextButton.onClick.RemoveListener(ShowNextInstruction);
            nextButton.onClick.AddListener(ShowNextInstruction);
        }

        isInitialized = true;
    }

    private void EnsureDefaultInstructionGroups()
    {
        string[][] defaultInstructions = new string[][]
        {
            new string[]
            {
                "The path is clear now.",
                "Head towards the cave entrance on the right."
            },
            new string[]
            {
                "You cannot leave yet.",
                "Find and collect the Ancient Relic first."
            },
            new string[]
            {
                "The relic is yours, but the bull is still alive.",
                "Defeat the bull before entering the cave."
            }
        };

        if (instructionGroups == null || instructionGroups.Length < defaultInstructions.Length)
        {
            InstructionGroup[] resizedGroups = new InstructionGroup[defaultInstructions.Length];

            if (instructionGroups != null)
            {
                for (int i = 0; i < instructionGroups.Length; i++)
                {
                    resizedGroups[i] = instructionGroups[i];
                }
            }

            instructionGroups = resizedGroups;
        }

        for (int i = 0; i < defaultInstructions.Length; i++)
        {
            if (instructionGroups[i] == null)
            {
                instructionGroups[i] = new InstructionGroup();
            }

            if (instructionGroups[i].instructions == null || instructionGroups[i].instructions.Length == 0)
            {
                instructionGroups[i].instructions = defaultInstructions[i];
            }
        }
    }

    private void EnsureInstructionUI()
    {
        if (instructionBox != null && instructionText != null && nextButton != null)
        {
            return;
        }

        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObject = new GameObject("Instruction Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
        }

        if (FindObjectOfType<EventSystem>() == null)
        {
            new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        }

        if (instructionBox == null)
        {
            instructionBox = new GameObject("InstructionBox", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            instructionBox.transform.SetParent(canvas.transform, false);

            RectTransform boxRect = instructionBox.GetComponent<RectTransform>();
            boxRect.sizeDelta = new Vector2(520, 190);

            Image boxImage = instructionBox.GetComponent<Image>();
            boxImage.color = new Color(0f, 0f, 0f, 0.78f);
        }

        if (instructionText == null)
        {
            GameObject textObject = new GameObject("InstructionText", typeof(RectTransform), typeof(TextMeshProUGUI));
            textObject.transform.SetParent(instructionBox.transform, false);

            RectTransform textRect = textObject.GetComponent<RectTransform>();
            textRect.anchorMin = new Vector2(0, 0.35f);
            textRect.anchorMax = new Vector2(1, 1);
            textRect.offsetMin = new Vector2(24, 10);
            textRect.offsetMax = new Vector2(-24, -18);

            instructionText = textObject.GetComponent<TextMeshProUGUI>();
            instructionText.fontSize = 28;
            instructionText.color = Color.white;
            instructionText.alignment = TextAlignmentOptions.Center;
            instructionText.enableWordWrapping = true;
        }

        if (nextButton == null)
        {
            GameObject buttonObject = new GameObject("NextButton", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
            buttonObject.transform.SetParent(instructionBox.transform, false);

            RectTransform buttonRect = buttonObject.GetComponent<RectTransform>();
            buttonRect.anchorMin = new Vector2(0.5f, 0);
            buttonRect.anchorMax = new Vector2(0.5f, 0);
            buttonRect.pivot = new Vector2(0.5f, 0);
            buttonRect.anchoredPosition = new Vector2(0, 18);
            buttonRect.sizeDelta = new Vector2(150, 46);

            Image buttonImage = buttonObject.GetComponent<Image>();
            buttonImage.color = new Color(0.18f, 0.55f, 0.2f, 1f);

            nextButton = buttonObject.GetComponent<Button>();

            GameObject labelObject = new GameObject("Label", typeof(RectTransform), typeof(TextMeshProUGUI));
            labelObject.transform.SetParent(buttonObject.transform, false);

            RectTransform labelRect = labelObject.GetComponent<RectTransform>();
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;

            TextMeshProUGUI label = labelObject.GetComponent<TextMeshProUGUI>();
            label.text = "Next";
            label.fontSize = 24;
            label.color = Color.white;
            label.alignment = TextAlignmentOptions.Center;
        }
    }

    private void LateUpdate()
    {
        if (!isReading)
        {
            return;
        }

        FollowPlayer();
    }

    public bool StartInstruction(int instructionGroupIndex)
    {
        if (!isInitialized)
        {
            Initialize();
        }

        if (instructionBox == null)
        {
            Debug.Log("Instruction Box is not assigned in DialougeInteraction");
            return false;
        }

        if (instructionText == null)
        {
            Debug.Log("Instruction Text is not assigned in DialougeInteraction");
            return false;
        }

        if (nextButton == null)
        {
            Debug.Log("Next Button is not assigned in DialougeInteraction");
            return false;
        }

        if (instructionGroups == null ||
            instructionGroupIndex < 0 ||
            instructionGroupIndex >= instructionGroups.Length)
        {
            Debug.Log("Instruction group index is not valid: " + instructionGroupIndex);
            return false;
        }

        sentences.Clear();

        string[] instructions = instructionGroups[instructionGroupIndex].instructions;
        if (instructions == null)
        {
            Debug.Log("Instruction group has no instructions: " + instructionGroupIndex);
            return false;
        }

        for (int i = 0; i < instructions.Length; i++)
        {
            if (!string.IsNullOrWhiteSpace(instructions[i]))
            {
                sentences.Enqueue(instructions[i]);
            }
        }

        if (sentences.Count == 0)
        {
            Debug.Log("Instruction group is empty: " + instructionGroupIndex);
            return false;
        }

        isReading = true;

        if (instructionBox != null)
        {
            instructionBox.SetActive(true);
        }

        FollowPlayer();
        ShowNextInstruction();
        return true;
    }

    private void FollowPlayer()
    {
        if (instructionBoxRect == null || parentCanvas == null || canvasRect == null)
        {
            return;
        }

        if (ServiceLocator.Instance == null || ServiceLocator.Instance.playerService == null)
        {
            return;
        }

        GameObject player = ServiceLocator.Instance.playerService.GetPlayer();
        if (player == null)
        {
            return;
        }

        Camera cameraToUse = worldCamera != null ? worldCamera : Camera.main;
        if (cameraToUse == null)
        {
            return;
        }

        Vector3 screenPosition = cameraToUse.WorldToScreenPoint(player.transform.position + playerOffset);
        Camera uiCamera = parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay
            ? null
            : parentCanvas.worldCamera;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPosition,
            uiCamera,
            out Vector2 localPoint))
        {
            instructionBoxRect.anchoredPosition = localPoint;
        }
    }

    public void ShowNextInstruction()
    {
        if (!isReading)
        {
            return;
        }

        if (sentences.Count == 0)
        {
            EndInstruction();
            return;
        }

        if (instructionText != null)
        {
            instructionText.text = sentences.Dequeue();
        }
    }

    private void EndInstruction()
    {
        isReading = false;
        sentences.Clear();

        if (instructionBox != null)
        {
            instructionBox.SetActive(false);
        }

        if (ServiceLocator.Instance != null && ServiceLocator.Instance.gamePlayservice != null)
        {
            ServiceLocator.Instance.gamePlayservice.EndInstruction();
        }
    }
}
