using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Yarn.Unity;
using TMPro;

public class StringHandler : MonoBehaviour
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

    private string playerInput;
    private bool InputHandlerActive = false;
    private bool isStartingDialogue = false;
    private bool isInputActive = false; // ? Tracks if input is currently being processed

    private void Start()
    {
        inputCanvas.SetActive(false);
        submitButton.gameObject.SetActive(false);
        submitButton.onClick.AddListener(SubmitInput);

        dialogueRunner.onDialogueComplete.AddListener(OnDialogueComplete);
    }

    private void OnDestroy()
    {
        dialogueRunner.onDialogueComplete.RemoveListener(OnDialogueComplete);
    }

    private void Update()
    {
        if (InputHandlerActive || isInputActive) // ? Keep doctor camera active during input
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            doctorCamera.SetActive(true);
            playerMainCamera.SetActive(false);
        }
    }

    [YarnCommand("StringInput")]
    public void ShowInputField()
    {
        inputCanvas.SetActive(true);
        submitButton.gameObject.SetActive(true);
        inputField.text = "";
        inputField.ActivateInputField();
        InputHandlerActive = true;
        isInputActive = true; // ? Track that input is currently active
    }

    public void SubmitInput()
    {
        if (isStartingDialogue) return;

        playerInput = inputField.text;
        variableStorage.SetValue("$playerName", playerInput);
        inputCanvas.SetActive(false);
        submitButton.gameObject.SetActive(false);

        isStartingDialogue = true;
        isInputActive = false; // ? Mark input as completed
        StartCoroutine(StartNextDialogue());
    }

    private IEnumerator StartNextDialogue()
    {
        while (dialogueRunner.IsDialogueRunning || dialogueRunner.Dialogue.IsActive)
            yield return null;

        yield return new WaitForSeconds(0.2f);

        if (!dialogueRunner.IsDialogueRunning)
            dialogueRunner.StartDialogue("NaughtyAnswer");

        submitButton.gameObject.SetActive(true);
        isStartingDialogue = false;
    }

    private void OnDialogueComplete()
    {
        if (!isInputActive) // ? Only unlock after input is completely finished
        {
            doctorCamera.SetActive(false);
            playerMainCamera.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            InputHandlerActive = false;
        }
    }
}
