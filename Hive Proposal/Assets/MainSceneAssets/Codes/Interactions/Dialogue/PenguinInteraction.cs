using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;

public class PenguinInteraction : MonoBehaviour
{
    public GameObject playerMainCamera;
    public GameObject penguinCamera;
    public DialogueRunner dialogueRunner;
    public QuestGiver questGiver;


    public GameObject questUI;
    public ImageChoice imageChoice;

    private bool interacting = false;

    private Dictionary<int, string> questDialogueMapping = new Dictionary<int, string>()
    {
        { 0, "PenguinWelcome" },

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

            if (!dialogueRunner.IsDialogueRunning)
            {
                dialogueRunner.StartDialogue(dialogueNode);

                playerMainCamera.SetActive(false);
                penguinCamera .SetActive(true);

                questUI.SetActive(false);

                interacting = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                dialogueRunner.onDialogueComplete.AddListener(OnDialogueComplete);
            }
            else
            {
                Debug.LogWarning("Dialogue already running, ignoring duplicate start.");
            }
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

        this.enabled = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void EndInteraction()
    {
        playerMainCamera.SetActive(true);
        penguinCamera .SetActive(false);
        interacting = false;
        questUI.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private int GetCurrentQuestIndex()
    {
        return questGiver.GetCurrentQuestIndex();
    }

    public bool IsInteracting()
    {
        return interacting;
    }
}

