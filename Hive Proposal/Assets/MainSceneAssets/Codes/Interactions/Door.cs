using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Door : MonoBehaviour
{
    public bool IsOpen = false;

    [SerializeField] private bool IsRotatingDoor = true;
    [SerializeField] private float speed = 1.0f;
    [Header("Rotation Configs")]
    [SerializeField] private float RotationAmount = 90f;
    [SerializeField] private float ForwardDirection = 0;

    [SerializeField] private TextMeshPro FrontText;
    [SerializeField] private TextMeshPro BackText;
    [SerializeField] private Transform Camera;
    [SerializeField] private float MaxUseDistance = 5f;
    [SerializeField] private LayerMask UseLayers;

    [SerializeField] private InputAction useAction;

    private Vector3 StartRotation;
    private Vector3 Forward;
    private Coroutine AnimationCoroutine;

    private void Awake()
    {
        StartRotation = transform.rotation.eulerAngles;
        Forward = transform.forward;

        useAction.performed += OnUseAction;
        useAction.Enable();
    }

    private void OnUseAction(InputAction.CallbackContext context)
    {
        CheckForInteraction();
    }

    private void CheckForInteraction()
    {
        if (Physics.Raycast(Camera.position, Camera.forward, out RaycastHit hit, MaxUseDistance, UseLayers))
        {
            if (hit.collider == this.GetComponent<Collider>())
            {
                Interact();
            }
        }
    }

    public void Interact()
    {
        if (IsOpen)
        {
            Close();
        }
        else
        {
            Open(Camera.position);
        }
    }

    public void ShowText(bool show)
    {
        string text = IsOpen ? "Close \"E\"" : "Open \"E\"";
        if (FrontText != null) FrontText.text = text;
        if (BackText != null) BackText.text = text;

        if (FrontText != null) FrontText.gameObject.SetActive(show);
        if (BackText != null) BackText.gameObject.SetActive(show);
    }

    public void Open(Vector3 userPosition)
    {
        if (!IsOpen)
        {
            if (AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);
            }

            if (IsRotatingDoor)
            {
                float dot = Vector3.Dot(Forward, (userPosition - transform.position).normalized);
                Debug.Log($"Dot: {dot.ToString("N3")}");
                AnimationCoroutine = StartCoroutine(DoRotationOpen(dot));
            }
        }
    }

    private IEnumerator DoRotationOpen(float forwardAmount)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation;

        if (forwardAmount >= ForwardDirection)
        {
            endRotation = Quaternion.Euler(new Vector3(0, StartRotation.y + RotationAmount, 0));
        }
        else
        {
            endRotation = Quaternion.Euler(new Vector3(0, StartRotation.y - RotationAmount, 0));
        }

        IsOpen = true;

        float time = 0;
        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * speed;
        }
    }

    public void Close()
    {
        if (IsOpen)
        {
            if (AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);
            }

            if (IsRotatingDoor)
            {
                AnimationCoroutine = StartCoroutine(DoRotationClose());
            }
        }
    }

    private IEnumerator DoRotationClose()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(StartRotation);
        IsOpen = false;

        float time = 0;
        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * speed;
        }

        transform.rotation = endRotation; // Ensure precise closing
    }
}
