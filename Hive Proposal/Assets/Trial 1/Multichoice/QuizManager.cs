using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuizManager : MonoBehaviour
{
    public List<QuestionAnswers> QnA;
    public List<AnswerScript> options;
    public TMP_Text QuestionTxt;
    public AnswerScript answer1;

    private int currentQuestion;

    private void Start()
    {
        GenerateQuestion();
    }

    public void Correct()
    {
        QnA.RemoveAt(currentQuestion);
        GenerateQuestion();
    }

    void SetAnswers()
    {
        for (int i = 0; i < options.Count; i++)
        {
            options[i].isCorrect = false;

            //if (options[i].label != null)
            //{
            //    options[i].label.text = QnA[currentQuestion].Answers[i];
            //}

            if (QnA[currentQuestion].CorrectAnswer == i + 1)
            {
                options[i].isCorrect = true;
            }
        }
    }

    void GenerateQuestion()
    {
        currentQuestion = Random.Range(0, QnA.Count);

        QuestionTxt.text = QnA[currentQuestion].Question;
        SetAnswers();
    }
}


//    private void Start()
//    {
//        generateQuestion();

//    }

//    public void correct()
//    {
//        QnA.RemoveAt(currentQuestion);
//        generateQuestion();

//    }

//    //void SetAnswers()
//    //{
//    //    for (int i = 0; i < options.Length; i++)
//    //    {
//    //        options[i].isCorrect = false;
//    //        options[i].label.text = QnA[currentQuestion].Answers[i];

//    //        //options[i].transform.GetChild(0).GetComponent<TMPro.TextMeshPro>().text = QnA[currentQuestion].Answers[i];

//    //        if (QnA[currentQuestion].CorrectAnswer == i + 1)
//    //        {
//    //            options[i].GetComponent<AnswerScript>().isCorrect = true;
//    //        }

//    //    }
//    //}

//    void SetAnswers()
//    {
//        for (int i = 0; i < options.Length; i++)
//        {
//            options[i].isCorrect = false;

//            // Check if the label is not null before updating text
//            if (options[i].label != null)
//            {
//                options[i].label.text = QnA[currentQuestion].Answers[i];
//            }

//            if (QnA[currentQuestion].CorrectAnswer == i + 1)
//            {
//                options[i].isCorrect = true;
//            }
//        }
//    }

//    void generateQuestion()
//    {
//        currentQuestion = Random.Range(0, QnA.Count);

//        QuestionTxt.text = QnA[currentQuestion].Question;
//        SetAnswers();
//    }
//}