using UnityEngine;
using Yarn.Unity;
using System;

public class AnswerEvaluator : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Reference to Yarn's in-memory variable storage")]
    public InMemoryVariableStorage variableStorage;

    public void EvaluateXrayAnswer(string userInput)
    {
        Debug.Log($"[AnswerEvaluator] Player input: {userInput}");

        string lower = userInput.ToLowerInvariant();

        int xrayScore = 0;

        bool hasGas = lower.Contains("gas");
        bool hasUlcer = lower.Contains("ulcer");
        bool hasOsteo = lower.Contains("osteomyelitis");

        if (hasGas)
            xrayScore++;

        if (hasUlcer)
            xrayScore++;

        if (hasOsteo)
            xrayScore++;

        variableStorage.SetValue("$xrayScore", xrayScore);

        Debug.Log($"[AnswerEvaluator] Xray answer -> xrayScore = {xrayScore}");
    }


    public void EvaluateCultureAnswer(string userInput)
    {
        // Convert to lower for case-insensitive checks
        string lower = userInput.ToLowerInvariant();
        int cultureScore = 0;

        // Check for relevant keywords
        bool hasEColi = lower.Contains("e coli") || lower.Contains("escherichia coli");
        bool hasMixed = lower.Contains("mixed");
        bool hasBacteroides = lower.Contains("bacteroides fragilis");
        bool hasClostridium = lower.Contains("clostridium perfringens");
        bool hasPseudomonas = lower.Contains("pseudomonas aeruginosa");

        // Increment the score for each keyword found:
        if (hasMixed) cultureScore++;
        if (hasEColi) cultureScore++;
        if (hasBacteroides) cultureScore++;
        if (hasClostridium) cultureScore++;
        if (hasPseudomonas) cultureScore++;

        // Now store that integer in Yarn
        variableStorage.SetValue("$cultureScore", cultureScore);

        Debug.Log($"[AnswerEvaluator] Culture answer -> cultureScore = {cultureScore}");
    }

    public bool IsFuzzyMatch(string userInput, string target, int maxEdits = 2)
    {
        int dist = LevenshteinDistance(
            userInput.ToLowerInvariant(),
            target.ToLowerInvariant()
        );
        return dist <= maxEdits;
    }
    public int LevenshteinDistance(string a, string b)
    {
        if (string.IsNullOrEmpty(a)) return string.IsNullOrEmpty(b) ? 0 : b.Length;
        if (string.IsNullOrEmpty(b)) return a.Length;

        int lengthA = a.Length;
        int lengthB = b.Length;
        int[,] distance = new int[lengthA + 1, lengthB + 1];

        for (int i = 0; i <= lengthA; i++)
            distance[i, 0] = i;

        for (int j = 0; j <= lengthB; j++)
            distance[0, j] = j;

        for (int i = 1; i <= lengthA; i++)
        {
            for (int j = 1; j <= lengthB; j++)
            {
                int cost = (b[j - 1] == a[i - 1]) ? 0 : 1;
                distance[i, j] = Math.Min(
                    Math.Min(
                        distance[i - 1, j] + 1,    // deletion
                        distance[i, j - 1] + 1),   // insertion
                    distance[i - 1, j - 1] + cost // substitution
                );
            }
        }

        return distance[lengthA, lengthB];
    }
}
