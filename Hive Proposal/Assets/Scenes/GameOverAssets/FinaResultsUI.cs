using UnityEngine;

public class FinalResultsUI : MonoBehaviour
{
    public float preGameTime { get; private set; }
    public float mainGameTime { get; private set; }
    public float postGameTime { get; private set; }

    public int preGameScore { get; private set; }
    public int mainGameScore { get; private set; }
    public int postGameScore { get; private set; }

    private float sceneTimer = 0f;
    private bool isTracking = false;

    private enum SceneType { None, Pre, Main, Post }
    private SceneType currentScene = SceneType.None;

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
            sceneTimer += Time.deltaTime;
        }
    }

    public void StartTracking(string sceneIdentifier)
    {
        isTracking = true;
        sceneTimer = 0f;

        if (sceneIdentifier.ToLower().Contains("pre")) currentScene = SceneType.Pre;
        else if (sceneIdentifier.ToLower().Contains("main")) currentScene = SceneType.Main;
        else if (sceneIdentifier.ToLower().Contains("post")) currentScene = SceneType.Post;
        else currentScene = SceneType.None;
    }

    public void StopTrackingAndSave(int finalScore)
    {
        isTracking = false;

        switch (currentScene)
        {
            case SceneType.Pre:
                preGameTime = sceneTimer;
                preGameScore = finalScore;
                break;
            case SceneType.Main:
                mainGameTime = sceneTimer;
                mainGameScore = finalScore;
                break;
            case SceneType.Post:
                postGameTime = sceneTimer;
                postGameScore = finalScore;
                break;
        }
    }

    public float GetTotalTime()
    {
        return preGameTime + mainGameTime + postGameTime;
    }

    public int GetTotalScore()
    {
        return preGameScore + mainGameScore + postGameScore;
    }

    public void ResetAll()
    {
        preGameTime = mainGameTime = postGameTime = 0f;
        preGameScore = mainGameScore = postGameScore = 0;
        isTracking = false;
        sceneTimer = 0f;
        currentScene = SceneType.None;
    }
}
