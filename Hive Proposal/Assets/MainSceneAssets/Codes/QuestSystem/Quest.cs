using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public bool isActive;

    public string title;
    public string description;
    public int Score;

    public List<QuestGoal> goals;

    public bool AreGoalsComplete()
    {
        foreach (var goal in goals)
        {
            if (goal.CurrentAction == 0) 
                return false;
        }
        return true;
    }

}
