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

    private bool interacting = false;

    private Dictionary<int, string> questDialogueMapping = new Dictionary<int, string>()
    {
        { 0, "Video1" },
        { 3, "afterVideo1" },
        { 10, "Vid2" },
        { 11, "Vid3" },
        { 17, "Day1Afternoon" }, // This quest is actually continued from the last one, find a way to avoid opening another quest.
        { 21, "Day3" }
    };

    private void Awake()
    {
        var playerInteractable = GetComponent<PlayerInteractable>();
        //if (playerInteractable != null && playerInteractable.UIObject == null)
        //{
        //    playerInteractable.UIObject = GameObject.Find("Interaction(E)"); // Adjust the name accordingly
        //}
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

        // Disable further interactions for this quest
        this.enabled = false;
    }

    public void EndInteraction()
    {
        playerMainCamera.SetActive(true);
        doctorCamera.SetActive(false);
        interacting = false;
    }

    private int GetCurrentQuestIndex()
    {
        return questGiver.GetCurrentQuestIndex();
    }
}
