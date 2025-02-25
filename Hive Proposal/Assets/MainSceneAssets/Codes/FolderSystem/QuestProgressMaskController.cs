using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class QuestProgressMaskController : MonoBehaviour
{
    [Header("References")]
    public UICharts uiCharts; // Reference to UICharts for tracking pages
    public QuestGiver questGiver; // Reference to listen for quest index updates
    public Image maskImage; // UI Image controlling fill amount

    [System.Serializable]
    public class QuestFillData
    {
        public int questIndex;
        [Range(0f, 1f)] public float fillAmount;
    }
    public List<QuestFillData> questFillDataList = new List<QuestFillData>();

    private void Start()
    {
        // Subscribe directly without extra method
        questGiver.OnQuestIndexChanged.AddListener(_ => UpdateMaskState());

        UpdateMaskState();
    }

    private void Update()
    {
        UpdateMaskState();
    }
    private void UpdateMaskState()
    {
        int currentPage = uiCharts.GetCurrentPageIndex();
        int lastPageIndex = uiCharts.GetLastPageIndex();

        bool isLastPage = (currentPage == lastPageIndex);
        maskImage.gameObject.SetActive(isLastPage);

        if (isLastPage)
            maskImage.fillAmount = GetFillAmountForQuest(questGiver.GetCurrentQuestIndex());
    }

    private float GetFillAmountForQuest(int questIndex)
    {
        float fillAmount = 0f;

        foreach (var questData in questFillDataList)
        {
            if (questIndex >= questData.questIndex)
                fillAmount = questData.fillAmount;
            else
                break; // Stop once a higher quest index is found
        }

        return fillAmount;
    }
}
