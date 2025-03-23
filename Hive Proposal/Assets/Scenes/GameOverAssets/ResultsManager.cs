using UnityEngine;

[CreateAssetMenu(fileName = "ResultsManager", menuName = "Quiz/Results Data")]
public class ResultsManager : ScriptableObject
{
    private float preQuizTime;
    private float mainQuizTime;
    private float postQuizTime;

    private int preQuizScore;
    private int mainQuizScore;
    private int postQuizScore;

    public void SetPreQuizTime(float time) => preQuizTime = time;
    public float GetPreQuizTime() => preQuizTime;

    public void SetPreQuizScore(int score) => preQuizScore = score;
    public int GetPreQuizScore() => preQuizScore;

    public void SetMainQuizTime(float time) => mainQuizTime = time;
    public float GetMainQuizTime() => mainQuizTime;

    public void SetMainQuizScore(int score) => mainQuizScore = score;
    public int GetMainQuizScore() => mainQuizScore;

    public void SetPostQuizTime(float time) => postQuizTime = time;
    public float GetPostQuizTime() => postQuizTime;

    public void SetPostQuizScore(int score) => postQuizScore = score;
    public int GetPostQuizScore() => postQuizScore;

    public float GetTotalTime() => preQuizTime + mainQuizTime + postQuizTime;
    public int GetTotalScore() => preQuizScore + mainQuizScore + postQuizScore;

    public void Clear()
    {
        preQuizTime = mainQuizTime = postQuizTime = 0f;
        preQuizScore = mainQuizScore = postQuizScore = 0;
    }
}
