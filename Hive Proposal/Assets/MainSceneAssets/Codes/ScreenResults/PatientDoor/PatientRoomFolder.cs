using System.Collections;
using UnityEngine;

public class PatientRoomFolder : MonoBehaviour
{
    [Header("Chart to Control")]
    public GameObject chart;  // Assign GPReferral for GP Collider, ProgressNotes for Progress Collider
    public GameObject otherChart; // The other chart to be disabled

    [Header("Movement Settings")]
    public Transform startPosition; // Assign the hidden position
    public Transform endPosition;   // Assign the elevated visible position
    public float moveSpeed = 3f;    // Speed of movement

    private bool isMoving = false;

    private void Start()
    {
        if (chart != null)
        {
            chart.SetActive(false); // Ensure chart starts hidden
            chart.transform.position = startPosition.position; // Set to start position
            chart.transform.rotation = startPosition.rotation; // Set initial rotation
        }
    }

    public void OnHover()
    {
        if (chart != null && !isMoving)
        {
            Debug.Log($"Showing {chart.name} with movement");

            // Ensure the other chart is fully disabled before moving the active one
            if (otherChart != null && otherChart.activeSelf)
            {
                Debug.Log($"Hiding other chart first: {otherChart.name}");
                otherChart.SetActive(false);
            }

            // Start moving the selected chart
            StartCoroutine(MoveChart(chart, endPosition.position, endPosition.rotation, true));
        }
    }

    public void OnLookAway()
    {
        if (chart != null && !isMoving)
        {
            Debug.Log($"Hiding {chart.name} with movement");
            StartCoroutine(MoveChart(chart, startPosition.position, startPosition.rotation, false));
        }
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
            Debug.Log($"Hiding {chart.name} after movement complete.");
            chart.SetActive(false); // Hide after movement finishes
        }

        isMoving = false;
    }
}
