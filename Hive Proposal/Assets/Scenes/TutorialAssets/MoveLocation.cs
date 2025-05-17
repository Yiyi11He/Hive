using UnityEngine;

public class MoveLocation : MonoBehaviour
{
    public QuestGiver questGiver;
    public GameObject MoveArea;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && MoveArea != null)
        {
            MoveArea.SetActive(false);
            questGiver.UpdateQuestProgress(GoalType.MoveLocation);
        }
    }
}
