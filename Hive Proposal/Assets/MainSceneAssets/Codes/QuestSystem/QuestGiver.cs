using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Reflection;
using Yarn.Unity;
using UnityEngine.SceneManagement;

public class QuestGiver : MonoBehaviour
{
    public List<Quest> quests;

    public PlayerMovement player;
    public Scores scoreManager;

    public GameObject questWindow;
    public TMP_Text TitleText;
    public TMP_Text DescriptionText;
    public TMP_Text ScoreText;

    private int currentQuestIndex = 0;

    private void Awake()
    {
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
        if (currentQuestIndex < 0 || currentQuestIndex >= quests.Count) return;

        Quest completedQuest = quests[currentQuestIndex];
        if (completedQuest != null && completedQuest.isActive)
        {
            Debug.Log($"Completing quest: {completedQuest.title}");

            // Add the quest's score to the player using the Scores system
            if (scoreManager != null)
            {
                scoreManager.AddPlayerScore(completedQuest.Score);
            }
            completedQuest.isActive = false;

            questWindow.SetActive(false);
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

        // Progress to the next quest if available
        if (currentQuestIndex + 1 < quests.Count)
        {
            OpenQuestByIndex(currentQuestIndex + 1);
        }
    }
}