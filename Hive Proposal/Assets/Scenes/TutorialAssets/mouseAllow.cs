using UnityEngine;

public class mouseAllow : MonoBehaviour
{
    [Header("References")]
    public GameObject PauseMenu;
    public MonoBehaviour playerController;
    public MouseLook mouseLook;            

    private bool isPaused = false;
    private CursorLockMode _cachedLockState;
    private bool _cachedVisibleState;

    void Awake()
    {
        LockOutPlayer();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                PauseGame();
        }
    }

    private void LockOutPlayer()
    {
        if (playerController != null)
            playerController.enabled = false;

        if (mouseLook != null)
        {
            mouseLook.enabled = false;
        }

        Time.timeScale = 1;
        _cachedVisibleState = Cursor.visible;
        _cachedLockState = Cursor.lockState;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    public void Resume()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;

        if (playerController != null)
            playerController.enabled = true;

        if (mouseLook != null)
        {
            mouseLook.enabled = true;
            mouseLook.lookEnabled = true; 
        }

        Cursor.lockState = _cachedLockState;
        Cursor.visible = _cachedVisibleState;
    }


    private void PauseGame()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;

        if (playerController != null)
            playerController.enabled = false;

        if (mouseLook != null)
            mouseLook.lookEnabled = false;

        _cachedVisibleState = Cursor.visible;
        _cachedLockState = Cursor.lockState;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
