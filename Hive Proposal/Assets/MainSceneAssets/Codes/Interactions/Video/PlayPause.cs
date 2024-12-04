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

    private void Start()
    {
        if (questGiver != null)
        {
            questGiver.OnQuestIndexChanged.AddListener(UpdateVideoState);
            UpdateVideoState(questGiver.GetCurrentQuestIndex());
        }
    }


    void Update()
    {
      
        if (Input.GetKeyDown(KeyCode.BackQuote)) //key `
        {
            SkipToEnd();
        }
    }

    private void UpdateVideoState(int currentQuestIndex)
    {
        Debug.Log($"Current Quest Index in UpdateVideoState: {currentQuestIndex}");

        bool foundMatchingQuest = false;

        foreach (var mapping in videoQuestMappings)
        {
            Debug.Log($"Checking Mapping: Required Index {mapping.requiredQuestIndex}, Current Index {currentQuestIndex}");

            if (currentQuestIndex == mapping.requiredQuestIndex)
            {
                Debug.Log($"Match Found! Activating Video Player for Quest Index {currentQuestIndex}");
                mapping.videoPlayer.SetActive(true);
                playPauseButton.SetActive(true);
                foundMatchingQuest = true;
            }
            else
            {
                mapping.videoPlayer.SetActive(false);
            }
        }

        if (!foundMatchingQuest)
        {
            Debug.LogWarning($"No matching video found for quest index: {currentQuestIndex}");
            playPauseButton.SetActive(false);
        }
    }


    private void OnMouseDown()
    {
        if (questGiver != null)
        {
            int currentQuestIndex = questGiver.GetCurrentQuestIndex(); // Use GetCurrentQuestIndex()

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
            activeVideoPlayer.time = activeVideoPlayer.clip.length - 0.1f;
            activeVideoPlayer.Play();

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
        if (questGiver != null)
        {
            Debug.Log("Calling QuestComplete from OnVideoEnd");
            questGiver.QuestComplete();
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