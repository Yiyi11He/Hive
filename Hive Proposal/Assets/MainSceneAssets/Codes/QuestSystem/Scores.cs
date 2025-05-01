using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using TMPro;

public class Scores : MonoBehaviour
{
    public int Score { get; private set; }

    private InMemoryVariableStorage variableStorage;
    public TMP_Text scoreText;

    public TMP_Text scorePopupText;
    public Animator scoreAnimator;

    [Space]

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip positiveSound;
    public AudioClip negativeSound;


    void Start()
    {
        variableStorage = FindObjectOfType<InMemoryVariableStorage>();
        UpdateScoreDisplay();
    }

    [YarnCommand("add_score")]
    public void AddPlayerScore(int scoreToAdd)
    {
        if (scoreToAdd == 0)
            return;

        StartCoroutine(AnimateScoreAddition(scoreToAdd));
    }


    private void UpdateScoreDisplay()
    {
        scoreText.text = $"Score: {Score}";
    }
    private IEnumerator AnimateScoreAddition(int scoreToAdd)
    {
        scorePopupText.text = $"{(scoreToAdd >= 0 ? "+" : "-")} {Mathf.Abs(scoreToAdd)}";
        scorePopupText.gameObject.SetActive(true);

        if (audioSource != null)
        {
            AudioClip clipToPlay = scoreToAdd >= 0 ? positiveSound : negativeSound;
            if (clipToPlay != null)
                audioSource.PlayOneShot(clipToPlay);
        }

        // Play animation
        scoreAnimator.ResetTrigger("AddScore");
        scoreAnimator.SetTrigger("AddScore");

        yield return new WaitForSeconds(1f); 

        scoreAnimator.Play("ScoresIdle", 0, 0f);
        scorePopupText.gameObject.SetActive(false);

        Score += scoreToAdd;
        UpdateScoreDisplay();
    }



}
