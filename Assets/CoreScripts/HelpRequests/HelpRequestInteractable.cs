using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpRequestInteractable : Interactable
{
    // DATA //
    // References
    public HelpRequestUIManager requestUI;
    public Quest[] possibleQuests;

    // Data
    public string returnItemsUpdateEvent;

    // Cached Data
    private Quest selectedQuest;


    // OVERRIDES //
    protected override void Interact()
    {
        if(!Quests.UpdateStages(returnItemsUpdateEvent, true, gameObject))
        {
            if(DataRef.sceneMenuManagerReference != null)
            {
                DataRef.sceneMenuManagerReference.OpenNewUIInstance(requestUI);
            }

            requestUI.SetupUIManager(selectedQuest, gameObject);
        }
    }

    protected override void Awake()
    {
        // First checks if any of them are currently started, does nothing if they are
        bool isPlayingRequest = false;

        foreach(Quest quest in possibleQuests)
        {
            if(quest.QuestCompletion == CompletionStatus.Started)
            {
                isPlayingRequest = true;
                selectedQuest = quest;
                break;
            }
        }

        if (!isPlayingRequest)
        {
            // Selects a random quest from possibleQuests to give as a request, and resets it.
            int selectedIndex = Random.Range(0, possibleQuests.Length);
            selectedQuest = possibleQuests[selectedIndex];
            selectedQuest.ResetQuest();
        }

        // Runs base awake
        base.Awake();
    }
}
