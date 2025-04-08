using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class QuizResponses : MonoBehaviour
{
    [System.Serializable]
    public class Response
    {
        public Question Question;
        public int SelectedResponseIndex;
    }

    [Header("Responses")]
    [SerializeField] public List<Response> responses = new List<Response>();

    [Header("UI References")]
    [SerializeField] private RectTransform ContentArea;
    [SerializeField] private GameObject questionTextPrefab;
    [SerializeField] private GameObject rightAnswerPrefab;
    [SerializeField] private GameObject wrongAnswerPrefab;
    [SerializeField] private ScrollRect scrollRect;

    public void ShowWrongAnswersScreen()
    {
        foreach (Transform child in ContentArea)
            Destroy(child.gameObject);

        foreach (var r in responses)
        {
            var q = r.Question;
            if (q == null) continue;

            // 1. Instantiate question text
            var questionGO = Instantiate(questionTextPrefab, ContentArea);
            var questionTMP = questionGO.GetComponentInChildren<TextMeshProUGUI>();
            if (questionTMP != null)
                questionTMP.text = q.Info;

            // 2. Show correct answers
            var correctIndexes = q.GetCorrectAnswers();
            foreach (var idx in correctIndexes)
            {
                var correctObj = Instantiate(rightAnswerPrefab, ContentArea);
                var tmp = correctObj.GetComponentInChildren<TextMeshProUGUI>();
                if (tmp != null)
                    tmp.text = q.Answers[idx].Info;
            }

            // 3. Show user's wrong answer
            if (!correctIndexes.Contains(r.SelectedResponseIndex))
            {
                var wrongObj = Instantiate(wrongAnswerPrefab, ContentArea);
                var tmp = wrongObj.GetComponentInChildren<TextMeshProUGUI>();
                if (tmp != null)
                    tmp.text = q.Answers[r.SelectedResponseIndex].Info;
            }
        }

        // Scroll to top
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 1f;
    }

    public void OnCheckMistakesButtonPressed()
    {
        gameObject.SetActive(true);
        ShowWrongAnswersScreen();
    }
}
