using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Reflection;
using Yarn.Unity;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class QuestGiver : MonoBehaviour
{
    public List<Quest> quests;

    public PlayerMovement player;
    public Scores scoreManager;

    public GameObject questWindow;
    public TMP_Text TitleText;
    public TMP_Text DescriptionText;
    public TMP_Text ScoreText;

    public UnityEvent<int> OnQuestIndexChanged;

    private int currentQuestIndex = 0;



    //updating everyframe to check quest number
    //private void Update()
    //{
    //    MonitorQuestProgress();
    //}

    //private void MonitorQuestProgress()
    //{
    //    // Continuously log the current quest index
    //    Debug.Log($"Monitoring Current Quest Index: {currentQuestIndex}");

    //    // Check if the current quest is active and incomplete
    //    if (currentQuestIndex >= 0 && currentQuestIndex < quests.Count)
    //    {
    //        Quest currentQuest = quests[currentQuestIndex];
    //        if (currentQuest != null && currentQuest.isActive && currentQuest.AreGoalsComplete())
    //        {
    //            Debug.Log($"Quest {currentQuestIndex} is ready to be completed!");
    //            QuestComplete(); 
    //        }
    //    }
    //}

    private void Awake()
    {
        OnQuestIndexChanged = new UnityEvent<int>();
        OpenQuestByIndex(0);
    }

    public int GetCurrentQuestIndex()
    {
        return currentQuestIndex;
    }


    [YarnCommand("open_quest")]
    public void OpenQuestByIndex(int questIndex)
    {
        if (questIndex < 0 || questIndex >= quests.Count) return;

        currentQuestIndex = questIndex;
        Debug.Log($"Opening quest {questIndex}");
        OpenQuest(quests[questIndex]);

        //added code to get current quest index
        OnQuestIndexChanged.Invoke(currentQuestIndex);
    }


    public void OpenQuest(Quest quest)
    {
        if (quest == null) return;

        quest.isActive = true;
        questWindow.SetActive(true);
        TitleText.text = quest.title;
        DescriptionText.text = quest.description;
        ScoreText.text = quest.Score.ToString();
        player.quest = quest;
    }

    public void QuestComplete()
    {
        Debug.Log("QuestComplete method called");
        if (currentQuestIndex < 0 || currentQuestIndex >= quests.Count) return;

        Quest completedQuest = quests[currentQuestIndex];
        if (completedQuest != null && completedQuest.isActive)
        {
            Debug.Log($"Completing quest: {completedQuest.title}");

            if (scoreManager != null)
            {
                scoreManager.AddPlayerScore(completedQuest.Score);
            }
            completedQuest.isActive = false;

            questWindow.SetActive(false);

            if (currentQuestIndex + 1 < quests.Count)
            {
                currentQuestIndex++;
                OpenQuestByIndex(currentQuestIndex);

                OnQuestIndexChanged?.Invoke(currentQuestIndex);
            }
        }
    }


    [YarnCommand("complete_quest")]
    public void CompleteQuestByIndex(int questIndex)
    {
        if (questIndex < 0 || questIndex >= quests.Count) return;

        currentQuestIndex = questIndex;
        QuestComplete();
    }


    public void OnDialogueComplete()
    {
        QuestComplete();
        Debug.Log("Dialogue complete.");
    }

    public void UpdateQuestProgress(GoalType goalType)
    {
        Quest currentQuest = quests[currentQuestIndex];
        if (currentQuest == null || !currentQuest.isActive) return;

        foreach (var goal in currentQuest.goals)
        {
            if (goal.goalType == goalType)
            {
                goal.CurrentAction++; 
                Debug.Log($"Updated quest goal: {goalType}, Progress: {goal.CurrentAction}");

                // Check if all goals are now complete
                if (currentQuest.AreGoalsComplete())
                {
                    QuestComplete(); 
                }
                break;
            }
        }
    }

}