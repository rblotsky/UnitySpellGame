using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicQuestNPCInteractable : Interactable
{
    // STORED DATA //
    [Header("Quest NPC Data")]
    public string questUpdateEventName;
    public NPCDialogue[] fallbackDialogues;
    public bool multipleQuestUpdates = false;


    // OVERRIDES //
    protected override void Interact()
    {
        // Only runs dialogue if player isn't already speaking to an NPC
        if (!PlayerStats.isRunningDialogue)
        {
            // Updates quest stages. If a transition dialogue exists, it is run from the transition itself and causes fallback not to play because it starts first.
            if(!Quests.UpdateStages(questUpdateEventName, multipleQuestUpdates, gameObject))
            {
                Quests.RunDialogueRequest(questUpdateEventName, gameObject);
            }

            // If a quest transition dialogue is not already playing, runs fallback
            if (!PlayerStats.isRunningDialogue)
            {
                RunFallbackDialogue();
            }
        }
    }


    // FUNCTIONS //
    protected virtual void RunFallbackDialogue()
    {
        // If there are no fallback dialogues, does nothing.
        if(fallbackDialogues.Length == 0)
        {
            return;
        }

        // Displays a random index in the array of fallback dialogues
        int dialogueIndex = Random.Range(0, fallbackDialogues.Length);

        if (fallbackDialogues[dialogueIndex] != null)
        {
            DataRef.overlayManagerReference.DisplayDialogue(fallbackDialogues[dialogueIndex], gameObject);
        }
    }
}