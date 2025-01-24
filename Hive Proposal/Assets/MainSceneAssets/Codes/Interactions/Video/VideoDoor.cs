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


    private void Start()
    {
        if (questGiver != null)
        {
            Debug.Log("Subscribing to QuestGiver OnQuestIndexChanged.");
            questGiver.OnQuestIndexChanged.AddListener(OnQuestIndexChanged);
        }
        else
        {
            Debug.LogError("QuestGiver reference is missing in VideoDoor.");
        }
    }

    private void OnQuestIndexChanged(int questIndex)
    {
        Debug.Log($"VideoDoor received quest index update: {questIndex}");
        // Perform any additional logic here if necessary
    }

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E)) // Player presses E to interact
        {
            int currentQuestIndex = questGiver.GetCurrentQuestIndex();
            VideoQuestMapping mapping = FindMappingForQuest(currentQuestIndex);

            if (mapping != null)
            {
                Debug.Log($"Found matching mapping for quest index {currentQuestIndex}. Playing video...");
                PlayVideo(mapping);
            }
            else
            {
                Debug.LogError("Could not find video mapping");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TESTING");

        if (other.CompareTag("MainCamera"))
        {
            Debug.Log("Video Door Entered");
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            Debug.Log("Video Door Exited");
            isPlayerNearby = false;
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
        VideoPlayer videoPlayer = mapping.videoPlayer.GetComponent<VideoPlayer>();

        if (videoPlayer != null)
        {
            Debug.Log("Activating video player and canvas...");
            videoPlayer.gameObject.SetActive(true);
            canvasElement.SetActive(true);
            videoPlayer.Play();
            isPlaying = true;

            StartCoroutine(ZoomVideo());
            StartCoroutine(WaitForVideoToEnd(videoPlayer));
        }
        else
        {
            Debug.LogWarning("VideoPlayer component not found!");
        }
    }

    private IEnumerator ZoomVideo()
    {
        float elapsed = 0f;
        Vector2 originalSize = videoTransform.sizeDelta;

        Vector2 startSize = originalSize * 0.6f;
        Vector2 endSize = originalSize * 1.0f;

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
        Debug.Log("Waiting for video to finish...");
        yield return new WaitUntil(() => !videoPlayer.isPlaying);

        Debug.Log("Video finished playing. Disabling video player and canvas.");
        videoPlayer.gameObject.SetActive(false);
        canvasElement.SetActive(false);
        isPlaying = false;

        if (questGiver != null)
        {
            questGiver.QuestComplete();
        }
    }
}
