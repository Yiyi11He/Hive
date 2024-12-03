using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PlayPause : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject canvasElement;
    public RectTransform videoTransform;

    public float zoomDuration = 0.5f;
    private bool isPlaying = false;
    private bool isCanvasEnabled = false;

    void OnMouseDown() 
    {
        TogglePlayPause();
    }

    public void TogglePlayPause()
    {
        isPlaying = !isPlaying;

        if (isPlaying)
        {
            videoPlayer.Play();
            EnableCanvas(true);
            StartCoroutine(ZoomVideo(0.6f, 1.0f));
        }
        else
        {
            videoPlayer.Pause();
        }
    }

    public void ToggleCanvas()
    {
        isCanvasEnabled = !isCanvasEnabled;
        EnableCanvas(isCanvasEnabled);
    }

    private void EnableCanvas(bool enable)
    {
        if (canvasElement != null)
        {
            canvasElement.SetActive(enable);
        }
    }
    private IEnumerator ZoomVideo(float startScale, float endScale)
    {
        float elapsed = 0f;
        Vector2 originalSize = videoTransform.sizeDelta;

       
        Vector2 startSize = originalSize * startScale;
        Vector2 endSize = originalSize * endScale;

      
        while (elapsed < zoomDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / zoomDuration;
            videoTransform.sizeDelta = Vector2.Lerp(startSize, endSize, t);
            yield return null;
        }

        videoTransform.sizeDelta = endSize;
    }
}