using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public class VideoPlayerDetect: MonoBehaviour
{
    public UnityEvent OnVideoPlayComplete;

    public VideoPlayer player;

    private void Awake()
    {
        // For dev only
        PlayVideo(player.clip);
    }

    public void PlayVideo (VideoClip clip)
    {
        player.clip = clip;
        player.Play();
        StartCoroutine(PlayRoutine());
    }

    private IEnumerator PlayRoutine()
    {
        while (player.isPlaying)
            yield return null;

        Debug.Log("Video play complete");
        OnVideoPlayComplete?.Invoke();
    }
}
