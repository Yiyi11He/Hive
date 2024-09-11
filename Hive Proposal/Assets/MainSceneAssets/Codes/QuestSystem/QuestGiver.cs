using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Reflection;
using Yarn.Unity;

public class QuestGiver : MonoBehaviour
{
    public List<Quest> quests;

    public PlayerMovement player;

    public GameObject questWindow;
    public TMP_Text TitleText;
    public TMP_Text DescriptionText;
    public TMP_Text ScoreText;

    private void Awake()
    {
        OpenQuestByIndex(0);
    }

    [YarnCommand("open_quest")]
    public void OpenQuestByIndex(int questIndex)
    {
        Debug.Log($"Opening quest {questIndex}");
        OpenQuest(quests[questIndex]);
    }

    public void OpenQuest(Quest quest)
    {
        questWindow.SetActive(true);
        TitleText.text = quest.title;
        DescriptionText.text = quest.description;
        ScoreText.text = quest.Score.ToString();
        player.quest = quest;
    }

    public void QuestComplete()
    {
        questWindow.SetActive(false);
    }
}
