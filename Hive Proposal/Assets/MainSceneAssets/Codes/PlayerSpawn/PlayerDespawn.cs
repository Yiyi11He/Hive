using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDespawn : MonoBehaviour
{
    // Array to hold all UI elements you want to turn off
    [SerializeField] private GameObject[] uiElementsToDisable;

    // The UI element to enable for text animation
    [SerializeField] private GameObject textAnimationUI;

    // Method to be called on interaction

    [SerializeField] private Animator animator;
    [SerializeField] private string triggerName = "DoorAnim";

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

        if (animator != null)
        {
            animator.SetTrigger(triggerName);
        }
    }
}
