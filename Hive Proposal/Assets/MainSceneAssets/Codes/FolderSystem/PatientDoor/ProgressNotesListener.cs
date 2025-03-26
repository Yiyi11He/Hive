using UnityEngine;

public class ProgressNotesListener : MonoBehaviour
{
    public QuestGiver questGiver;
    public GameObject trackedObject; // The object to monitor

    private bool hasBeenActivated = false;

    private void Update()
    {
        if (trackedObject == null || questGiver == null) return;

        // Detect when the object is turned ON for the first time
        if (trackedObject.activeSelf && !hasBeenActivated)
        {
            hasBeenActivated = true; // Mark as activated
        }

        // Detect when the object is turned OFF after being activated
        if (!trackedObject.activeSelf && hasBeenActivated)
        {
            questGiver.UpdateQuestProgress(GoalType.ProgressFileInteract);
            enabled = false; // Disable the script after progress update
        }
    }
}
