using System.Collections;
using UnityEngine;

public class Chair : MonoBehaviour
{
    [Header("Player Objects")]
    public GameObject playerStanding;
    public GameObject playerSitting;
    public GameObject intText;
    public GameObject standText;
    public GameObject switchView;

    [Header("Camera Settings")]
    public Camera targetCamera; // The camera to activate when sitting down
    public float targetFOV = 40f; // Desired field of view when sitting
    public float zoomSpeed = 1f;  // Speed of the zoom transition

    [Header("Quest System")]
    public QuestGiver questGiver;

    public bool sitting;
    private bool interactable = false;
    private float defaultFOV;
    private Coroutine zoomCoroutine;

    void Start()
    {
        if (targetCamera != null)
        {
            defaultFOV = targetCamera.fieldOfView; // Store the default FOV
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            intText.SetActive(true);
            interactable = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            intText.SetActive(false);
            interactable = false;
        }
    }

    void Update()
    {
        if (interactable && Input.GetKeyDown(KeyCode.E))
        {
            SitDown();
        }

        if (sitting && Input.GetKeyDown(KeyCode.Q))
        {
            StandUp();
        }
    }

    private void SitDown()
    {
        intText.SetActive(false);
        standText.SetActive(true);
        playerSitting.SetActive(true);
        playerStanding.SetActive(false);
        switchView.SetActive(true);
        sitting = true;
        interactable = false;

        if (targetCamera != null)
        {
            targetCamera.gameObject.SetActive(true);

            // Start zoom effect
            if (zoomCoroutine != null) StopCoroutine(zoomCoroutine);
            zoomCoroutine = StartCoroutine(SmoothZoom(targetFOV));
        }

        if (questGiver != null)
        {
            Debug.Log($"Current Quest Index: {questGiver.GetCurrentQuestIndex()}");
            questGiver.UpdateQuestProgress(GoalType.ChairInteract);
        }
    }

    private void StandUp()
    {
        playerSitting.SetActive(false);
        standText.SetActive(false);
        playerStanding.SetActive(true);
        switchView.SetActive(false);
        sitting = false;

        if (targetCamera != null)
        {
            // Start reverse zoom effect
            if (zoomCoroutine != null) StopCoroutine(zoomCoroutine);
            zoomCoroutine = StartCoroutine(SmoothZoom(defaultFOV));
        }
    }

    private IEnumerator SmoothZoom(float targetFOV)
    {
        float startFOV = targetCamera.fieldOfView;
        float elapsedTime = 0f;

        while (elapsedTime < zoomSpeed)
        {
            targetCamera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, elapsedTime / zoomSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        targetCamera.fieldOfView = targetFOV;
    }
}
