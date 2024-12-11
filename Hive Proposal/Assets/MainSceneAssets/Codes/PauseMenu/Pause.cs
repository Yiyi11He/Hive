using UnityEngine;
using UnityEngine.Video;

public class Pause : MonoBehaviour
{
    public GameObject PauseMenu; 
    private bool isPaused = false; 


    public MonoBehaviour playerController; 

    public PlayPause playPauseScript;

    private CursorLockMode _cachedLockState;
    private bool _cachedVisibleState;

    void Start()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1; 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void Resume()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1; // Resume time
        isPaused = false;

        // Re-enable player controls
        if (playerController != null)
        {
            playerController.enabled = true;
        }

        VideoPlayer activeVideoPlayer = playPauseScript.GetActiveVideoPlayer();
        if (activeVideoPlayer != null && activeVideoPlayer.isPaused)
        {
            activeVideoPlayer.Play();
        }

        // Re-enable cursor movement
        Cursor.lockState = _cachedLockState;
        Cursor.visible = _cachedVisibleState;
    }

    void PauseGame()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0; // Freeze time
        isPaused = true;

        // Disable player controls
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        VideoPlayer activeVideoPlayer = playPauseScript.GetActiveVideoPlayer();
        if (activeVideoPlayer != null && activeVideoPlayer.isPlaying)
        {
            activeVideoPlayer.Pause();
        }

        // Show the cursor
        _cachedVisibleState = Cursor.visible;
        _cachedLockState = Cursor.lockState;

        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true; 
    }
}
