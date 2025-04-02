using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuizResponses : MonoBehaviour
{
    [System.Serializable]
    public class Response
    {
        public Question Question;
        public int SelectedResponseIndex;
    }

    [Header("All player responses (populated at runtime)")]
    public List<Response> responses = new List<Response>();

    [Header("UI References for Wrong Answers Screen")]
    [Tooltip("Parent content transform under the ScrollRect, e.g. WrongAnswers -> Viewport -> Content -> A_ContentArea")]
    public Transform answersContentParent;

    [Tooltip("Prefab or TMP text object used to display the question text")]
    public TMP_Text questionTitlePrefab;

    [Tooltip("Prefab or TMP text object used to display an answer line")]
    public TMP_Text answerTextPrefab;

    [Header("Highlight Colors")]
    public Color correctColor = Color.yellow;
    public Color wrongColor = Color.red;

    /// <summary>
    /// Call this method when the quiz is over to show a list of wrong answers.
    /// </summary>
    public void ShowWrongAnswersScreen()
    {
        if (answersContentParent == null
            || questionTitlePrefab == null
            || answerTextPrefab == null)
        {
            Debug.LogWarning("Please assign references in the QuizResponses inspector!");
            return;
        }

        // Clear out any existing children from a previous run
        foreach (Transform child in answersContentParent)
        {
            Destroy(child.gameObject);
        }

        // Build entries for each question the player answered
        foreach (var response in responses)
        {
            var question = response.Question;
            if (question == null)
                continue;

            // 1) Create a “Question Title” text object
            var titleObj = Instantiate(questionTitlePrefab, answersContentParent);
            titleObj.text = question.Info;    // The question prompt (e.g. “What antibiotic…?”)
            titleObj.color = Color.white;     // or however you want the question title styled

            // 2) Figure out which answers were correct
            List<int> correctAnswers = question.GetCorrectAnswers();
            // (Your Question script’s “GetCorrectAnswers” usually returns a List<int> of correct indices)

            // 3) If the question is single-answer:
            //    - If SelectedResponseIndex is NOT correct, display that answer in red
            //    - Also show the correct answer(s) in yellow
            //
            // For multi-answer questions, you'd store multiple indices in `Response`,
            // then highlight any mismatch. But let's assume single for now.

            bool playerWasWrong = !correctAnswers.Contains(response.SelectedResponseIndex);

            // Show the correct answer(s) in yellow
            foreach (var idx in correctAnswers)
            {
                var correctObj = Instantiate(answerTextPrefab, answersContentParent);
                correctObj.text = $"Correct: {question.Answers[idx].Info}";
                correctObj.color = correctColor;
            }

            // If the player was wrong, highlight their chosen answer in red
            if (playerWasWrong && response.SelectedResponseIndex >= 0
                                && response.SelectedResponseIndex < question.Answers.Length)
            {
                var wrongObj = Instantiate(answerTextPrefab, answersContentParent);
                wrongObj.text = $"Your Pick: {question.Answers[response.SelectedResponseIndex].Info}";
                wrongObj.color = wrongColor;
            }

            // If you want to show the player’s picked answer even if correct, you could do that too.
        }

        // Finally, ensure your “WrongAnswers” panel is active,
        // so the ScrollRect is visible with the newly?instantiated content.
    }
}
