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

    private bool isInteracting = false;

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
            hitObject = hitInfo.collider.gameObject;
            interactable = hitInfo.collider.GetComponentInParent<PlayerInteractable>();

            // Check for PatientRoomFolder
            PatientRoomFolder patientFolder = hitObject.GetComponent<PatientRoomFolder>();
            if (patientFolder != null)
            {
                Debug.Log($"Raycast hit PatientRoomFolder: {hitObject.name}");
                patientFolder.OnHover();
            }

            // Check for AdminFolder
            AdminFolder adminFolder = hitObject.GetComponent<AdminFolder>();
            if (adminFolder != null)
            {
                Debug.Log($"Raycast hit AdminFolder: {hitObject.name}");
                adminFolder.OnHover(); 
            }

            UIHoverControl uiControl = hitObject.GetComponent<UIHoverControl>();
            if (uiControl != null)
            {
                uiControl.OnHover();
            }


            if (hitObject != previousHover)
            {
                if (previousHover != null)
                {
                    Debug.Log($"DISABLE: {previousHover.name}");

                    // Disable previous PatientRoomFolder
                    PatientRoomFolder previousPatientFolder = previousHover.GetComponent<PatientRoomFolder>();
                    if (previousPatientFolder != null)
                    {
                        previousPatientFolder.OnLookAway();
                    }

                    // Disable previous AdminFolder
                    AdminFolder previousAdminFolder = previousHover.GetComponent<AdminFolder>();
                    if (previousAdminFolder != null)
                    {
                        previousAdminFolder.OnLookAway();
                    }

                    UIHoverControl previousUI = previousHover.GetComponent<UIHoverControl>();
                    if (previousUI != null)
                    {
                        previousUI.OnLookAway();
                    }

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

            // Handle previous PatientRoomFolder
            PatientRoomFolder previousPatientFolder = previousHover.GetComponent<PatientRoomFolder>();
            if (previousPatientFolder != null)
            {
                previousPatientFolder.OnLookAway();
            }

            // Handle previous AdminFolder
            AdminFolder previousAdminFolder = previousHover.GetComponent<AdminFolder>();
            if (previousAdminFolder != null)
            {
                previousAdminFolder.OnLookAway();
            }

            UIHoverControl previousUI = previousHover.GetComponent<UIHoverControl>();
            if (previousUI != null)
            {
                previousUI.OnLookAway();
            }

            DisableUI(previousHover);
            previousHover = null;
        }
    }

    private IEnumerator EndInteraction()
    {
        yield return new WaitForSeconds(0.1f);
        isInteracting = false;
    }

    private void OnInteractAction(InputAction.CallbackContext context)
    {
        if (interactable != null)
        {
            if (interactable.UIObject != null)
            {
                interactable.UIObject.SetActive(false);
            }

            isInteracting = true;

            if (interactable.GetComponent<Door>() != null)
            {
                Door door = interactable.GetComponent<Door>();
                door.Interact(transform);
            }
            else
            {
                interactable?.OnInteraction?.Invoke();
            }

            StartCoroutine(EndInteraction());
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
