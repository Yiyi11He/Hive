using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "Results Data")]
public class ResultsData : ScriptableObject
{
    public string fileName;

    [SerializeField] private string studentNumber;
    [SerializeField] private string preQuizScore;
    [SerializeField] private string postQuizScore;

    public void SetStudentNumber(string id)
    {
        Debug.Log($"Setting student number: {id}");
        studentNumber = id.ToString();
    }

    public void SetPreQuizScore (int score)
    {
        Debug.Log($"Setting pre quiz score: {score}");

        preQuizScore = score.ToString();
    }

    public void SetPostQuizScore(int score)
    {
        Debug.Log($"Setting post quiz score: {score}");
        postQuizScore = score.ToString();
    }

    public void Clear()
    {
        studentNumber = string.Empty;
        preQuizScore = string.Empty;
        postQuizScore = string.Empty;
    }

    public void Record()
    {
        string path = Application.persistentDataPath + "/" + fileName;
        Debug.Log($"Writing to {path}");

        if (!File.Exists(path))
        {
            using (var writer = new StreamWriter(path, true))
            {
                writer.WriteLine($"Student Number,PreQuizScore,PostQuizScore");
            }
        }

        using (var writer = new StreamWriter(path, true))
        {
            string output = $"{studentNumber},{preQuizScore},{postQuizScore}";
            Debug.Log($"Writing results: {output}");
            writer.WriteLine($"{output}");
        }
    }
}
