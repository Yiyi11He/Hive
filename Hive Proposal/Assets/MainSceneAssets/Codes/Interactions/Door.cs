using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool IsOpen = false;

    [Header("Door Type & Speed")]
    [SerializeField] private bool IsRotatingDoor = true;
    [SerializeField] private float speed = 1.0f;

    [Header("Rotation Configs")]
    [SerializeField] private float RotationAmount = 90f;
    [SerializeField] private float ForwardDirection = 0;

    [Header("Auto-Close Settings")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float autoCloseRange = 2f;
    [SerializeField] private float autoCloseDelay = 0.5f;

    private Vector3 startRotation;
    private Vector3 forward;
    private Coroutine animationCoroutine;
    private Coroutine autoCloseCoroutine; // Track the auto-close so don't stack multiple coroutines

    private void Awake()
    {
        startRotation = transform.rotation.eulerAngles;
        forward = transform.forward;
    }

    private void Update()
    {
        // If the door is open and we haven't started any auto-close routine yet,
        // check whether the player is out of range
        if (IsOpen && autoCloseCoroutine == null && playerTransform != null)
        {
            float distance = Vector3.Distance(transform.position, playerTransform.position);
            if (distance > autoCloseRange)
            {
                // Begin auto-close countdown
                autoCloseCoroutine = StartCoroutine(AutoCloseAfterDelay(autoCloseDelay));
            }
        }
    }

    public void Interact(Transform playerTransform)
    {
        if (IsOpen)
        {
            Close();
        }
        else
        {
            Open(playerTransform.position);
        }
    }

    public void Open(Vector3 userPosition)
    {
        if (!IsOpen)
        {
            // Stop any close-in-progress
            if (animationCoroutine != null)
            {
                StopCoroutine(animationCoroutine);
            }

            if (IsRotatingDoor)
            {
                float dot = Vector3.Dot(forward, (userPosition - transform.position).normalized);
                Debug.Log($"Dot: {dot.ToString("N3")}");
                animationCoroutine = StartCoroutine(DoRotationOpen(dot));
            }
        }
    }

    private IEnumerator DoRotationOpen(float forwardAmount)
    {
        Quaternion startRot = transform.rotation;
        Quaternion endRot;

        if (forwardAmount >= ForwardDirection)
        {
            endRot = Quaternion.Euler(new Vector3(0, startRotation.y + RotationAmount, 0));
        }
        else
        {
            endRot = Quaternion.Euler(new Vector3(0, startRotation.y - RotationAmount, 0));
        }

        IsOpen = true;

        float time = 0;
        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(startRot, endRot, time);
            yield return null;
            time += Time.deltaTime * speed;
        }

        transform.rotation = endRot; // Ensure precise rotation
    }

    public void Close()
    {
        if (IsOpen)
        {
            // Stop any open-in-progress
            if (animationCoroutine != null)
            {
                StopCoroutine(animationCoroutine);
            }

            // Stop any pending auto-close
            if (autoCloseCoroutine != null)
            {
                StopCoroutine(autoCloseCoroutine);
                autoCloseCoroutine = null;
            }

            if (IsRotatingDoor)
            {
                animationCoroutine = StartCoroutine(DoRotationClose());
            }
        }
    }

    private IEnumerator DoRotationClose()
    {
        Quaternion startRot = transform.rotation;
        Quaternion endRot = Quaternion.Euler(startRotation);
        IsOpen = false;

        float time = 0;
        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(startRot, endRot, time);
            yield return null;
            time += Time.deltaTime * speed;
        }

        transform.rotation = endRot;
    }

    private IEnumerator AutoCloseAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (IsOpen && playerTransform != null)
        {
            float distance = Vector3.Distance(transform.position, playerTransform.position);
            if (distance > autoCloseRange)
            {
                Close();
            }
        }

        autoCloseCoroutine = null;
    }
}
