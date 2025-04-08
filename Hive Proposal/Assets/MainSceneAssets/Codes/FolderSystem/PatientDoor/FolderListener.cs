using UnityEngine;

public class FolderListener : MonoBehaviour
{
    public QuestGiver questGiver;
    public GameObject trackedObject; // The object to monitor

    public InteractionType interactionType = InteractionType.File; // Inspector toggle

    private bool hasBeenActivated = false;

    public enum InteractionType
    {
        File,
        ProgressFile
    }

    private void Update()
    {
        if (trackedObject == null || questGiver == null) return;

        if (trackedObject.activeSelf && !hasBeenActivated)
        {
            hasBeenActivated = true;
        }

        if (!trackedObject.activeSelf && hasBeenActivated)
        {
            // Choose goal type based on enum selection
            GoalType goal = interactionType == InteractionType.File
                ? GoalType.FileInteract
                : GoalType.ProgressFileInteract;

            questGiver.UpdateQuestProgress(goal);
            enabled = false;
        }
    }
}
