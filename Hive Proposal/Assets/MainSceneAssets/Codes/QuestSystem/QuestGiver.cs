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
    [SerializeField] private bool debugMode = false;
    [SerializeField] private int debugQuestIndex = 0;

    public List<Quest> quests;
    public PlayerMovement player;
    public Scores scoreManager;

    [Space]
    public GameObject questWindow;
    public TMP_Text TitleText;
    public TMP_Text DescriptionText;
    public TMP_Text ScoreText;

    [SerializeField] private Animator questAnimator;
    public UnityEvent<int> OnQuestIndexChanged;
    private int lastShownQuestIndex = -1;

    [Space]
    [Header("Audio Settings")]
    [SerializeField] private AudioSource questAudioSource;
    [SerializeField] private AudioClip newQuestSound;

    private int currentQuestIndex = 0;

    private void Awake()
    {
        OnQuestIndexChanged = new UnityEvent<int>();
        currentQuestIndex = debugMode ? debugQuestIndex : 0;
        OpenQuestByIndex(currentQuestIndex);
    }

    public int GetCurrentQuestIndex() => currentQuestIndex;

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


        if (currentQuestIndex == lastShownQuestIndex)
        {
            ShowQuestImmediately(quest);
            return;
        }

        lastShownQuestIndex = currentQuestIndex;


        if (questAudioSource != null && newQuestSound != null)
        {
            questAudioSource.PlayOneShot(newQuestSound);
        }

        if (questWindow != null && questAnimator != null && questWindow.activeSelf)
        {
            StartCoroutine(AnimateQuestTransition(quest));
        }
        else
        {
            ShowQuestImmediately(quest);
        }
    }



    private IEnumerator AnimateQuestTransition(Quest quest)
    {

        questWindow.SetActive(true);


        questAnimator.SetTrigger("Fade");


        yield return new WaitForSeconds(1f);

        ShowQuestImmediately(quest);
    }

    private void ShowQuestImmediately(Quest quest)
    {
        questWindow.SetActive(true);
        TitleText.text = quest.title;
        DescriptionText.text = quest.description;
        ScoreText.text = $"Score + {quest.Score}";
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
                scoreManager.AddPlayerScore(completedQuest.Score);

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
                    QuestComplete();
                break;
            }
        }
    }

    public void SetQuestToIndexZero(int questIndex)
    {
        if (questIndex < 0 || questIndex >= quests.Count)
        {
            Debug.LogError($"Invalid quest index: {questIndex}");
            return;
        }

        Quest selectedQuest = quests[questIndex];
        quests.RemoveAt(questIndex);
        quests.Insert(0, selectedQuest);

        Debug.Log($"Quest \"{selectedQuest.title}\" has been moved to index 0.");
        OpenQuestByIndex(0);
    }

    public void HideQuestUI()
    {
        if (questWindow != null)
            questWindow.SetActive(false);
    }

    public void SetQuestUIVisible(bool isVisible)
    {
        if (questWindow == null) return;

        for (int i = 0; i < questWindow.transform.childCount; i++)
        {
            questWindow.transform.GetChild(i).gameObject.SetActive(isVisible);
        }
    }


}
