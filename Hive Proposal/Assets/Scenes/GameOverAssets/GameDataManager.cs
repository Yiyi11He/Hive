using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;

    public float TimeSpentInScene { get; private set; }
    public int FinalScore { get; private set; }

    private bool isTracking = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Update()
    {
        if (isTracking)
        {
            TimeSpentInScene += Time.deltaTime;
        }
    }

    public void StartTracking()
    {
        TimeSpentInScene = 0f;
        isTracking = true;
    }

    public void StopTracking()
    {
        isTracking = false;
    }

    public void SetFinalScore(int score)
    {
        FinalScore = score;
    }

    public void ResetTracker()
    {
        TimeSpentInScene = 0f;
        FinalScore = 0;
        isTracking = false;
    }

}
