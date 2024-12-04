using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn;
using Yarn.Unity;


public class DoctorInteractable : MonoBehaviour
{
    public GameObject playerMainCamera;
    public GameObject doctorCamera;
    public GameObject dialogueUI;
    public DialogueRunner dialogueRunner;
    public QuestGiver questGiver;

    private bool interacting = false;


    private Dictionary<int, string> questDialogueMapping = new Dictionary<int, string>()
    {
        //update this after completing seat interact.
        { 0, "Video1" },
        { 3, "afterVideo1" },
        { 10, "Vid2" },
        { 11, "Vid3"},
        { 17, "Day1Afternoon"},//this quest is actually continued from last one, find an way to avoid opening another quest.
        { 21, "Day3" }
    };
    public void OnInteraction()
    {
        // Sync currentQuestNumber with the QuestGiver's current index
        int currentQuestNumber = GetCurrentQuestIndex();

        if (questDialogueMapping.ContainsKey(currentQuestNumber))
        {
            string dialogueNode = questDialogueMapping[currentQuestNumber];
            dialogueRunner.StartDialogue(dialogueNode);

            Debug.Log($"Starting dialogue node: {dialogueNode}");

            // Switch to the doctor's camera and enable the dialogue UI
            playerMainCamera.SetActive(false);
            doctorCamera.SetActive(true);
            dialogueUI.SetActive(true);

            interacting = true;
            dialogueRunner.onDialogueComplete.AddListener(OnDialogueComplete);
        }
        else
        {
            Debug.LogWarning($"No dialogue found for quest index: {currentQuestNumber}");
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

        // Disable further interactions for this quest
        this.enabled = false;
    }

    public void EndInteraction()
    {
        playerMainCamera.SetActive(true);
        doctorCamera.SetActive(false);
        dialogueUI.SetActive(false);
        interacting = false;
    }
    private int GetCurrentQuestIndex()
    {
        return questGiver.GetCurrentQuestIndex();
    }
}