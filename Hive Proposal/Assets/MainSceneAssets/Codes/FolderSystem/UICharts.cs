using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class QuestPageUnlock
{
    public int questIndex;
    public List<GameObject> pagesToUnlock;
}

public class UICharts : MonoBehaviour
{
    [Header("Quest Progression")]
    public QuestGiver questGiver;

    [Header("Page Navigation")]
    public Button nextPageButton;
    public Button prevPageButton;

    private int currentPageIndex = 0;
    private List<GameObject> availablePages = new List<GameObject>(); // List of unlocked pages

    [Header("Quest-based Unlocking")]
    public List<QuestPageUnlock> questPageUnlocks;


    void Start()
    {
        if (questGiver != null)
        {
            questGiver.OnQuestIndexChanged.AddListener(UpdatePagesOnQuestProgress);
        }

        UpdateUnlockedPages();
        SetFirstAvailablePage();
        UpdatePage();
        UpdateButtonState();
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



    private void SetFirstAvailablePage()
    {
        currentPageIndex = availablePages.Count > 0 ? 0 : -1;
    }

    public void NextPage()
    {
        if (currentPageIndex < availablePages.Count - 1)
        {
            currentPageIndex++;
            UpdatePage();
        }
        UpdateButtonState();
    }

    public void PreviousPage()
    {
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            UpdatePage();
        }
        UpdateButtonState();
    }

    private void UpdatePage()
    {
        foreach (GameObject page in availablePages)
        {
            page.SetActive(false);
        }

        if (currentPageIndex >= 0 && currentPageIndex < availablePages.Count)
        {
            availablePages[currentPageIndex].SetActive(true);
        }
    }

    private void UpdateButtonState()
    {
        prevPageButton.gameObject.SetActive(currentPageIndex > 0);
        nextPageButton.gameObject.SetActive(currentPageIndex < availablePages.Count - 1);
    }

    private void UpdatePagesOnQuestProgress(int questIndex)
    {
        UpdateUnlockedPages();
        SetFirstAvailablePage();
        UpdatePage();
        UpdateButtonState();
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

