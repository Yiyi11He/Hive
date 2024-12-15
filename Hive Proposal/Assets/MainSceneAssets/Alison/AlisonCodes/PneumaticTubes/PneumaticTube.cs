using UnityEngine;

public class PneumaticTube : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private Transform spawnPoint;    // The spawn point relative to the player's camera
    [SerializeField] private GameObject tubePrefab;   // The Pneumatic Tube prefab to instantiate

    private GameObject currentTube;
    public bool HasTube { get; private set; } = false;  // Tracks if the player has the tube

    // Call this method to give the player the Pneumatic Tube
    public void GiveTube(Transform playerCameraTransform)
    {
        if (tubePrefab != null && spawnPoint != null)
        {
            if (currentTube != null)
            {
                Destroy(currentTube);
            }

            // Instantiate the Pneumatic Tube at the specified spawn point relative to the player's camera
            currentTube = Instantiate(tubePrefab, spawnPoint.position, spawnPoint.rotation, playerCameraTransform);
            currentTube.SetActive(true);

            HasTube = true;
            Debug.Log("Pneumatic Tube given to the player.");
        }
    }

    // Call this method to consume the tube (e.g., when the player uses it at the station)
    public void UseTube()
    {
        if (currentTube != null)
        {
            Destroy(currentTube);
            currentTube = null;
        }

        HasTube = false;
        Debug.Log("Pneumatic Tube used.");
    }
}
