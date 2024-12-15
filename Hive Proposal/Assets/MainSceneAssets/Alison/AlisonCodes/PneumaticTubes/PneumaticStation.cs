using System.Collections.Generic;
using UnityEngine;

public class PneumaticStation : MonoBehaviour
{
    [Header("References")]
    public PneumaticTube pneumaticTubeManager; 
    public QuestGiver questGiver;              

    [Header("Quest Requirements")]
    public List<int> requiredQuestIndices;

    public void OnInteraction()
    {
        int currentQuestIndex = questGiver.GetCurrentQuestIndex();

        // Check if the current quest index is in the required list and the player has the tube
        if (requiredQuestIndices.Contains(currentQuestIndex) && pneumaticTubeManager.HasTube)
        {
            CompleteQuest();
        }
        else
        {
            Debug.Log("Conditions not met: Either the quest index does not match or you don't have the pneumatic tube.");
        }
    }

    private void CompleteQuest()
    {
        Debug.Log("Pneumatic Tube detected. Completing the quest.");

        // Use the pneumatic tube
        pneumaticTubeManager.UseTube();

        // Complete the current quest
        questGiver.QuestComplete();
    }
}
