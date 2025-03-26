using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestGoal
{
    public GoalType goalType;
    public GameObject RequiredAction;
    public int CurrentAction;
}

public enum GoalType
{
    ChairInteract,
    MonitorInteract,
    IconClick,
    FileInteract,
    ProgressFileInteract,
    DoctorInteract,
    SwabInteract,
    SwabSubmit,
    DoorInteract,
    Leave
}