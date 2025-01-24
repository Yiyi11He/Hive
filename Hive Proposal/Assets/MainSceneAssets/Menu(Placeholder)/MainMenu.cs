using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void GoTo4()
    {
        SceneManager.LoadScene(4);
    }
}
