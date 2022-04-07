using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueRequestListener
{
    // When given a valid Dialogue request, output dialogue is run, if it has already run, uses repeat dialogue.

    // STORED DATA //
    // Basic Data
    public string requestName;
    public NPCDialogue outputDialogue;


    // FUNCTIONS //
    public bool RequestDialogue(string dialogueRequest, GameObject context)
    {
        // If the request is valid, displays dialogue and returns true.
        if (dialogueRequest.Equals(requestName))
        {
            // Only displays dialogue if overlaymanager isn't null
            if (DataRef.overlayManagerReference != null)
            {
                DataRef.overlayManagerReference.DisplayDialogue(outputDialogue);
            }

            // Otherwise logs an error
            else
            {
                Debug.LogError("[DialogueRequestListener] SceneDataRef has no overlayManagerReference to display dialogue!");
            }

            return true;
        }

        // If request is invalid or no dialogue to say, returns false.
        return false;
    }
}

