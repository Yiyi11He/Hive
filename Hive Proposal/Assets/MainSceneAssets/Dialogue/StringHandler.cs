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

    private void Start()
    {
        inputCanvas.SetActive(false);
        submitButton.gameObject.SetActive(false);
        submitButton.onClick.AddListener(SubmitInput);
    }

    private void Update()
    {
        if (InputHandlerActive)
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
    }

    public void SubmitInput()
    {
        playerInput = inputField.text;
        variableStorage.SetValue("$playerName", playerInput);
        inputCanvas.SetActive(false);
        submitButton.gameObject.SetActive(false); 
        StartCoroutine(StartNextDialogue());
    }

    private IEnumerator StartNextDialogue()
    {
        while (dialogueRunner.IsDialogueRunning || dialogueRunner.Dialogue.IsActive)
            yield return null;

        yield return new WaitForSeconds(0.2f);

        dialogueRunner.StartDialogue("NaughtyAnswer");

        submitButton.gameObject.SetActive(true); 
    }
}
