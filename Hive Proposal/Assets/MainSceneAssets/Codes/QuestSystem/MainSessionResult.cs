using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSessionResult : MonoBehaviour
{
    [Header("Tracking Settings")]
    [SerializeField] private Timer timer;
    [SerializeField] private Scores score;

    private bool hasBeenActivated = false;

    private void OnEnable()
    {
        if (!hasBeenActivated)
        {
            hasBeenActivated = true;
            Debug.Log($"[{name}] Activated for the first time. Tracking started.");
        }
    }

    private void OnDisable()
    {
        if (!hasBeenActivated) return;

        float timeSpent = timer != null ? timer.GetElapsedTime() : 0f;
        int scoreValue = score != null ? score.Score : 0;

        ResultsManager.Instance.SetMainQuizTime(timeSpent);
        ResultsManager.Instance.SetMainQuizScore(scoreValue);

        Debug.Log($"[{name}] Deactivated. Main session logged — Time: {timeSpent}s, Score: {scoreValue}");

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}
