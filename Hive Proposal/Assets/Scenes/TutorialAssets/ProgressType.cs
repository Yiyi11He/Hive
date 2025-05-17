using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressType : MonoBehaviour
{
    public QuestGiver questGiver;
    public GameObject TrackedObject;

    public InteractionType interactionType = InteractionType.Pickup;

    private bool previousActiveState = false;
    public enum InteractionType
    {   
        MoveDetect,
        Pickup,
        DoorDetect,
        PenguinDetect
    }

    private void Update()
    {
        if (TrackedObject == null || questGiver == null) return;

        bool currentState = TrackedObject.activeSelf;

        if (previousActiveState && !currentState)
        {
            GoalType goal;

            switch (interactionType)
            {

                case InteractionType.MoveDetect:
                    goal = GoalType.MoveLocation;
                    break;
                case InteractionType.Pickup:
                    goal = GoalType.CubeInteract;
                    break;
                case InteractionType.PenguinDetect:
                    goal = GoalType.PenguinInteract;
                    break;
                case InteractionType.DoorDetect:
                    goal = GoalType.DoorProgress;
                    break;
                default:
                    Debug.LogWarning("Unknown InteractionType, defaulting to MoveLocation");
                    goal = GoalType.MoveLocation;
                    break;
            }
            questGiver.UpdateQuestProgress(goal);
        }

            previousActiveState = currentState;
    }
}
