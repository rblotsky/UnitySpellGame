using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueBase : ScriptableObject
{
    // Base class for all types of dialogue
    // DATA //
    public string speakingNPC;
    [TextArea(1,6)]
    public string[] lines;
    [Space]
    [Header("On Dialogue Finish")]
    public string onFinishQuestUpdate;
    public UnityEvent OnDialogueFinish;

    
    // FUNCTIONS //
    public virtual void RunDialogueFinish()
    {
        Quests.UpdateStages(onFinishQuestUpdate, true);
        OnDialogueFinish.Invoke();
    }
}
