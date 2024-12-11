 using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private Transform raycastOrigin;
    [SerializeField] private InputAction interactAction;

    [SerializeField] private float maxRange;
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private PlayerInteractable interactable;
    [SerializeField] private GameObject hitObject;

    private Vector3 lastPosition;
    private Quaternion lastRotation;
    private GameObject previousHover;


    private void Awake()
    {
        interactAction.performed += OnInteractAction;
        interactAction.Enable();
    }



    private void Update()
    {
        if (transform.position != lastPosition || transform.rotation != lastRotation)
        {
            PerformRaycast();
            lastPosition = transform.position;
            lastRotation = transform.rotation;
        }
    }

    private void PerformRaycast()
    {
        interactable = null;
        if (Physics.Raycast(raycastOrigin.position, raycastOrigin.forward, out RaycastHit hitInfo, maxRange, layerMask))
        {
            Debug.Log($"Hit: {hitInfo.collider.name}");
            hitObject = hitInfo.collider.gameObject;
            interactable = hitInfo.collider.GetComponentInParent<PlayerInteractable>();

            if (hitObject != previousHover)
            {
                if (previousHover != null)
                {
                    Debug.Log($"DISABLE: {previousHover.name}");
                    DisableUI(previousHover);
                }

                Debug.Log($"ENABLE: {hitObject.name}");

                EnableUI(hitObject);
                previousHover = hitObject;
            }
        }
        else if (previousHover != null)
        {
            Debug.Log("Raycast missed.");
            DisableUI(previousHover);
            previousHover = null;
        }
    }

    private void OnInteractAction(InputAction.CallbackContext context)
    {
        if (interactable != null && interactable.GetComponent<Door>() != null)
        {
            Door door = interactable.GetComponent<Door>();
            door.Interact(transform); // Pass the player's transform
        }
        else
        {
            interactable?.OnInteraction?.Invoke();
        }
    }


    private void EnableUI(GameObject obj)
    {
        var interactableComponent = obj.GetComponent<PlayerInteractable>();
        interactableComponent?.UIObject?.SetActive(true);

    }

    private void DisableUI(GameObject obj)
    {
        var interactableComponent = obj.GetComponent<PlayerInteractable>();
        interactableComponent?.UIObject?.SetActive(false);

    }
}