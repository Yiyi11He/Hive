using UnityEngine;
using TMPro;
using System.Collections;

public class ResultsUI : MonoBehaviour
{
    [Header("Score Texts")]
    [SerializeField] private TMP_Text preScoreText;
    [SerializeField] private TMP_Text mainScoreText;
    [SerializeField] private TMP_Text postScoreText;
    [SerializeField] private TMP_Text totalScoreText;

    [Header("Time Texts")]
    [SerializeField] private TMP_Text preTimeText;
    [SerializeField] private TMP_Text mainTimeText;
    [SerializeField] private TMP_Text postTimeText;
    [SerializeField] private TMP_Text totalTimeText;

    [Header("Animation Settings")]
    [SerializeField] private float countUpDuration = 1.5f;
    [SerializeField] private float delayBetweenCategories = 0.5f;
    [SerializeField] private AudioClip countUpSFX;
    [SerializeField] private AudioClip drumrollSFX;
    [SerializeField] private float drumrollDuration = 4f;

    [SerializeField] private AudioSource audioSource;



    private void Start()
    {
        StartCoroutine(WaitForResultsManagerThenStart());
    }

    private IEnumerator WaitForResultsManagerThenStart()
    {
        // Wait until ResultsManager.Instance is not null
        while (ResultsManager.Instance == null)
            yield return null;

        StartCoroutine(AnimateAllResults());
    }

    private IEnumerator AnimateAllResults()
    {
        var results = ResultsManager.Instance;
        float normalDuration = countUpDuration;

        // Pre Quiz
        ClearTexts(preScoreText, preTimeText);
        PlaySFX(); yield return StartCoroutine(CountUpInt(preScoreText, results.GetPreQuizScore(), normalDuration));
        PlaySFX(); yield return StartCoroutine(CountUpTime(preTimeText, results.GetPreQuizTime(), normalDuration));
        yield return new WaitForSeconds(delayBetweenCategories);

        // Main Quiz
        ClearTexts(mainScoreText, mainTimeText);
        PlaySFX(); yield return StartCoroutine(CountUpInt(mainScoreText, results.GetMainQuizScore(), normalDuration));
        PlaySFX(); yield return StartCoroutine(CountUpTime(mainTimeText, results.GetMainQuizTime(), normalDuration));
        yield return new WaitForSeconds(delayBetweenCategories);

        // Post Quiz
        ClearTexts(postScoreText, postTimeText);
        PlaySFX(); yield return StartCoroutine(CountUpInt(postScoreText, results.GetPostQuizScore(), normalDuration));
        PlaySFX(); yield return StartCoroutine(CountUpTime(postTimeText, results.GetPostQuizTime(), normalDuration));
        yield return new WaitForSeconds(delayBetweenCategories);

        // Total with Drumroll
        ClearTexts(totalScoreText, totalTimeText);

        StartDrumroll();
        yield return StartCoroutine(CountUpInt(totalScoreText, results.GetTotalScore(), drumrollDuration));
        StopDrumroll();

        yield return new WaitForSeconds(delayBetweenCategories);

        StartDrumroll();
        yield return StartCoroutine(CountUpTime(totalTimeText, results.GetTotalTime(), drumrollDuration));
        StopDrumroll();
    }



    private void ClearTexts(TMP_Text scoreText, TMP_Text timeText)
    {
        if (scoreText != null) scoreText.text = "";
        if (timeText != null) timeText.text = "";
    }

    private IEnumerator CountUpInt(TMP_Text textElement, int targetValue, float duration)
    {
        float elapsed = 0f;
        int lastDisplayed = -1;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            int displayed = Mathf.RoundToInt(Mathf.Lerp(0, targetValue, t));

            if (displayed != lastDisplayed)
            {
                textElement.text = displayed.ToString();
                PlaySFX();
                lastDisplayed = displayed;
            }

            yield return null;
        }

        textElement.text = targetValue.ToString();
    }



    private IEnumerator CountUpTime(TMP_Text textElement, float targetTime, float duration)
    {
        float elapsed = 0f;
        int lastDisplayedSeconds = -1;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float displayed = Mathf.Lerp(0f, targetTime, t);

            int secondsNow = Mathf.FloorToInt(displayed);
            if (secondsNow != lastDisplayedSeconds)
            {
                textElement.text = FormatTime(displayed);
                PlaySFX();
                lastDisplayedSeconds = secondsNow;
            }

            yield return null;
        }

        textElement.text = FormatTime(targetTime);
    }
    private void StartDrumroll()
    {
        if (audioSource != null && drumrollSFX != null)
        {
            audioSource.clip = drumrollSFX;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    private void StopDrumroll()
    {
        if (audioSource != null && audioSource.clip == drumrollSFX)
        {
            audioSource.Stop();
            audioSource.loop = false;
            audioSource.clip = null;
        }
    }


    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return $"{minutes:00}:{seconds:00}";
    }

    private void PlaySFX()
    {
        if (audioSource != null && countUpSFX != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(countUpSFX);
        }
    }
}
