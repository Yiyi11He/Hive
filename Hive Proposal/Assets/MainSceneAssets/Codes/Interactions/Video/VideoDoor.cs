using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoDoor : MonoBehaviour
{
    [System.Serializable]
    public class VideoQuestMapping
    {
        public GameObject videoPlayer;
        public int requiredQuestIndex;
    }

    [Header("Video Configs")]
    [SerializeField] public List<VideoQuestMapping> videoQuestMappings;
    public GameObject canvasElement;
    public RectTransform videoTransform;
    public float zoomDuration = 0.5f;

    [Space]

    public QuestGiver questGiver;
    [SerializeField] private GameObject questUI;

    private bool isPlayerNearby = false;
    private bool isPlaying = false;
    private VideoPlayer activeVideoPlayer = null;
    private bool isPaused = false;

    [Space]

    public PlayerMovement playerController; // Reference to Player Controller
    public MouseLook mouseLookScript;

    private void Start()
    {
        if (questGiver != null)
        {
            questGiver.OnQuestIndexChanged.AddListener(UpdateVideoForQuestIndex);
        }

        foreach (var mapping in videoQuestMappings)
        {
            mapping.videoPlayer.SetActive(false);
        }
        canvasElement.SetActive(false);

        playerController = FindObjectOfType<PlayerMovement>();
        mouseLookScript = FindObjectOfType<MouseLook>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }

        if (!isPaused && isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            int currentQuestIndex = questGiver.GetCurrentQuestIndex();
            VideoQuestMapping mapping = FindMappingForQuest(currentQuestIndex);

            if (mapping != null)
            {
                PlayVideo(mapping);
            }
        }

        /*
        if (!isPaused && Input.GetKeyDown(KeyCode.BackQuote) && activeVideoPlayer != null && activeVideoPlayer.isPlaying)
        {
            SkipToEnd();
        }
        */
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
            else if (mapping.videoPlayer != activeVideoPlayer)
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

        mapping.videoPlayer.SetActive(true);
        VideoPlayer videoPlayer = mapping.videoPlayer.GetComponent<VideoPlayer>();
        if (videoPlayer == null) return;

        videoTransform.gameObject.SetActive(true);
        canvasElement.SetActive(true);

        questUI.SetActive(false);


        videoPlayer.loopPointReached += OnVideoFinished;

        videoPlayer.Play();
        isPlaying = true;
        activeVideoPlayer = videoPlayer;

        DisablePlayerControls();
        StartCoroutine(ZoomVideo());
    }


    private void OnVideoFinished(VideoPlayer source)
    {
        source.loopPointReached -= OnVideoFinished;

        videoTransform.gameObject.SetActive(false);
        canvasElement.SetActive(false);
        isPlaying = false;

        EnablePlayerControls();
        questUI.SetActive(true);

        if (questGiver != null)
        {
            questGiver.UpdateQuestProgress(GoalType.DoorInteract);
        }
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

        EnablePlayerControls();

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

    private void DisablePlayerControls()
    {
        if (playerController != null)
            playerController.enabled = false; 

        if (mouseLookScript != null)
            mouseLookScript.enabled = false;
        Cursor.lockState = CursorLockMode.None;

    }

    private void EnablePlayerControls()
    {
        if (playerController != null)
            playerController.enabled = true; 

        if (mouseLookScript != null)
            mouseLookScript.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public bool IsVideoPlaying()
    {
        return isPlaying;
    }
}
