using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class ImageChoice : MonoBehaviour
{
    [Header("UI References")]
    public GameObject choicePanel;
    public List<Button> choiceButtons;
    public DialogueRunner dialogueRunner;
    public GameObject doctorCamera;
    public GameObject playerMainCamera;

    public InMemoryVariableStorage variableStorage; // Reference to Yarn's variable storage

    private string[] yarnVariableNames = { "$Swab1", "$Swab2", "$Swab3", "$Swab4" }; // Corresponding Yarn variables

    private bool isImageChoiceActive = false;

    void Start()
    {
        Initialise();
        ResetYarnVariables();
    }
    void Update()
    {
        if (isImageChoiceActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            doctorCamera.SetActive(true);

            playerMainCamera.SetActive(false);
        }
    }
    void Initialise()
    {
        choicePanel.SetActive(false); // Start with the panel hidden

        for (int i = 0; i < choiceButtons.Count; i++)
        {
            int index = i; // Prevent closure issues
            choiceButtons[i].onClick.AddListener(() => ToggleSelection(index));
        }
    }
    private void ResetYarnVariables()
    {
        if (variableStorage != null)
        {
            foreach (string variableName in yarnVariableNames)
            {
                variableStorage.SetValue(variableName, false); // Set all variables to false initially
                Debug.Log($"{variableName} initialised to false.");
            }
        }
    }

    public void ToggleSelection(int index)
    {
        if (index < 0 || index >= choiceButtons.Count)
        {
            Debug.LogWarning($"Invalid button index: {index}");
            return;
        }


        if (variableStorage != null && index < yarnVariableNames.Length)
        {

            bool currentState = false;
            variableStorage.TryGetValue(yarnVariableNames[index], out currentState);

            // Toggle the state
            bool newState = !currentState;
            variableStorage.SetValue(yarnVariableNames[index], newState);

            Debug.Log($"{yarnVariableNames[index]} set to {newState}");

            // Update the button's color
            ColorBlock colors = choiceButtons[index].colors;
            colors.normalColor = newState ? Color.green : Color.white;
            choiceButtons[index].colors = colors;

            // Force the button to visually update
            choiceButtons[index].GetComponentInChildren<Image>().color = colors.normalColor;
            choiceButtons[index].OnDeselect(null); // Ensure Unity updates the button state
        }
    }


    public void SubmitChoicesToDialogue()
    {
        // Stop any running dialogue
        if (dialogueRunner.IsDialogueRunning)
        {
            dialogueRunner.Stop();
        }

        // Hide the choice panel
        HideImageChoices();

        // Start the results node
        dialogueRunner.StartDialogue("PictureResults");

        isImageChoiceActive = false; // Reset the flag
    }

    [YarnCommand("hide_image_choices")]
    public void HideImageChoices()
    {
        choicePanel.SetActive(false);
        Debug.Log("Image choices hidden.");
    }

    [YarnCommand("show_image_choices")]
    public void ShowImageChoice()
    {
        Initialise();

        choicePanel.SetActive(true);

        isImageChoiceActive = true;
    }
}
