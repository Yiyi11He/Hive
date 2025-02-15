using System.Collections;
using UnityEngine;

public class AdminFolder : MonoBehaviour
{
    [Header("Folders to Control")]
    public GameObject folder1;
    public GameObject folder2;
    public GameObject folder3;
    public GameObject folder4;

    [Header("UI Panels")]
    public GameObject uiMainFolder;
    public GameObject uiSubFolder;

    [Header("Additional UI Elements to Hide")]
    public GameObject uiToHide1;
    public GameObject uiToHide2;

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

    private static AdminFolder activeFolder = null; // Change to AdminFolder
    private GameObject currentFolder; // Tracks which folder is active

    private void Start()
    {
        // Hide all folders initially
        HideAllFolders();

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
        // Listen for "E" key press to activate UI mode
        if (isHovered && Input.GetKeyDown(KeyCode.E))
        {
            ActivateUI();
        }

        // Listen for "Q" key press to exit UI mode
        if (isUIActive && Input.GetKeyDown(KeyCode.Q))
        {
            ExitUIMode();
        }
    }

    public void OnHover(int folderIndex)
    {
        if (!isMoving)
        {
            // Ensure previous active folder is hidden before switching
            if (activeFolder != null && activeFolder != this)
            {
                activeFolder.HideAllFolders();
            }

            // Set this as the active folder
            activeFolder = this;

            // Determine which folder to show
            switch (folderIndex)
            {
                case 1: currentFolder = folder1; break;
                case 2: currentFolder = folder2; break;
                case 3: currentFolder = folder3; break;
                case 4: currentFolder = folder4; break;
            }

            if (currentFolder != null)
            {
                StartCoroutine(MoveFolder(currentFolder, endPosition.position, endPosition.rotation, true));
            }
        }

        isHovered = true;
    }

    public void OnLookAway()
    {
        isHovered = false;

        if (!isMoving && currentFolder != null)
        {
            StartCoroutine(MoveFolder(currentFolder, startPosition.position, startPosition.rotation, false));
        }

        if (isUIActive)
        {
            ExitUIMode();
        }
    }

    private void HideAllFolders()
    {
        if (folder1 != null) folder1.SetActive(false);
        if (folder2 != null) folder2.SetActive(false);
        if (folder3 != null) folder3.SetActive(false);
        if (folder4 != null) folder4.SetActive(false);
    }

    private void ActivateUI()
    {
        if (!isUIActive)
        {
            if (uiMainFolder != null) uiMainFolder.SetActive(true);
            if (uiSubFolder != null) uiSubFolder.SetActive(true);

            HideUIElements();
            DisablePlayerControl();

            isUIActive = true;
        }
    }

    public void ExitUIMode()
    {
        if (uiSubFolder != null) uiSubFolder.SetActive(false);
        if (uiMainFolder != null) uiMainFolder.SetActive(false);

        if (currentFolder != null)
        {
            StartCoroutine(MoveFolder(currentFolder, startPosition.position, startPosition.rotation, false));
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
            folder.SetActive(false);
        }

        isMoving = false;
    }

    private void HideUIElements()
    {
        if (uiToHide1 != null) uiToHide1.SetActive(false);
        if (uiToHide2 != null) uiToHide2.SetActive(false);
    }

    private void ShowUIElements()
    {
        if (uiToHide1 != null) uiToHide1.SetActive(true);
        if (uiToHide2 != null) uiToHide2.SetActive(true);
    }

    private void DisablePlayerControl()
    {
        if (playerMovementScript != null) playerMovementScript.enabled = false;
        if (playerLookScript != null) playerLookScript.enabled = false;
    }

    private void EnablePlayerControl()
    {
        if (playerMovementScript != null) playerMovementScript.enabled = true;
        if (playerLookScript != null) playerLookScript.enabled = true;
    }
}
