using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoDoor : MonoBehaviour
{
    [System.Serializable]
    public class VideoQuestMapping
    {
        public GameObject videoPlayer;      // The Video Player GameObject
        public int requiredQuestIndex;      // Quest index needed to trigger this video
    }

    [Header("Video Configs")]
    [SerializeField] public List<VideoQuestMapping> videoQuestMappings; // Multiple mappings for quest indices
    public GameObject canvasElement;       // Canvas for the video
    public RectTransform videoTransform;   // Zoom Effect
    public float zoomDuration = 0.5f;

    public QuestGiver questGiver;          // Reference to the quest system

    private bool isPlayerNearby = false;
    private bool isPlaying = false;
    private VideoPlayer activeVideoPlayer = null;
    private bool isPaused = false;

    private void Start()
    {
        if (questGiver != null)
        {
            questGiver.OnQuestIndexChanged.AddListener(UpdateVideoForQuestIndex);
        }

        // Ensure all videos are initially inactive
        foreach (var mapping in videoQuestMappings)
        {
            mapping.videoPlayer.SetActive(false);
        }
        canvasElement.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        if (!isPaused && isPlayerNearby && Input.GetKeyDown(KeyCode.E)) // Player presses E to interact
        {
            int currentQuestIndex = questGiver.GetCurrentQuestIndex();
            VideoQuestMapping mapping = FindMappingForQuest(currentQuestIndex);

            if (mapping != null)
            {
                PlayVideo(mapping);
            }
        }

        if (!isPaused && Input.GetKeyDown(KeyCode.BackQuote) && activeVideoPlayer != null && activeVideoPlayer.isPlaying)
        {
            SkipToEnd();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            isPlayerNearby = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            isPlayerNearby = false;
        }
    }

    private void UpdateVideoForQuestIndex(int questIndex)
    {
        foreach (var mapping in videoQuestMappings)
        {
            if (mapping.requiredQuestIndex == questIndex)
            {
                activeVideoPlayer = mapping.videoPlayer.GetComponent<VideoPlayer>();
                mapping.videoPlayer.SetActive(true);
            }
            else
            {
                mapping.videoPlayer.SetActive(false);
            }
        }
    }

    private VideoQuestMapping FindMappingForQuest(int questIndex)
    {
        foreach (var mapping in videoQuestMappings)
        {
            if (mapping.requiredQuestIndex == questIndex)
            {
                return mapping;
            }
        }
        return null;
    }

    private void PlayVideo(VideoQuestMapping mapping)
    {
        if (isPlaying || mapping.videoPlayer == null) return;

        VideoPlayer videoPlayer = mapping.videoPlayer.GetComponent<VideoPlayer>();
        if (videoPlayer == null)
        {
            return;
        }

        videoTransform.gameObject.SetActive(true);
        canvasElement.SetActive(true);
        videoPlayer.Play();
        isPlaying = true;
        activeVideoPlayer = videoPlayer;

        StartCoroutine(ZoomVideo());
        StartCoroutine(WaitForVideoToEnd(videoPlayer));
    }

    private IEnumerator ZoomVideo()
    {
        float elapsed = 0f;
        Vector2 originalSize = videoTransform.sizeDelta;
        Vector2 startSize = originalSize * 0.6f;
        Vector2 endSize = originalSize;

        while (elapsed < zoomDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / zoomDuration;
            videoTransform.sizeDelta = Vector2.Lerp(startSize, endSize, t);
            yield return null;
        }

        videoTransform.sizeDelta = endSize;
    }

    private IEnumerator WaitForVideoToEnd(VideoPlayer videoPlayer)
    {
        yield return new WaitUntil(() => videoPlayer.isPlaying);
        yield return new WaitUntil(() => !videoPlayer.isPlaying);

        videoTransform.gameObject.SetActive(false);
        canvasElement.SetActive(false);
        isPlaying = false;

        if (questGiver != null)
        {
            questGiver.UpdateQuestProgress(GoalType.DoorInteract);
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

    private void SkipToEnd()
    {
        if (activeVideoPlayer == null) return;

        activeVideoPlayer.time = activeVideoPlayer.length - 0.1f;
    }

    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
        if (activeVideoPlayer != null && activeVideoPlayer.isPlaying)
        {
            activeVideoPlayer.Pause();
        }

        // Ensure VideoDoor remains active during pause
        this.enabled = true;
    }

    private void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        if (activeVideoPlayer != null && activeVideoPlayer.isPaused)
        {
            activeVideoPlayer.Play();
        }
    }
}
