using UnityEngine;
using Yarn.Unity;
using TMPro;
using System.Collections.Generic;

public class Highlight : MonoBehaviour
{
    public OptionsListView optionsListView;
    public Color highlightColor = Color.green;
    public Color defaultColor = Color.white;

    // We'll store any "active/toggled" keywords in here.
    private HashSet<string> toggledKeywords = new HashSet<string>();

    void Start()
    {
        // (Optionally locate a DialogueRunner if you need it, but we won’t rely on Yarn variables.)
    }

    [YarnCommand("highlight")]
    public void HighlightOption(string keyword)
    {
        if (optionsListView == null)
        {
            Debug.LogWarning("OptionsListView is null.");
            return;
        }

        // Toggle the keyword on/off
        var lowerKeyword = keyword.ToLower();
        if (toggledKeywords.Contains(lowerKeyword))
        {
            // If already in the set, remove it => turning it off
            toggledKeywords.Remove(lowerKeyword);
            Debug.Log($"[Highlight] Removed keyword '{keyword}' from toggled set. Now off.");
        }
        else
        {
            // Otherwise, add it => turning it on
            toggledKeywords.Add(lowerKeyword);
            Debug.Log($"[Highlight] Added keyword '{keyword}' to toggled set. Now on.");
        }

        // Now refresh the highlight states of all labels
        RefreshAllLabels();
    }

    // Applies the highlight color to any label that contains any active keyword
    private void RefreshAllLabels()
    {
        if (optionsListView == null)
            return;

        foreach (var option in optionsListView.OptionViews)
        {
            var label = option.GetComponentInChildren<TMP_Text>();
            if (label == null)
                continue;

            // Check if this label contains ANY of the toggled keywords
            bool shouldHighlight = false;
            string labelTextLower = label.text.ToLower();

            foreach (var kw in toggledKeywords)
            {
                if (labelTextLower.Contains(kw))
                {
                    shouldHighlight = true;
                    break;
                }
            }

            // Assign color based on whether at least one keyword matched
            label.color = shouldHighlight ? highlightColor : defaultColor;
        }
    }

    [YarnCommand("clear_highlights")]
    public void ClearAllHighlights()
    {
        toggledKeywords.Clear(); // forget all toggled keywords

        if (optionsListView == null)
        {
            Debug.LogWarning("OptionsListView not assigned.");
            return;
        }

        // Reset color to default
        foreach (var option in optionsListView.OptionViews)
        {
            var label = option.GetComponentInChildren<TMP_Text>();
            if (label != null)
            {
                label.color = defaultColor;
            }
        }

        Debug.Log("Cleared all toggled keywords and reset highlights.");
    }
}
