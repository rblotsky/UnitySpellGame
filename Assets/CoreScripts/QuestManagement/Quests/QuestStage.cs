using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Quest Stage", menuName = "Quests/Quest Stage")]
[System.Serializable]
public class QuestStage : GameResource
{
    // STAGE DATA //
    // Basic Data
    [Header("Basic Data")]
    public string transitionToThisStage;
    public CompletionStatus completionStatus;
    public bool isFinishStage;
    [HideInInspector]
    public Quest questReference;

    // Transitions and Requests
    [Tooltip("Stage transitions are run on the current stage to transition to the given stage.")]
    public QuestStageTransition[] stageTransitions;
    [Tooltip("Dialogue requests are run on the current stage for repeated dialogues (The dialogue will run every time the request is run).")]
    public DialogueRequestListener[] dialogueRequestListeners;
    [Tooltip("Map location requests are run for the stages that the current stage will transition to.")]
    public MapLocationDataRequestListener[] mapLocationRequestListeners;


    // FUNCTIONS //
    // Basic Stage Functions
    public QuestStage RunStageTransition(string questUpdateEvent, GameObject context)
    {
        // Checks each transition to see if requirements are met.
        // Updates completion status of this stage accordingly.
        // Returns which stage should become the active stage.

        foreach (QuestStageTransition transition in stageTransitions)
        {
            // If the transition requirements are met, runs the transition.
            if (transition.CheckTransitionRequirements(questUpdateEvent))
            {
                transition.RunTransitionEvents(context);
                return transition.newStage;
            }
        }

        // Returns this stage if no transitions are run.
        return this;
    }


    // Running requests
    public bool CheckDialogueRequests(string requestValue, GameObject context)
    {
        // Searches through its dialogue requests.
        // Runs the first one that works, then stops.
        // Return value: true = ran a dialogue, false = didn't run a dialogue

        foreach (DialogueRequestListener requestListener in dialogueRequestListeners)
        {
            // If the dialogue runs, stops and returns true.
            if (requestListener.RequestDialogue(requestValue, context))
            {
                return true;
            }
        }

        // Returns false if no dialogues are run.
        return false;
    }

    public void MapLocationDataRequest(string requestValue, MapLocationQuestDataContainer locationDataContainer, QuestStage thisStageReference)
    {
        // Runs all map location request listeners, and adds data to container
        foreach(MapLocationDataRequestListener requestListener in mapLocationRequestListeners)
        {
            requestListener.RequestLocationInfo(requestValue, locationDataContainer, this);
        }
    }

    public void SetupStage(Quest assignedQuest)
    {
        // Sets the quest reference
        questReference = assignedQuest;
    }

}
