using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoDoor : MonoBehaviour
{
    public bool IsOpen = false;

    [SerializeField] private bool IsRotatingDoor = true;
    [SerializeField] private float speed = 1.0f;

    [Header("Rotation Configs")]
    [SerializeField] private float RotationAmount = 90f;
    [SerializeField] private float ForwardDirection = 0;

    private Vector3 StartRotation;
    private Vector3 Forward;
    private Coroutine AnimationCoroutine;

    [System.Serializable]
    public class VideoQuestMapping
    {
        public GameObject videoPlayer;      // The Video Player GameObject
        public int requiredQuestIndex;      // Quest index needed to trigger this video
    }

    [Header("Video Configs")]
    [SerializeField] public List<VideoQuestMapping> videoQuestMappings; // Multiple mappings for quest indices
    public GameObject canvasElement;       // Canvas for the video
    public RectTransform videoTransform;   // RectTransform for zoom effect
    public float zoomDuration = 0.5f;

    public QuestGiver questGiver;          // Reference to the quest system

    private bool isPlayerNearby = false;
    private bool isPlaying = false;

    private void Awake()
    {
        StartRotation = transform.rotation.eulerAngles;
        Forward = transform.forward;
    }

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            // Always open the door
            Interact();

            // Check for video playback
            int currentQuestIndex = questGiver.GetCurrentQuestIndex();
            VideoQuestMapping mapping = FindMappingForQuest(currentQuestIndex);

            if (mapping != null)
            {
                Debug.Log($"Found matching mapping for quest index {currentQuestIndex}. Playing video...");
                PlayVideo(mapping);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
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

    public void Interact()
    {
        if (IsOpen)
        {
            Close();
        }
        else
        {
            Open(transform.position);
        }
    }

    public void Open(Vector3 userPosition)
    {
        if (!IsOpen)
        {
            if (AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);
            }

            if (IsRotatingDoor)
            {
                float dot = Vector3.Dot(Forward, (userPosition - transform.position).normalized);
                AnimationCoroutine = StartCoroutine(DoRotationOpen(dot));
            }
        }
    }

    private IEnumerator DoRotationOpen(float forwardAmount)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation;

        if (forwardAmount >= ForwardDirection)
        {
            endRotation = Quaternion.Euler(new Vector3(0, StartRotation.y + RotationAmount, 0));
        }
        else
        {
            endRotation = Quaternion.Euler(new Vector3(0, StartRotation.y - RotationAmount, 0));
        }

        IsOpen = true;

        float time = 0;
        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * speed;
        }

        transform.rotation = endRotation;
    }

    public void Close()
    {
        if (IsOpen)
        {
            if (AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);
            }

            if (IsRotatingDoor)
            {
                AnimationCoroutine = StartCoroutine(DoRotationClose());
            }
        }
    }

    private IEnumerator DoRotationClose()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(StartRotation);
        IsOpen = false;

        float time = 0;
        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * speed;
        }

        transform.rotation = endRotation;
    }
}
