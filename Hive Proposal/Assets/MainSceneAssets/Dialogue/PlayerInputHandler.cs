using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Yarn.Unity;
using TMPro;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private DialogueRunner dialogueRunner;
    [SerializeField] private GameObject inputCanvas;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button submitButton;

    public InMemoryVariableStorage variableStorage;

    [Header("Cameras")]
    [SerializeField] public GameObject doctorCamera;
    [SerializeField] public GameObject playerMainCamera;

    private float playerResponse;
    private bool InputHandlerActive = false;
    private bool isStartingDialogue = false;
    private bool isInputActive = false;

    private void Start()
    {
        inputCanvas.SetActive(false);
        submitButton.onClick.AddListener(SubmitInput);
        dialogueRunner.onDialogueComplete.AddListener(OnDialogueComplete);
    }

    private void OnDestroy()
    {
        dialogueRunner.onDialogueComplete.RemoveListener(OnDialogueComplete);
    }
    public bool IsInputActive()
    {
        return isInputActive || InputHandlerActive;
    }

    private void Update()
    {
        if (InputHandlerActive || isInputActive)
            if (InputHandlerActive || isInputActive)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                doctorCamera.SetActive(true);
                playerMainCamera.SetActive(false);               
            }
    }

    [YarnCommand("InputFieldActive")]
    public void ShowInputField()
    {
        inputCanvas.SetActive(true);
        inputField.text = "";
        inputField.ActivateInputField();
        InputHandlerActive = true;
        isInputActive = true;
    }

    public void SubmitInput()
    {
        if (float.TryParse(inputField.text, out playerResponse))
        {
            if (isStartingDialogue) return;

            submitButton.interactable = false;
            isStartingDialogue = true;

            variableStorage.SetValue("$playerResponse", playerResponse);
            inputCanvas.SetActive(false);

            isInputActive = false;

            if (dialogueRunner.IsDialogueRunning)
            {
                dialogueRunner.Stop();
            }

            StartCoroutine(StartNextDialogue());
        }
    }

    private IEnumerator StartNextDialogue()
    {
        while (dialogueRunner.IsDialogueRunning)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);
        if (variableStorage.TryGetValue<string>("$numericInput", out var inputContext))
        {
            if (inputContext == "Hb")
            {
                dialogueRunner.StartDialogue("HbNumberResponse");
            }
            else if (inputContext == "White")
            {
                dialogueRunner.StartDialogue("WhiteNumberResponse");
            }
            else if (inputContext == "CRP")
            {
                dialogueRunner.StartDialogue("CRPNumberResponse");
            }
            else if (inputContext == "Blood")
            {
                dialogueRunner.StartDialogue("BloodGlucoseNumberResponse");
            }
            else if (inputContext == "Insulin")
            {
                dialogueRunner.StartDialogue("InsulinDoseNumberResponse");
            }
            else if (inputContext == "WCC")
            {
                dialogueRunner.StartDialogue("WCCResponseDay3");
            }
            else if (inputContext == "CRP1")
            {
                dialogueRunner.StartDialogue("CRPResponseDay3");
            }
        }

        submitButton.interactable = true;
        isStartingDialogue = false;
    }

    private void OnDialogueComplete()
    {
        if (!isInputActive)
        {
            doctorCamera.SetActive(false);
            playerMainCamera.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            InputHandlerActive = false;
        }
    }
}
