using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest Update Trigger", menuName = "Quests/Quest Update Trigger")]
public class QuestUpdateTrigger : ScriptableObject
{
    // UPDATE DATA //
    public string updateEventName;
    public bool runMultipleUpdates;


    // FUNCTIONS //
    public void UpdateEvent()
    {
        Quests.UpdateStages(updateEventName, runMultipleUpdates);
    }
}
