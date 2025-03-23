using UnityEngine;

public class PlayerInteractionTrigger : MonoBehaviour
{
    [SerializeField] private PlayerDespawn despawnHandler;

    public void TriggerInteraction()
    {
        if (despawnHandler != null)
        {
            despawnHandler.HandleInteraction();
        }
        else
        {
            Debug.LogWarning("[PlayerInteractionTrigger] Missing reference to PlayerDespawnHandler.");
        }
    }
}
