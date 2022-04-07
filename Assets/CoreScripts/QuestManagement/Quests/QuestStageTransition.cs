using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestStageTransition
{
    // TRANSITION DATA //
    // Basic Data
    [Header("Transition Data/Requirements")]
    public string requiredEvent;
    public QuestStage newStage;
    public bool displayToPlayer;
    public RequirementList transitionRequirements;

    // Transition Events
    [Header("Transition Events")]
    public MapLocationUpdate[] locationUpdates = new MapLocationUpdate[0];
    public MerchantTradeUpdate[] tradeUpdates = new MerchantTradeUpdate[0];
    public NPCDialogue transitionDialogue;


    // FUNCTIONS //
    public bool CheckTransitionRequirements(string questUpdateEvent)
    {
        // Checks requirements, but only if the quest update event is equal to the required event
        if (requiredEvent.Equals(questUpdateEvent))
        {
            // Checks if requirements are met
            return transitionRequirements.CheckRequirements();
        }

        // Returns false if quest update event is the wrong one
        return false;
    }

    public void RunTransitionEvents(GameObject context)
    {
        // Runs location updates
        foreach(MapLocationUpdate locationUpdateEvent in locationUpdates)
        {
            locationUpdateEvent.RunLocationUpdate();
        }

        // Runs merchant updates
        foreach(MerchantTradeUpdate tradeUpdateEvent in tradeUpdates)
        {
            tradeUpdateEvent.RunTradeUpdate();
        }

        // Runs transition dialogue if there is a dialogue manager
        if(DataRef.overlayManagerReference != null)
        {
            if (transitionDialogue != null)
            {
                DataRef.overlayManagerReference.DisplayDialogue(transitionDialogue, context);
            }
        }
    }
}
