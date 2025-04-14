using System.Collections;
using UnityEngine;

public class BackgroundAudio : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private AudioClip hospitalAmbience;

    [SerializeField] public VideoDoor videoDoor;

    private bool hasFadedOut = false;
    private bool hasFadedIn = false;
    private float targetVolume = 1f;

    //reference doctor too! And turn the audio for it down when talking to doctor

    void Start()
    {
        // Assign the clip and start playing
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
        if (videoDoor != null)
        {
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
        backgroundMusic.Stop();
    }

    private IEnumerator FadeInBackgroundMusic(float duration)
    {
        backgroundMusic.clip = hospitalAmbience;  // Re-assign the clip just in case
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
}
