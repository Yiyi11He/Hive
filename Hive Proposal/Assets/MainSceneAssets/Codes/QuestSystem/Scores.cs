using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using TMPro;

public class Scores : MonoBehaviour
{
    private InMemoryVariableStorage variableStorage;
    public TMP_Text scoreText;

    void Start()
    {
        variableStorage = FindObjectOfType<InMemoryVariableStorage>();



        UpdateScoreDisplay();

        // Register command handler for "add_score"
        FindObjectOfType<DialogueRunner>().AddCommandHandler<float>("add_score", AddPlayerScore);
    }

    [YarnCommand ("add_score")]
    public void AddPlayerScore(float amount)
    {
        Debug.Log("Adding Current Score");
        float currentScore = 0f;
        variableStorage.TryGetValue("$player_score", out currentScore);
        currentScore += amount;
        variableStorage.SetValue("$player_score", currentScore);
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        float currentScore = 0f;
        variableStorage.TryGetValue("$player_score", out currentScore);
        scoreText.text = $"Score: {currentScore}";
    }
}
