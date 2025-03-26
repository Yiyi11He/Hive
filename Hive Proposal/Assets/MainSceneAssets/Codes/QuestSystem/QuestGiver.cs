using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Yarn.Unity;
using UnityEngine.Events;

public class QuestGiver : MonoBehaviour
{
    [Header("Debugging Tools")]
    [SerializeField] private bool debugMode = false;  // Toggle debug mode on/off
    [SerializeField] private int debugQuestIndex = 0; // Index to set as zero for debugging

    public List<Quest> quests;

    public PlayerMovement player;
    public Scores scoreManager;

    public GameObject questWindow;
    public TMP_Text TitleText;
    public TMP_Text DescriptionText;
    public TMP_Text ScoreText;

    public UnityEvent<int> OnQuestIndexChanged;

    private int currentQuestIndex = 0;


    private void Awake()
{
    OnQuestIndexChanged = new UnityEvent<int>();

    // Set the initial quest index only ONCE
    currentQuestIndex = debugMode ? debugQuestIndex : 0;
    OpenQuestByIndex(currentQuestIndex);
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

                if (currentQuest.AreGoalsComplete())
                {
                    QuestComplete();
                }
                break;
            }
        }
    }

    // Debugging Functionality
    public void SetQuestToIndexZero(int questIndex)
    {
        if (questIndex < 0 || questIndex >= quests.Count)
        {
            Debug.LogError($"Invalid quest index: {questIndex}");
            return;
        }

        // Move the selected quest to index 0
        Quest selectedQuest = quests[questIndex];
        quests.RemoveAt(questIndex);
        quests.Insert(0, selectedQuest);

        Debug.Log($"Quest \"{selectedQuest.title}\" has been moved to index 0.");
        OpenQuestByIndex(0); // Update the quest to reflect the change
    }
    public void HideQuestUI()
    {
        if (questWindow != null)
            questWindow.SetActive(false);
    }
}
