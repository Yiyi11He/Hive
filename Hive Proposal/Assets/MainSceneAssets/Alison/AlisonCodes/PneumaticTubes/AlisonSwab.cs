using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AlisonSwab : MonoBehaviour
{
    public GameObject playerMainCamera;
    public QuestGiver questGiver;
    public Scores scoreManager;
    public PneumaticTube pneumaticTubeManager;  // Reference to PneumaticTube

    [SerializeField] private LayerMask swabLayerMask;
    [Header("Swab Cameras and Targets")]
    [SerializeField] private List<SwabAction> swabActions;

    [Header("Disable Objects on Camera Swap")]
    [SerializeField] private GameObject disableOnCameraSwap1; // First object to disable
    [SerializeField] private GameObject disableOnCameraSwap2; // Second object to disable

    private bool interacting = false;
    private int currentSwabIndex = 0;
    [SerializeField] private int penaltyScore = -5;

    [System.Serializable]
    public class SwabAction
    {
        public string swabDescription;
        public GameObject targetArea;
        public GameObject swabCamera;
        public RectTransform incorrectSwabIndicator;
        public int requiredQuestIndex;
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

                // Give the tube to the player upon a successful swab
                pneumaticTubeManager.GiveTube(playerMainCamera.transform);

                CompleteQuest();
                EndInteraction();
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

        // Disable both objects when switching to swab camera

        disableOnCameraSwap1.SetActive(false);


        disableOnCameraSwap2.SetActive(false);


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
        currentSwabIndex++;
    }

    public void EndInteraction()
    {
        interacting = false;
        playerMainCamera.SetActive(true);


        disableOnCameraSwap1.SetActive(true);
        disableOnCameraSwap2.SetActive(true);

        foreach (var swabAction in swabActions)
        {
            swabAction.swabCamera.SetActive(false);
        }
    }
}
