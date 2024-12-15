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

    void Start()
    {
        selections = new bool[choiceButtons.Count - 1];

        for (int i = 0; i < choiceButtons.Count - 1; i++)
        {
            int index = i;
            choiceButtons[i].onClick.AddListener(() => ToggleSelection(index));
        }

        choiceButtons[choiceButtons.Count - 1].onClick.AddListener(SubmitChoices);
    }

    private void ToggleSelection(int index)
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
