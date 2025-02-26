using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn;
using Yarn.Unity;

public class DoctorInteractable : MonoBehaviour
{
    public GameObject playerMainCamera;
    public GameObject doctorCamera;
    public DialogueRunner dialogueRunner;
    public QuestGiver questGiver;

    public GameObject questUI;
    public ImageChoice imageChoice;

    private bool interacting = false;

    private Dictionary<int, string> questDialogueMapping = new Dictionary<int, string>()
    {
        { 0, "Video1" },
        { 2, "afterVideo1" },
        { 4, "Vid2" },
        { 13, "Vid3" },
        { 25, "Day1Afternoon" }, // This quest is actually continued from the last one, find a way to avoid opening another quest.
        { 29, "Day3" }
    };

    private void Awake()
    {
        var playerInteractable = GetComponent<PlayerInteractable>();

    }

    public void OnInteraction()
    {
        // Sync currentQuestNumber with the QuestGiver's current index
        int currentQuestNumber = GetCurrentQuestIndex();

        if (questDialogueMapping.ContainsKey(currentQuestNumber))
        {
            string dialogueNode = questDialogueMapping[currentQuestNumber];
            dialogueRunner.StartDialogue(dialogueNode);

            Debug.Log($"Starting dialogue node: {dialogueNode}");

            // Switch to the doctor's camera
            playerMainCamera.SetActive(false);
            doctorCamera.SetActive(true);

            questUI.SetActive(false);

            interacting = true;
            dialogueRunner.onDialogueComplete.AddListener(OnDialogueComplete);
        }
    }

    private void OnDestroy()
    {
        dialogueRunner.onDialogueComplete.RemoveListener(OnDialogueComplete);
    }

    private void OnDialogueComplete()
    {
        Debug.Log("Dialogue complete.");
        EndInteraction();
        questUI.SetActive(true);

        // Call HideImageChoices to hide the image choices panel
        if (imageChoice != null)
        {
            imageChoice.HideImageChoices();
        }

        // Disable further interactions for this quest
        this.enabled = false;
    }

    public void EndInteraction()
    {
        playerMainCamera.SetActive(true);
        doctorCamera.SetActive(false);
        interacting = false;
        questUI.SetActive(true);
    }

    private int GetCurrentQuestIndex()
    {
        return questGiver.GetCurrentQuestIndex();
    }
}
