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

    void Start()
    {
        variableStorage = FindObjectOfType<InMemoryVariableStorage>();
        UpdateScoreDisplay();
    }

    [YarnCommand ("add_score")]
    public void AddPlayerScore(int scoreToAdd)
    {
        Score += scoreToAdd;
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        scoreText.text = $"Score: {Score}";
    }
}
