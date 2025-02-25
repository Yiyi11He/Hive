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
    private int currentNodeIndex = 0;

    private void Start()
    {
        inputCanvas.SetActive(false);
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

    [YarnCommand("InputFieldActive")]
    public void ShowInputField()
    {
        inputCanvas.SetActive(true);
        inputField.text = "";
        inputField.ActivateInputField();
        InputHandlerActive = true;
    }

    public void SubmitInput()
    {
        if (float.TryParse(inputField.text, out playerResponse))
        {
            variableStorage.SetValue("$playerResponse", playerResponse);
            inputCanvas.SetActive(false);
            submitButton.interactable = false;
            StartCoroutine(StartNextDialogue());
        }
    }

    private IEnumerator StartNextDialogue()
    {
        while (dialogueRunner.IsDialogueRunning || dialogueRunner.Dialogue.IsActive)
            yield return null;

        yield return new WaitForSeconds(0.2f);

        if (currentNodeIndex == 0)
            dialogueRunner.StartDialogue("HbNumberResponse");
        else if (currentNodeIndex == 1)
            dialogueRunner.StartDialogue("WhiteNumberResponse");
        else if (currentNodeIndex == 2)
            dialogueRunner.StartDialogue("CRPNumberResponse");
        else if (currentNodeIndex == 3)
            dialogueRunner.StartDialogue("BloodGlucoseNumberResponse");
        else if (currentNodeIndex == 4)
            dialogueRunner.StartDialogue("InsulinDoseNumberResponse");
        else
            Debug.Log("No more responses left. Dialogue has concluded.");

        currentNodeIndex++;
        submitButton.interactable = true;
    }
}
