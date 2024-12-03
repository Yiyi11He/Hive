using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PlayPause : MonoBehaviour
{
    [System.Serializable]
    public class VideoQuestMapping
    {
        public GameObject videoPlayer; // The Video Player GameObject
        public int requiredQuestIndex; // The quest index needed to unlock this video
    }

    [SerializeField] public List<VideoQuestMapping> videoQuestMappings; 
    public GameObject canvasElement;
    public RectTransform videoTransform;

    public float zoomDuration = 0.5f;
    private bool isPlaying = false;
    private bool isCanvasEnabled = false;

    public QuestGiver questGiver; 
    public GameObject playPauseButton; 

    private int activeQuestIndex = -1; 

    void Start()
    {
        UpdateVideoState(); 
    }

    void Update()
    {
        UpdateVideoState(); 
        
        if (Input.GetKeyDown(KeyCode.BackQuote)) // Backtick key
        {
            SkipToEnd();
        }
    }

    private void UpdateVideoState()
    {
        if (questGiver != null && questGiver.player.quest != null)
        {
            int currentQuestIndex = questGiver.quests.IndexOf(questGiver.player.quest);

            // Only update video state if the quest index changes
            if (currentQuestIndex != activeQuestIndex)
            {
                activeQuestIndex = currentQuestIndex;

                foreach (var mapping in videoQuestMappings)
                {
                    if (currentQuestIndex == mapping.requiredQuestIndex)
                    {
                        mapping.videoPlayer.SetActive(true); // Enable the matching video
                        playPauseButton.SetActive(true); // Enable play/pause button
                    }
                    else
                    {
                        mapping.videoPlayer.SetActive(false); // Disable non-matching videos
                    }
                }

                // Disable play/pause button if no matching quest is found
                if (currentQuestIndex < 0 || currentQuestIndex >= videoQuestMappings.Count)
                {
                    playPauseButton.SetActive(false);
                }
            }
        }
    }

    void OnMouseDown()
    {
        
        if (questGiver != null && questGiver.player.quest != null)
        {
            int currentQuestIndex = questGiver.quests.IndexOf(questGiver.player.quest);

            foreach (var mapping in videoQuestMappings)
            {
                if (currentQuestIndex == mapping.requiredQuestIndex)
                {
                    TogglePlayPause(mapping.videoPlayer.GetComponent<VideoPlayer>());
                    return;
                }
            }

            Debug.LogWarning($"No matching video for quest index {currentQuestIndex}");
        }
    }

    public void TogglePlayPause(VideoPlayer videoPlayer)
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

    public VideoPlayer GetActiveVideoPlayer()
    {
        foreach (var mapping in videoQuestMappings)
        {
            if (mapping.videoPlayer.activeSelf)
            {
                return mapping.videoPlayer.GetComponent<VideoPlayer>();
            }
        }
        return null;
    }

    //delete this on deploy
    public void SkipToEnd()
    {
        VideoPlayer activeVideoPlayer = GetActiveVideoPlayer();

        if (activeVideoPlayer != null)
        {
            // Skip to the last frame and start playing
            activeVideoPlayer.time = activeVideoPlayer.clip.length - 0.1f;
            activeVideoPlayer.Play();

            // Attach an event to check when the video finishes
            activeVideoPlayer.loopPointReached += OnVideoEnd;
        }
    }

    private void OnVideoEnd(VideoPlayer videoPlayer)
    {
        isPlaying = false;
        isCanvasEnabled = false;

        videoPlayer.Stop();
        EnableCanvas(false);

        Debug.Log($"Video {videoPlayer.clip.name} has ended. Resetting state.");
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
