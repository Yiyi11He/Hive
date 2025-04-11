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
    [SerializeField] public GameObject questUI;

    public InMemoryVariableStorage variableStorage;
    public AnswerEvaluator answerEvaluator;

    [Header("Cameras")]
    [SerializeField] public GameObject doctorCamera;
    [SerializeField] public GameObject playerMainCamera;


    private string playerInput;
    private bool InputHandlerActive = false;
    private bool isStartingDialogue = false;
    private bool isInputActive = false;

    private void Start()
    {
        dialogueRunner.startAutomatically = false;
        dialogueRunner.startNode = "";
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
        if (InputHandlerActive || isInputActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            doctorCamera.SetActive(true);
            playerMainCamera.SetActive(false);
            questUI.SetActive(false);
        }
    }

    [YarnCommand("StringInput")]
    public void ShowInputField()
    {
        inputCanvas.SetActive(true);
        submitButton.gameObject.SetActive(true);
        inputField.text = "";
        inputField.Select();
        inputField.ActivateInputField();
        InputHandlerActive = true;
        isInputActive = true;
    }

    public void SubmitInput()
    {
        if (isStartingDialogue) return;

        string playerInput = inputField.text;
        Debug.Log("SubmitInput() is sending: " + playerInput);

        if (variableStorage.TryGetValue<string>("$currentInputContext", out var inputContext))
        {
            Debug.Log("SubmitInput() inputContext = " + inputContext);

            if(inputContext == "SeeIn2Days") { return; }
            else if (inputContext == "xray")
            {
                answerEvaluator.EvaluateXrayAnswer(playerInput);
            }
            else if (inputContext == "culture")
            {
                answerEvaluator.EvaluateCultureAnswer(playerInput);
            }
        }
        else
        {
            Debug.LogWarning("SubmitInput(): Could not retrieve $currentInputContext from Yarn storage.");
        }

        variableStorage.SetValue("$playerInput", playerInput);

        
        inputCanvas.SetActive(false);
        submitButton.gameObject.SetActive(false);
        isStartingDialogue = true;
        isInputActive = false;

        if (dialogueRunner.IsDialogueRunning)
        {
            dialogueRunner.Stop();
        }
        StartCoroutine(StartNextDialogue());
    }
    public bool IsInputActive()
    {
        return isInputActive || InputHandlerActive;
    }
    private IEnumerator StartNextDialogue()
    {
        while (dialogueRunner.IsDialogueRunning)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        if (variableStorage.TryGetValue<string>("$currentInputContext", out var inputContext))
        {
            Debug.Log("SubmitInput() inputContext = " + inputContext);

            if (inputContext == "SeeIn2Days")
            {
                dialogueRunner.StartDialogue("NaughtyAnswer");
            }
            else if (inputContext == "xray")
            {
                dialogueRunner.StartDialogue("XrayDay3");
            }
            else if (inputContext == "culture")
            {
                dialogueRunner.StartDialogue("CultureResponse3");
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
