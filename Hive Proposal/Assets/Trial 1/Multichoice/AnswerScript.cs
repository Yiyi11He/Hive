using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class AnswerScript : MonoBehaviour
{
    public bool isCorrect = false;
    public QuizManager quizManager;
    [SerializeField] private TMP_Text label;

    private float PlayerScore;

    public void Answer()
    {
        PlayerScore = 0;
        if(isCorrect)
        {
            Debug.Log("Correct Answer");
            quizManager.Correct();
            PlayerScore += 1;
        }
        else
        {
            Debug.Log("Wrong Answer");
        }

    }
}
