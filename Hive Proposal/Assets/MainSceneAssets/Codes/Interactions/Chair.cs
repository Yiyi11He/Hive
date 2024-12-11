using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : MonoBehaviour
{
    public GameObject playerStanding, playerSitting, intText, standText, switchView;
    public bool sitting;
    public QuestGiver questGiver;

    private bool interactable = false;

    //player actions trigger quest goals
    //questGiver.UpdateQuestProgress(GoalType.ChairInteract);

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            intText.SetActive(true);
            interactable = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            intText.SetActive(false);
            interactable = false;
        }
    }

    void Update()
    {
        if (interactable == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                SitDown();
            }
        }

        if (sitting == true)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                StandUp();
            }
        }
    }

    private void SitDown()
    {
        intText.SetActive(false);
        standText.SetActive(true);
        playerSitting.SetActive(true);
        sitting = true;
        playerStanding.SetActive(false);
        interactable = false;
        switchView.SetActive(true);

        // Trigger quest progress if the QuestGiver is available
        if (questGiver != null)
        {
            Debug.Log($"Current Quest Index: {questGiver.GetCurrentQuestIndex()}");
            questGiver.UpdateQuestProgress(GoalType.ChairInteract);
        }
    }

    private void StandUp()
    {
        playerSitting.SetActive(false);
        standText.SetActive(false);
        playerStanding.SetActive(true);
        sitting = false;
        switchView.SetActive(false);
    }
}