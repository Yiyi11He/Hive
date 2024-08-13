using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;

public class DoctorInteract : MonoBehaviour
{
    public GameObject playerMainCamera;
    public GameObject doctorCamera;
    [SerializeField]
    private float maxUseDistance = 5f;
    [SerializeField]
    private LayerMask useLayers;
    [SerializeField]
    public GameObject dialogueUI;
    public DialogueRunner dialogueRunner;
    public GameObject intText;
    public GameObject doctorObject;
    private bool interactable = false;
    private bool interacting = false;

    private void Awake()
    {
        if (dialogueRunner == null)
        {
            Debug.LogError("DialogueRunner is not assigned.");
        }

        dialogueRunner.onDialogueComplete.AddListener(OnDialogueComplete);
    }

    private void OnDestroy()
    {
        dialogueRunner.onDialogueComplete.RemoveListener(OnDialogueComplete);
    }

    void Update()
    {
        if (!interacting)
        {
            CheckForInteraction();
        }

        if (interactable && !interacting && Input.GetKeyDown(KeyCode.E))
        {
            StartInteraction();
        }
    }
    private void CheckForInteraction()
    {
        // Cast a ray from the camera's position forward
        Ray ray = new Ray(playerMainCamera.transform.position, playerMainCamera.transform.forward);
        RaycastHit hit;

        // Check if the ray hits an object within the maxUseDistance and if that object is the doctor
        if (Physics.Raycast(ray, out hit, maxUseDistance, useLayers))
        {
            if (hit.collider.gameObject == doctorObject)
            {
                intText.SetActive(true);
                interactable = true;
            }
            else
            {
                intText.SetActive(false);
                interactable = false;
            }
        }
        else
        {
            intText.SetActive(false);
            interactable = false;
        }
    }

    private void StartInteraction()
    {
        intText.SetActive(false);
        playerMainCamera.SetActive(false);
        doctorCamera.SetActive(true);
        dialogueUI.SetActive(true);
        interacting = true;

        dialogueRunner.StartDialogue("initialInteraction");
    }

    private void OnDialogueComplete()
    {
        EndInteraction();
    }

    public void EndInteraction()
    {
        playerMainCamera.SetActive(true);
        doctorCamera.SetActive(false);
        dialogueUI.SetActive(false);
        interacting = false;
    }
}