using UnityEngine;

public class FinalResultsRecorder : MonoBehaviour
{
    public static FinalResultsRecorder Instance { get; private set; }

    public float preGameTime { get; private set; }
    public float mainGameTime { get; private set; }
    public float postGameTime { get; private set; }

    public int preGameScore { get; private set; }
    public int mainGameScore { get; private set; }
    public int postGameScore { get; private set; }

    private float sceneTimer = 0f;
    private bool isTracking = false;

    public enum SceneType { None, Pre, Main, Post }
    [SerializeField] private SceneType currentScene = SceneType.None;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    void Update()
    {
        if (isTracking)
        {
            sceneTimer += Time.deltaTime;
        }
    }

    public static void EnsureInstance()
    {
        if (Instance == null)
        {
            var existing = FindObjectOfType<FinalResultsRecorder>();
            if (existing != null)
            {
                Instance = existing;
                DontDestroyOnLoad(existing.gameObject);
            }
            else
            {
                var prefab = Resources.Load<FinalResultsRecorder>("FinalResultsRecorder");
                if (prefab != null)
                {
                    var obj = Instantiate(prefab);
                    obj.name = "FinalResultsRecorder (Runtime)";
                }
                else
                {
                    Debug.LogWarning("FinalResultsRecorder prefab not found in Resources.");
                }
            }
        }
    }

    public void StartTracking(string sceneIdentifier)
    {
        EnsureInstance();

        isTracking = true;
        sceneTimer = 0f;

        if (sceneIdentifier.ToLower().Contains("pre")) currentScene = SceneType.Pre;
        else if (sceneIdentifier.ToLower().Contains("main")) currentScene = SceneType.Main;
        else if (sceneIdentifier.ToLower().Contains("post")) currentScene = SceneType.Post;
        else currentScene = SceneType.None;
    }

    public void StopTrackingAndSave(int finalScore)
    {
        EnsureInstance();

        isTracking = false;

        switch (currentScene)
        {
            case SceneType.Pre:
                preGameTime = sceneTimer;
                preGameScore = finalScore;
                ResultsManager.Instance.SetPreQuizTime(preGameTime);
                ResultsManager.Instance.SetPreQuizScore(preGameScore);
                break;

            case SceneType.Main:
                mainGameTime = sceneTimer;
                mainGameScore = finalScore;
                ResultsManager.Instance.SetMainQuizTime(mainGameTime);
                ResultsManager.Instance.SetMainQuizScore(mainGameScore);
                break;

            case SceneType.Post:
                postGameTime = sceneTimer;
                postGameScore = finalScore;
                ResultsManager.Instance.SetPostQuizTime(postGameTime);
                ResultsManager.Instance.SetPostQuizScore(postGameScore);
                break;
        }
    }

    public float GetTotalTime() => preGameTime + mainGameTime + postGameTime;
    public int GetTotalScore() => preGameScore + mainGameScore + postGameScore;

    public void ResetAll()
    {
        preGameTime = mainGameTime = postGameTime = 0f;
        preGameScore = mainGameScore = postGameScore = 0;
        isTracking = false;
        sceneTimer = 0f;
        currentScene = SceneType.None;

        ResultsManager.Instance.Clear(); // Optional: clear stored data at runtime
    }
}
