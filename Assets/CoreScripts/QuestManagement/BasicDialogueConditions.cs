using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicDialogueConditions : Interactable
{
    // DATA //
    public NPCDialogue[] randomDialogueLines;
    OverlayUIManager overlayManager;


    // OVERRIDES //
    protected override void Interact()
    {
        // If player isn't speaking to an NPC and has random dialogues, displays one of them
        if (!PlayerStats.isRunningDialogue)
        {
            if (randomDialogueLines.Length != 0)
            {
                int dialogueIndex = Random.Range(0, randomDialogueLines.Length-1);
                StartDialogue(randomDialogueLines[dialogueIndex]);
            }
        }
    }


    // FUNCTIONS //
    // Basic Functions
    private void Start()
    {
        overlayManager = FindObjectOfType<OverlayUIManager>();
    }

    // Running Dialogue
    public virtual bool StartDialogue(NPCDialogue dialogue)
    {
        // If given valid dialogue, displays it and returns true
        if (dialogue != null)
        {
            overlayManager.DisplayDialogue(dialogue);
            return true;
        }

        // Otherwise, logs error and returns false
        else
        {
            Debug.LogError("[BasicDialogueConditions] StartDialogue dialogueToDisplay is null!");
            return false;
        }        
    }
}

