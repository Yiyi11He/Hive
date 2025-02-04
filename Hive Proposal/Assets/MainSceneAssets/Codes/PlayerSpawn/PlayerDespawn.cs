using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDespawn : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject[] uiElementsToDisable; // UI elements to turn off
    [SerializeField] private GameObject textAnimationUI;       // UI for text animation
    [SerializeField] private GameObject finalUIElement;        // UI element to enable after animation

    [Header("Animator Settings")]
    [SerializeField] private Animator doorAnimator;           // Animator for the door
    [SerializeField] private string doorTriggerName = "DoorAnim"; // Trigger name for the door animation

    [SerializeField] private Animator sceneOutroAnimator;     // Animator for the scene outro
    [SerializeField] private string outroTriggerName = "SceneOutro"; // Trigger name for the outro animation

    [Header("Quest Integration")]
    [SerializeField] private QuestGiver questGiver;       // Reference to the QuestGiver
    [SerializeField] private int requiredQuestIndex;      // The required quest index to allow interaction

    private bool isPlayerNearby = false;                 // Tracks if the player is near

    private void Update()
    {
        // Check for player interaction (E key) only if the player is nearby
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            int currentQuestIndex = questGiver.GetCurrentQuestIndex();

            if (currentQuestIndex == requiredQuestIndex)
            {
                Debug.Log("Player interaction allowed. Triggering despawn logic.");
                OnInteract();
            }
            else
            {
                Debug.Log($"Interaction denied. Current quest index: {currentQuestIndex}, required: {requiredQuestIndex}");
            }
        }

        // Check if the final UI element is active and enable the mouse
        if (finalUIElement != null && finalUIElement.activeSelf)
        {
            EnableMouse();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player is within range
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log("Player entered interaction range.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the player leaves the range
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            Debug.Log("Player exited interaction range.");
        }
    }

    public void OnInteract()
    {
        // Disable all UI elements in the array
        foreach (GameObject uiElement in uiElementsToDisable)
        {
            if (uiElement != null)
            {
                uiElement.SetActive(false);
            }
        }

        // Enable the text animation UI element
        if (textAnimationUI != null)
        {
            textAnimationUI.SetActive(true);
        }

        // Trigger the door animation
        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger(doorTriggerName);
            Debug.Log("Door animation triggered.");
        }

        // Trigger the scene outro animation and wait for it to complete
        if (sceneOutroAnimator != null)
        {
            sceneOutroAnimator.SetTrigger(outroTriggerName);
            StartCoroutine(WaitForSceneOutroToFinish());
            Debug.Log("Scene outro animation triggered.");
        }

        Debug.Log("Despawn interaction completed.");
    }

    private IEnumerator WaitForSceneOutroToFinish()
    {
        // Wait for the duration of the animation
        float animationLength = sceneOutroAnimator.GetCurrentAnimatorStateInfo(0).length;
        Debug.Log($"Waiting for scene outro animation to finish. Duration: {animationLength}s");
        yield return new WaitForSeconds(animationLength);

        // Enable the final UI element
        if (finalUIElement != null)
        {
            finalUIElement.SetActive(true);
            Debug.Log("Final UI element enabled.");
        }
    }

    private void EnableMouse()
    {
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true;                 // Make the cursor visible
        Debug.Log("Mouse cursor unlocked and visible.");
    }
}
