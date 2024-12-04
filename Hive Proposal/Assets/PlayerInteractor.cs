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

    private Coroutine highlightCoroutine;
    [SerializeField] private Color highlightColorInner = Color.yellow;
    [SerializeField] private Color highlightColorOuter = Color.red;



    private void Awake()
    {
        interactAction.performed += OnInteractAction;
        interactAction.Enable();
    }

    private void OnInteractAction(InputAction.CallbackContext context)
    {
        interactable?.OnInteraction?.Invoke();
    }

    //private void Update()
    //{
    //    // Update what we're hovering over
    //    interactable = null;
    //    if (Physics.Raycast(raycastOrigin.position, raycastOrigin.forward, out RaycastHit hitInfo, maxRange, layerMask.value))
    //    {
    //        hitObject = hitInfo.collider.gameObject;
    //        interactable = hitInfo.collider.GetComponentInParent<PlayerInteractable>();
    //    }
    //}
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

            if (hitObject != previousHover)
            {
                if (previousHover != null)
                    DisableHighlight(previousHover);

                EnableHighlight(hitObject);
                previousHover = hitObject;
            }
        }
        else if (previousHover != null)
        {
            DisableHighlight(previousHover);
            previousHover = null;
        }
    }

    private void EnableHighlight(GameObject obj)
    {
        // Start the blinking effect
        if (highlightCoroutine != null)
            StopCoroutine(highlightCoroutine);

        highlightCoroutine = StartCoroutine(BlinkHighlight(obj));
    }

    private void DisableHighlight(GameObject obj)
    {
        // Stop any blinking effect and reset the object's material
        if (highlightCoroutine != null)
            StopCoroutine(highlightCoroutine);

        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            // Reset to the original material or disable highlight effects
            Material material = renderer.material;
            material.SetColor("_EmissionColor", Color.black); // Disable emission
            material.color = Color.white; // Reset colour
        }
    }

    private IEnumerator BlinkHighlight(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer == null) yield break;

        Material material = renderer.material;

        Color highlightColorInner = Color.yellow; // Replace with your chosen inner ring colour
        Color highlightColorOuter = Color.red;    // Replace with your chosen outer ring colour

        float blinkDuration = 1.5f;
        int blinkCount = 3;
        float singleBlinkDuration = blinkDuration / (blinkCount * 2); // Half of one on/off cycle

        for (int i = 0; i < blinkCount; i++)
        {
            // Enable highlight
            material.SetColor("_EmissionColor", highlightColorInner * 2); // Bright inner ring
            material.color = highlightColorOuter; // Outer ring colour
            material.SetFloat("_Transparency", 0.5f); // Semi-transparent outer ring
            yield return new WaitForSeconds(singleBlinkDuration);

            // Disable highlight
            material.SetColor("_EmissionColor", Color.black);
            material.color = Color.white; // Reset to default colour
            yield return new WaitForSeconds(singleBlinkDuration);
        }

        // Ensure highlight is off at the end
        material.SetColor("_EmissionColor", Color.black);
        material.color = Color.white;
    }


}
