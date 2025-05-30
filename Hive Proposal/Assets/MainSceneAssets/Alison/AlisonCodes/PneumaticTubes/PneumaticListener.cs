using UnityEngine;
using System.Collections.Generic;

public class PneumaticListener : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PneumaticTube pneumaticTube;

    [Header("Activation List")]
    [SerializeField] private List<GameObject> activationObjects = new List<GameObject>();
    private int activationCount = 0;
    private GameObject lastActivatedObject;

    private void Start()
    {
        FindActivePneumaticTube();
    }

    private void OnEnable()
    {
        FindActivePneumaticTube();
    }

    private void FindActivePneumaticTube()
    {
        pneumaticTube = FindObjectOfType<PneumaticTube>();
        SubscribeToPneumaticTube();
    }

    public void SetPneumaticTube(PneumaticTube newTube)
    {
        if (newTube == null)
        {
            Debug.LogWarning("SetPneumaticTube was passed a null reference.");
            return;
        }

        UnsubscribeFromPneumaticTube();
        pneumaticTube = newTube;
        SubscribeToPneumaticTube();
    }


    private void SubscribeToPneumaticTube()
    {
        UnsubscribeFromPneumaticTube(); 
        pneumaticTube.OnTubeGiven += HandleTubeGiven;
        pneumaticTube.OnTubeUsed += HandleTubeUsed;
    }

    private void UnsubscribeFromPneumaticTube()
    {
        pneumaticTube.OnTubeGiven -= HandleTubeGiven;
        pneumaticTube.OnTubeUsed -= HandleTubeUsed;
    }

    private void HandleTubeGiven()
    {
        if (activationCount >= activationObjects.Count)
        {
            Debug.LogWarning($"PneumaticListener: No more activation objects to assign (index {activationCount}).");
            return;
        }

        if (lastActivatedObject != null)
        {
            lastActivatedObject.SetActive(false);
        }

        lastActivatedObject = activationObjects[activationCount];
        lastActivatedObject.SetActive(true);

        activationCount++;
    }


    private void HandleTubeUsed()
    {
        Destroy(lastActivatedObject);
        lastActivatedObject = null;
    }
}
