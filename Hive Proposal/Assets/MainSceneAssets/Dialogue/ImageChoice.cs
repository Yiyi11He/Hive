using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class ImageChoice : MonoBehaviour
{
    [Header("UI References")]
    public GameObject choicePanel;         
    public List<Button> choiceButtons;   
    public DialogueRunner dialogueRunner;   

    private bool[] selections;

    void Initialise()
    {
        choicePanel.SetActive(true);
        selections = new bool[choiceButtons.Count];

        // TODO: Add submit button
        // choiceButtons[choiceButtons.Count].onClick.AddListener(SubmitChoices);
    }

    public void ToggleSelection(int index)
    {
        selections[index] = !selections[index];

        ColorBlock colors = choiceButtons[index].colors;
        colors.normalColor = selections[index] ? Color.green : Color.white;
        choiceButtons[index].colors = colors;
    }

    private void SubmitChoices()
    {
        choicePanel.SetActive(false);
        Debug.Log("Choices submitted.");
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
        ResetChoices();
    }

    private void ResetChoices()
    {
        for (int i = 0; i < selections.Length; i++)
        {
            selections[i] = false;
            ColorBlock colors = choiceButtons[i].colors;
            colors.normalColor = Color.white;
            choiceButtons[i].colors = colors;
        }
    }
}
