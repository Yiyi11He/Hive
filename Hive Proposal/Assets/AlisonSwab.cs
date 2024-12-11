using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AlisonSwab : MonoBehaviour
{
    public GameObject playerMainCamera;
    public QuestGiver questGiver;
    public Scores scoreManager;

    [SerializeField] private LayerMask swabLayerMask;
    [Header("Swab Cameras and Targets")]
    [SerializeField] private List<SwabAction> swabActions;

    private bool interacting = false;
    private int currentSwabIndex = 0;
    [SerializeField] private int penaltyScore = -5;

    [System.Serializable]
    public class SwabAction
    {
        public string swabDescription;
        public GameObject targetArea;        // The specific area on Alison to swab
        public GameObject swabCamera;        // The camera to switch to during swabbing
        public RectTransform incorrectSwabIndicator; // Incorrect swab indicator for this swab camera
        public int requiredQuestIndex;       // The quest index required to perform this swab
    }

    private void Update()
    {
        if (interacting && Mouse.current.leftButton.wasPressedThisFrame)
        {
            PerformSwab();
        }
    }

    public void StartSwabInteraction()
    {
        int currentQuestIndex = questGiver.GetCurrentQuestIndex();

        if (currentSwabIndex < swabActions.Count && currentQuestIndex == swabActions[currentSwabIndex].requiredQuestIndex)
        {
            swabActions[currentSwabIndex].targetArea.SetActive(true);
            SwitchToSwabCamera();
            interacting = true;
        }
        else
        {
            Debug.Log("Swab interaction unavailable. Complete the required quest first.");
        }
    }

    private void PerformSwab()
    {
        if (currentSwabIndex >= swabActions.Count) return;

        Camera currentSwabCamera = swabActions[currentSwabIndex].swabCamera.GetComponent<Camera>();

        Ray ray = currentSwabCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, swabLayerMask))
        {
            if (hit.collider.gameObject == swabActions[currentSwabIndex].targetArea)
            {
                Debug.Log($"Swab successful: {swabActions[currentSwabIndex].swabDescription}");

                CompleteQuest(); // Complete the current quest after a successful swab
                EndInteraction(); // Return to the main camera
            }
            else
            {
                Debug.Log("Incorrect swab target. Try again.");
                ApplyPenalty();
                StartCoroutine(ShowIncorrectSwabIndicator(Mouse.current.position.ReadValue()));
            }
        }
        else
        {
            Debug.Log("No hit detected. Try again.");
            ApplyPenalty();
            StartCoroutine(ShowIncorrectSwabIndicator(Mouse.current.position.ReadValue()));
        }
    }

    private void ApplyPenalty()
    {
        if (scoreManager != null)
        {
            scoreManager.AddPlayerScore(penaltyScore);
        }
    }

    private void SwitchToSwabCamera()
    {
        playerMainCamera.SetActive(false);

        foreach (var swabAction in swabActions)
        {
            swabAction.swabCamera.SetActive(false);
        }

        swabActions[currentSwabIndex].swabCamera.SetActive(true);
    }

    private IEnumerator ShowIncorrectSwabIndicator(Vector2 screenPosition)
    {
        RectTransform currentIndicator = swabActions[currentSwabIndex].incorrectSwabIndicator;

        if (currentIndicator != null)
        {
            currentIndicator.gameObject.SetActive(true);
            currentIndicator.position = screenPosition;

            yield return new WaitForSeconds(0.7f);

            currentIndicator.gameObject.SetActive(false);
        }
    }

    private void CompleteQuest()
    {
        questGiver.QuestComplete();
        currentSwabIndex++; // Move to the next swab action

        // Check if there are more swabs or reset for next interaction
        if (currentSwabIndex >= swabActions.Count || swabActions[currentSwabIndex].requiredQuestIndex != questGiver.GetCurrentQuestIndex())
        {
            currentSwabIndex = 0; // Reset for the next interaction
        }
    }

    public void EndInteraction()
    {
        interacting = false;
        playerMainCamera.SetActive(true);

        foreach (var swabAction in swabActions)
        {
            swabAction.swabCamera.SetActive(false);
        }
    }
}
