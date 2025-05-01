using System.Collections;
using UnityEngine;

public class BackgroundAudio : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private AudioClip hospitalAmbience;

    [SerializeField] public VideoDoor videoDoor;
    [SerializeField] public DoctorInteractable DoctorInteract;

    private bool hasFadedOut = false;
    private bool hasFadedIn = false;
    private float targetVolume = 1f;

    private bool isInDoctorDialogue = false;
    private float dialogueFadeMultiplier = 0.8f; 

    void Start()
    {
        if (backgroundMusic != null && hospitalAmbience != null)
        {
            backgroundMusic.clip = hospitalAmbience;
            backgroundMusic.loop = true;
            backgroundMusic.volume = targetVolume;
            backgroundMusic.Play();
        }
    }

    void Update()
    {
        HandleVideoFade();
        HandleDoctorDialogueFade();
    }

    private void HandleVideoFade()
    {
        if (videoDoor == null) return;

        if (videoDoor.IsVideoPlaying())
        {
            if (!hasFadedOut)
            {
                hasFadedOut = true;
                hasFadedIn = false;
                StartCoroutine(FadeOutBackgroundMusic(1.5f));
            }
        }
        else
        {
            if (!hasFadedIn && hasFadedOut)
            {
                hasFadedIn = true;
                hasFadedOut = false;
                StartCoroutine(FadeInBackgroundMusic(1.5f));
            }
        }
    }

    private void HandleDoctorDialogueFade()
    {
        if (DoctorInteract == null || backgroundMusic == null) return;

        if (DoctorInteract.IsInteracting())
        {
            if (!isInDoctorDialogue)
            {
                isInDoctorDialogue = true;
                StartCoroutine(FadeToVolume(targetVolume * dialogueFadeMultiplier, 1.5f));
            }
        }
        else
        {
            if (isInDoctorDialogue)
            {
                isInDoctorDialogue = false;
                StartCoroutine(FadeToVolume(targetVolume, 1.5f));
            }
        }
    }

    private IEnumerator FadeOutBackgroundMusic(float duration)
    {
        float startVolume = backgroundMusic.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            backgroundMusic.volume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
            yield return null;
        }

        backgroundMusic.volume = 0f;
    }

    private IEnumerator FadeInBackgroundMusic(float duration)
    {
        backgroundMusic.clip = hospitalAmbience;
        backgroundMusic.loop = true;
        backgroundMusic.Play();

        float startVolume = backgroundMusic.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            backgroundMusic.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / duration);
            yield return null;
        }

        backgroundMusic.volume = targetVolume;
    }

    private IEnumerator FadeToVolume(float target, float duration)
    {
        float startVolume = backgroundMusic.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            backgroundMusic.volume = Mathf.Lerp(startVolume, target, elapsed / duration);
            yield return null;
        }

        backgroundMusic.volume = target;
    }
    public void ResumeBackgroundMusic()
    {
        if (!backgroundMusic.isPlaying)
        {
            backgroundMusic.clip = hospitalAmbience;
            backgroundMusic.loop = true;
            backgroundMusic.volume = 0f;
            backgroundMusic.Play();
            StartCoroutine(FadeToVolume(targetVolume, 1.5f)); 
        }
    }
}
