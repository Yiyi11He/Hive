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
    private bool interacting = false;
    public void OnInteraction()
    {
        dialogueRunner.StartDialogue("Video1");

        // codetodo
        Debug.Log("Doctor interaction");
        playerMainCamera.SetActive(false);
        doctorCamera.SetActive(true);
        dialogueUI.SetActive(true);
        interacting = true;
        dialogueRunner.onDialogueComplete.AddListener(OnDialogueComplete);
    }


    private void OnDestroy()
    {
        dialogueRunner.onDialogueComplete.RemoveListener(OnDialogueComplete);
    }


    private void OnDialogueComplete()
    {
        EndInteraction();
    }

    public void EndInteraction()
    {
        playerMainCamera.SetActive(true);
        doctorCamera.SetActive(false);
        dialogueUI.SetActive(false);
        interacting = false;
    }
}
