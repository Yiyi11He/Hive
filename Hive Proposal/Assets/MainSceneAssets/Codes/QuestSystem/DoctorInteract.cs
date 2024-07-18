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
        if (interactable && !interacting)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartInteraction();
            }
        }
    }

    private void StartInteraction()
    {
        intText.SetActive(false);
        playerMainCamera.SetActive(false);
        doctorCamera.SetActive(true);
        dialogueUI.SetActive(true);
        interacting = true;

        dialogueRunner.StartDialogue("initialInteration");
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