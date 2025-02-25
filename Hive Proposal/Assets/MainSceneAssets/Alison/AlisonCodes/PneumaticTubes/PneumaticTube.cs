using UnityEngine;
using System;

public class PneumaticTube : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject tubePrefab;
    [SerializeField] private PneumaticListener listener; // Reference to the listener

    private GameObject currentTube;
    public bool HasTube { get; private set; } = false;

    public event Action OnTubeGiven;
    public event Action OnTubeUsed;

    public void GiveTube(Transform playerCameraTransform)
    {
        if (currentTube)
            Destroy(currentTube);

        currentTube = Instantiate(tubePrefab, spawnPoint.position, spawnPoint.rotation, playerCameraTransform);
        currentTube.SetActive(true);
        HasTube = true;

        PneumaticTube newTubeScript = currentTube.GetComponent<PneumaticTube>();

        PneumaticListener listener = FindObjectOfType<PneumaticListener>(); // Find the listener in the scene
        if (listener != null)
        {
            listener.SetPneumaticTube(newTubeScript); // Dynamically update the reference
        }

        OnTubeGiven?.Invoke();
    }


    public void UseTube()
    {
        Destroy(currentTube);
        HasTube = false;

        OnTubeUsed?.Invoke();
    }
}
