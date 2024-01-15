using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class ProgressToPostQuiz : MonoBehaviour
{
    public InputAction nextSceneButton;
    //public VideoPlayer video;

    private void Awake()
    {
        nextSceneButton.performed += OnLoadNextScene;
        nextSceneButton.Enable();
    }

    private void OnLoadNextScene(InputAction.CallbackContext context)
    {
        LoadScene();
        //video.Pause();
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
