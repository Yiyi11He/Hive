 using System;
using System.Collections;
using System.Collections.Generic;
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

    private void Awake()
    {
        interactAction.performed += OnInteractAction;
        interactAction.Enable();
    }

    private void OnInteractAction(InputAction.CallbackContext context)
    {
        interactable?.OnInteraction?.Invoke();
    }

    private void Update()
    {
        // Update what we're hovering over
        interactable = null;
        if (Physics.Raycast(raycastOrigin.position, raycastOrigin.forward, out RaycastHit hitInfo, maxRange, layerMask.value))
        {
            hitObject = hitInfo.collider.gameObject;
            interactable = hitInfo.collider.GetComponentInParent<PlayerInteractable>();
        }
    }


}
