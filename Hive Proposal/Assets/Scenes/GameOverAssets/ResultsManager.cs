using UnityEngine;

public class ResultsManager : MonoBehaviour
{
    public static ResultsManager Instance { get; private set; }

    [SerializeField] private float preQuizTime = 0f;
    [SerializeField] private float mainTime = 0f;
    [SerializeField] private float postQuizTime = 0f;

    [SerializeField] private int preQuizScore = 0;
    [SerializeField] private int mainScore = 0;
    [SerializeField] private int postQuizScore = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    public void SetPreQuizTime(float time) => preQuizTime = time;
    public float GetPreQuizTime() => preQuizTime;

    public void SetPreQuizScore(int score) => preQuizScore = score;
    public int GetPreQuizScore() => preQuizScore;

    public void SetMainQuizTime(float time) => mainTime = time;
    public float GetMainQuizTime() => mainTime;

    public void SetMainQuizScore(int score) => mainScore = score;
    public int GetMainQuizScore() => mainScore;

    public void SetPostQuizTime(float time) => postQuizTime = time;
    public float GetPostQuizTime() => postQuizTime;

    public void SetPostQuizScore(int score) => postQuizScore = score;
    public int GetPostQuizScore() => postQuizScore;

    public float GetTotalTime() => preQuizTime + mainTime + postQuizTime;
    public int GetTotalScore() => preQuizScore + mainScore + postQuizScore;

    public void Clear()
    {
        preQuizTime = mainTime = postQuizTime = 0f;
        preQuizScore = mainScore = postQuizScore = 0;
    }
}
