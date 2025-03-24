using System.Collections;
using UnityEngine;

public class PatientRoomFolder : MonoBehaviour
{
    [Header("Chart to Control")]
    public GameObject chart;       // Assign GPReferral for GP Collider, ProgressNotes for Progress Collider
    public GameObject otherChart;  // The other chart to be disabled

    [Header("UI Panels")]
    public GameObject uiMainFolder; // The UI main folder (PatientFolder)
    public GameObject uiSubFolder;  // The UI subfolder (GPReferral UI or ProgressNotes UI)

    [Header("Additional UI Elements to Hide")]
    public GameObject uiToHide1; // UI element that should hide when UI mode is active
    public GameObject uiToHide2; // Another UI element to hide when UI mode is active

    [Header("Player Components")]
    public MonoBehaviour playerMovementScript; // Reference to the player's movement script
    public MonoBehaviour playerLookScript; // Reference to the player's mouse look script

    [Header("Movement Settings")]
    public Transform startPosition; // Assign the hidden position
    public Transform endPosition;   // Assign the elevated visible position
    public float moveSpeed = 3f;    // Speed of movement

    private bool isMoving = false;
    private bool isHovered = false; // Tracks if player is hovering over the collider
    private bool isUIActive = false; // Tracks if UI is currently displayed

    private static PatientRoomFolder activeFolder = null; // Tracks the currently active chart

    private void Start()
    {
        if (chart != null)
        {
            chart.SetActive(false); 
            chart.transform.position = startPosition.position;
            chart.transform.rotation = startPosition.rotation;
        }

        if (uiMainFolder != null)
        {
            uiMainFolder.SetActive(false); 
        }

        if (uiSubFolder != null)
        {
            uiSubFolder.SetActive(false); 
        }

        ShowUIElements();


        EnablePlayerControl();
    }

    private void Update()
    {
        if (isHovered && Input.GetKeyDown(KeyCode.E))
        {
            ActivateUI();
        }

        if (isUIActive && Input.GetKeyDown(KeyCode.Q))
        {
            ExitUIMode();
        }
    }

    public void OnHover()
    {
        if (chart != null && !isMoving)
        {


            if (activeFolder != null && activeFolder != this)
            {
                activeFolder.HideChart();
            }


            activeFolder = this;

            StartCoroutine(MoveChart(chart, endPosition.position, endPosition.rotation, true));
        }

        isHovered = true; 
    }

    public void OnLookAway()
    {
        isHovered = false; 
        if (chart != null && !isMoving)
        {
            StartCoroutine(MoveChart(chart, startPosition.position, startPosition.rotation, false));
        }


        if (isUIActive)
        {
            ExitUIMode();
        }
    }

    private void HideChart()
    {
        if (chart != null && chart.activeSelf)
        {
            StartCoroutine(MoveChart(chart, startPosition.position, startPosition.rotation, false));
        }

        if (isUIActive)
        {
            ExitUIMode();
        }
    }

    private void ActivateUI()
    {
        if (!isUIActive)
        {
            // Activate UI components
            if (uiMainFolder != null)
            {
                uiMainFolder.SetActive(true); // Ensure main UI folder is visible
            }

            if (uiSubFolder != null)
            {
                uiSubFolder.SetActive(true); // Ensure sub UI is visible
            }

            // Hide additional UI elements
            HideUIElements();

            // Disable player movement and camera look
            DisablePlayerControl();

            isUIActive = true; // Mark UI as active
        }
    }

    public void ExitUIMode()
    {

        // Hide UI panels
        if (uiSubFolder != null)
        {
            uiSubFolder.SetActive(false);
        }

        if (uiMainFolder != null)
        {
            uiMainFolder.SetActive(false);
        }

        // Hide charts if they are currently visible
        if (chart != null && chart.activeSelf)
        {
            StartCoroutine(MoveChart(chart, startPosition.position, startPosition.rotation, false));
        }

        if (otherChart != null && otherChart.activeSelf)
        {
            StartCoroutine(MoveChart(otherChart, startPosition.position, startPosition.rotation, false));
        }

        // Show additional UI elements again
        ShowUIElements();

        // Enable player movement and camera look again
        EnablePlayerControl();

        isUIActive = false; // Mark UI as inactive
    }

    private IEnumerator MoveChart(GameObject chart, Vector3 targetPosition, Quaternion targetRotation, bool show)
    {
        isMoving = true;

        if (show)
        {
            chart.SetActive(true); // Activate before moving
        }

        float t = 0f;
        Vector3 initialPosition = chart.transform.position;
        Quaternion initialRotation = chart.transform.rotation;

        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            chart.transform.position = Vector3.Lerp(initialPosition, targetPosition, t);
            chart.transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, t);
            yield return null;
        }

        chart.transform.position = targetPosition;
        chart.transform.rotation = targetRotation;

        if (!show)
        {
            chart.SetActive(false); // Hide after movement finishes
        }

        isMoving = false;
    }

    private void HideUIElements()
    {
        if (uiToHide1 != null)
        {
            uiToHide1.SetActive(false);
        }

        if (uiToHide2 != null)
        {
            uiToHide2.SetActive(false);
        }
    }

    private void ShowUIElements()
    {
        if (uiToHide1 != null)
        {
            uiToHide1.SetActive(true);
        }

        if (uiToHide2 != null)
        {
            uiToHide2.SetActive(true);
        }
    }

    private void DisablePlayerControl()
    {
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = false;
        }

        if (playerLookScript != null)
        {
            playerLookScript.enabled = false;
        }
    }

    private void EnablePlayerControl()
    {
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = true;
        }

        if (playerLookScript != null)
        {
            playerLookScript.enabled = true;
        }
    }
}
