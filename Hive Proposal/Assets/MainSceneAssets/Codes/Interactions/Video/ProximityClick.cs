using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityClick : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionDistance = 3.0f; // Distance within which interaction is allowed
    public GameObject interactionPrompt;    // Optional UI prompt to show interaction availability

    private Transform playerTransform;      // Player's position reference
    private bool isPlayerClose = false;
    private bool isInteractionDisabled = false; // Tracks if interaction is disabled

    private void Start()
    {
        // Find the player object by tag (ensure the player is tagged "Player")
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false); // Hide the prompt initially
        }
    }

    private void Update()
    {
        if (isInteractionDisabled || playerTransform == null) return;

        // Calculate distance between the player and this object
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // Check if the player is close enough
        if (distanceToPlayer <= interactionDistance)
        {
            if (!isPlayerClose)
            {
                isPlayerClose = true;
                Debug.Log("Player is close enough to interact.");

                // Show the interaction prompt
                if (interactionPrompt != null)
                {
                    interactionPrompt.SetActive(true);
                }
            }

            // Check for mouse click
            if (Input.GetMouseButtonDown(0)) // Left mouse button
            {
                Debug.Log("Player clicked on the door.");
                InteractWithObject();
            }
        }
        else
        {
            if (isPlayerClose)
            {
                isPlayerClose = false;
                Debug.Log("Player is too far to interact.");

                // Hide the interaction prompt
                if (interactionPrompt != null)
                {
                    interactionPrompt.SetActive(false);
                }
            }
        }
    }

    private void InteractWithObject()
    {
        // Trigger the PlayPause script's OnMouseDown logic
        PlayPause playPause = GetComponent<PlayPause>();
        if (playPause != null)
        {
            Debug.Log("Calling PlayPause OnMouseDown.");
            playPause.SendMessage("OnMouseDown"); // Call OnMouseDown

            // Disable interaction after triggering the video
            DisableInteraction();
        }
        else
        {
            Debug.LogWarning("PlayPause script not found on this object.");
        }
    }

    private void DisableInteraction()
    {
        Debug.Log("Disabling interaction for this object.");
        isInteractionDisabled = true;

        // Hide the interaction prompt
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }
}
