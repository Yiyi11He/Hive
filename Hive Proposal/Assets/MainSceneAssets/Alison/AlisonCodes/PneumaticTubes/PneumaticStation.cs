using System.Collections.Generic;
using UnityEngine;

public class PneumaticStation : MonoBehaviour
{
    [Header("References")]
    public PneumaticTube pneumaticTubeManager; 
    public QuestGiver questGiver;
    public ParticleSystem confettiParticleSystem;

    [Space]
    public AudioSource VFXConfetti;
    public AudioClip Confetti;

    [Header("Quest Requirements")]
    public List<int> requiredQuestIndices;

    public void OnInteraction()
    {
        int currentQuestIndex = questGiver.GetCurrentQuestIndex();

        // Check if the current quest index is in the required list and the player has the tube
        if (requiredQuestIndices.Contains(currentQuestIndex) && pneumaticTubeManager.HasTube)
        {
            CompleteQuest();
        }
    }

    private void CompleteQuest()
    {
        if (confettiParticleSystem != null)
        {
            confettiParticleSystem.Play();
            VFXConfetti.PlayOneShot(Confetti);
        }

        // Use the pneumatic tube
        pneumaticTubeManager.UseTube();

        // Complete the current quest
        questGiver.QuestComplete();
    }
}
