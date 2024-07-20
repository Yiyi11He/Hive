using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Reflection;

public class QuestGiver : MonoBehaviour
{
    public Quest quest;

    public PlayerMovement player;

    public GameObject questWindow;
    public TMP_Text TitleText;
    public TMP_Text DescriptionText;
    public TMP_Text ScoreText;


    public void OpenQuest()
    {
        questWindow.SetActive(true);
        TitleText.text = quest.title;
        DescriptionText.text = quest.description;
        ScoreText.text = quest.Score.ToString();
        player.quest = quest;
    }
}
