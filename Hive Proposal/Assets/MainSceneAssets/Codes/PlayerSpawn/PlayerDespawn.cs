using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SceneOutroMapping
{
    public int requiredQuestIndex;
    public GameObject uiAnimationFolder;
}

public class PlayerDespawn : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject[] uiElementsToDisable;

    [Header("Door Settings")]
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private string doorTriggerName = "DoorAnim";

    [Header("Scene Outro Mappings")]
    [SerializeField] private List<SceneOutroMapping> sceneOutroMappings;

    [Header("Player Reset Settings")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform resetPosition;
    [SerializeField] private float resetDelaySeconds = 5.2f;

    [Header("Quest Integration")]
    [SerializeField] private QuestGiver questGiver;

    public void HandleInteraction()
    {
        int currentQuestIndex = questGiver.GetCurrentQuestIndex();

        foreach (var mapping in sceneOutroMappings)
        {
            if (mapping.requiredQuestIndex == currentQuestIndex)
            {
                Debug.Log($"[PlayerDespawnHandler] Matched quest {currentQuestIndex}. Handling interaction...");
                StartCoroutine(ProcessDespawnSequence(mapping));
                StartCoroutine(CompleteQuestAfterAnimation(resetDelaySeconds));
                return;
            }
        }

        Debug.LogWarning($"[PlayerDespawnHandler] No matching scene outro for quest {currentQuestIndex}.");
    }
    private IEnumerator CompleteQuestAfterAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);
        questGiver.QuestComplete();
    }
    private IEnumerator ProcessDespawnSequence(SceneOutroMapping mapping)
    {
        questGiver.HideQuestUI();
        foreach (GameObject uiElement in uiElementsToDisable)
        {
            if (uiElement != null)
                uiElement.SetActive(false);
        }

        if (mapping.uiAnimationFolder != null)
        {
            mapping.uiAnimationFolder.SetActive(true);
        }

        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger(doorTriggerName);
            Debug.Log("Door animation triggered.");
        }

        yield return new WaitForSeconds(resetDelaySeconds);

        if (playerTransform != null && resetPosition != null)
        {
            playerTransform.position = resetPosition.position;
            playerTransform.rotation = resetPosition.rotation;
            Debug.Log($"Player reset to position: {resetPosition.position}");
        }
        foreach (GameObject uiElement in uiElementsToDisable)
        {
            uiElement.SetActive(true);
        }
    }
}
