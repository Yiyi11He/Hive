using UnityEngine;

public class FolderListener : MonoBehaviour
{
    public QuestGiver questGiver;
    public GameObject trackedObject;

    public InteractionType interactionType = InteractionType.File;

    private bool previousActiveState = false;

    public enum InteractionType
    {
        File,
        ProgressFile
    }

    private void Update()
    {
        if (trackedObject == null || questGiver == null) return;

        bool currentState = trackedObject.activeSelf;

        if (previousActiveState && !currentState)
        {
            GoalType goal = interactionType == InteractionType.File
                ? GoalType.FileInteract
                : GoalType.ProgressFileInteract;

            questGiver.UpdateQuestProgress(goal);
        }

        previousActiveState = currentState;
    }
}
