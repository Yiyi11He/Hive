using System.Collections;
using UnityEngine;

public class AdminFolder : MonoBehaviour
{
    [Header("Folder to Control")]
    public GameObject folder;

    [Header("UI Elements")]
    public GameObject uiMainFolder;
    public GameObject uiSubFolder;

    [Header("Additional UI Elements to Hide")]
    public GameObject TimeScores;
    public GameObject QuestWindow;

    [Header("Player Components")]
    public MonoBehaviour playerMovementScript;
    public MonoBehaviour playerLookScript;

    [Header("Movement Settings")]
    public Transform startPosition;
    public Transform endPosition;
    public float moveSpeed = 3f;

    private bool isMoving = false;
    private bool isHovered = false;
    private bool isUIActive = false;

    private static AdminFolder activeFolder = null;

    private void Start()
    {
        if (folder != null)
        {
            folder.SetActive(true); // Ensure it's always active
            folder.transform.position = startPosition.position;
            folder.transform.rotation = startPosition.rotation;
        }

        if (uiMainFolder != null) uiMainFolder.SetActive(false);
        if (uiSubFolder != null) uiSubFolder.SetActive(false);

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
        if (isMoving) return; // Prevent multiple animations from starting

        Debug.Log($"Hovering: Moving {folder.name}");

        // Ensure the previous active folder hides before activating a new one
        if (activeFolder != null && activeFolder != this)
        {
            Debug.Log($"Hiding previous folder: {activeFolder.folder.name}");
            activeFolder.HideFolder();
        }

        activeFolder = this; // Set this as the new active folder

        StartCoroutine(MoveFolder(folder, endPosition.position, endPosition.rotation, true));

        isHovered = true;
    }

    public void OnLookAway()
    {
        if (!isHovered) return; // Prevent redundant calls

        isHovered = false;

        if (!isMoving)
        {
            Debug.Log($"Hiding {folder.name} with movement");
            StartCoroutine(MoveFolder(folder, startPosition.position, startPosition.rotation, false));
        }

        if (isUIActive)
        {
            ExitUIMode();
        }
    }

    private void HideFolder()
    {
        if (folder != null && folder.activeSelf)
        {
            StartCoroutine(MoveFolder(folder, startPosition.position, startPosition.rotation, false));
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
            Debug.Log($"Activating UI for {folder.name}");

            if (uiMainFolder != null) uiMainFolder.SetActive(true);
            if (uiSubFolder != null) uiSubFolder.SetActive(true);

            HideUIElements();
            DisablePlayerControl();
            isUIActive = true;
        }
    }

    public void ExitUIMode()
    {
        Debug.Log("Exiting UI Mode - Hiding all UI panels");

        if (uiSubFolder != null) uiSubFolder.SetActive(false);
        if (uiMainFolder != null) uiMainFolder.SetActive(false);

        if (folder != null && folder.activeSelf)
        {
            StartCoroutine(MoveFolder(folder, startPosition.position, startPosition.rotation, false));
        }

        ShowUIElements();
        EnablePlayerControl();

        isUIActive = false;
    }

    private IEnumerator MoveFolder(GameObject folder, Vector3 targetPosition, Quaternion targetRotation, bool show)
    {
        isMoving = true;

        if (show)
        {
            folder.SetActive(true);
        }

        float t = 0f;
        Vector3 initialPosition = folder.transform.position;
        Quaternion initialRotation = folder.transform.rotation;

        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            folder.transform.position = Vector3.Lerp(initialPosition, targetPosition, t);
            folder.transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, t);
            yield return null;
        }

        folder.transform.position = targetPosition;
        folder.transform.rotation = targetRotation;

        if (!show)
        {
            Debug.Log($"Hiding {folder.name} after movement complete.");
        }

        isMoving = false;
    }

    private void HideUIElements()
    {
        if (TimeScores != null)
        {
            CanvasGroup cg = TimeScores.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                Debug.Log("Hiding TimeScores via CanvasGroup");
                cg.alpha = 0f;
                cg.interactable = false;
                cg.blocksRaycasts = false;
            }
            else
            {
                Debug.LogWarning("CanvasGroup missing on TimeScores.");
            }
        }


        if (QuestWindow != null)
        {
            CanvasGroup cg = QuestWindow.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                Debug.Log("Hiding QuestWindow via CanvasGroup");
                cg.alpha = 0f;
                cg.interactable = false;
                cg.blocksRaycasts = false;
            }
            else
            {
                Debug.LogWarning("CanvasGroup missing on QuestWindow.");
            }
        }
    }



    private void ShowUIElements()
    {
        if (TimeScores != null)
        {
            CanvasGroup cg = TimeScores.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                Debug.Log("Showing TimeScores via CanvasGroup");
                cg.alpha = 1f;
                cg.interactable = true;
                cg.blocksRaycasts = true;
            }
        }

        if (QuestWindow != null)
        {
            CanvasGroup cg = QuestWindow.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                Debug.Log("Showing QuestWindow via CanvasGroup");
                cg.alpha = 1f;
                cg.interactable = true;
                cg.blocksRaycasts = true;
            }
        }
    }




    private void DisablePlayerControl()
    {
        if (playerMovementScript != null) playerMovementScript.enabled = false;
        if (playerLookScript != null) playerLookScript.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void EnablePlayerControl()
    {
        if (playerMovementScript != null) playerMovementScript.enabled = true;
        if (playerLookScript != null) playerLookScript.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
