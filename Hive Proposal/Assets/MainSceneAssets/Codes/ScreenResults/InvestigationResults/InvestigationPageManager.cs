using System.Collections.Generic;
using UnityEngine;
public class InvestigationPageManager : MonoBehaviour
{
    [Header("Quest Progression")]
    public QuestGiver questGiver;

    private List<GameObject> availablePages = new List<GameObject>();
    private int currentPageIndex = -1;

    [Header("Quest-based Unlocking")]
    public List<QuestPageUnlock> questPageUnlocks;

    void Start()
    {
        if (questGiver != null)
        {
            questGiver.OnQuestIndexChanged.AddListener(UpdatePagesOnQuestProgress);
        }

        UpdateUnlockedPages();
        ShowLatestPage();
    }

    private void UpdateUnlockedPages()
    {
        int currentQuestIndex = questGiver.GetCurrentQuestIndex();
        QuestPageUnlock lastValidUnlock = null;

        foreach (var unlockEntry in questPageUnlocks)
        {
            if (unlockEntry.questIndex <= currentQuestIndex)
            {
                lastValidUnlock = unlockEntry;
            }
        }

        if (lastValidUnlock != null)
        {
            availablePages = new List<GameObject>(lastValidUnlock.pagesToUnlock);
        }
    }

    private void ShowLatestPage()
    {
        foreach (GameObject page in availablePages)
        {
            page.SetActive(false);
        }

        if (availablePages.Count > 0)
        {
            currentPageIndex = availablePages.Count - 1; // Show the latest unlocked page
            availablePages[currentPageIndex].SetActive(true);
        }
    }

    private void UpdatePagesOnQuestProgress(int questIndex)
    {
        UpdateUnlockedPages();
        ShowLatestPage();
    }

    public int GetCurrentPageIndex()
    {
        return currentPageIndex;
    }

    public int GetLastPageIndex()
    {
        return availablePages.Count - 1;
    }
}
