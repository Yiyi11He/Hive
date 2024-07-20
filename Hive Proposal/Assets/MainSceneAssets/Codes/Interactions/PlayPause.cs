using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PlayPause : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    private bool isPlaying = false;

    void OnMouseDown()
    {
        isPlaying = !isPlaying;
        if (isPlaying) videoPlayer.Play();
        else videoPlayer.Pause();
    }
}
