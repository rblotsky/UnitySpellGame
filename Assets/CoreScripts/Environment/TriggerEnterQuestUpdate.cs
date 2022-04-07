using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(DataPersistentObject))]
public class TriggerEnterQuestUpdate : MonoBehaviour, IDataPersistentComponent
{
    // DATA //
    // Basic Data
    public string questUpdateName;
    public string requiredColliderTag = "Player";
    public bool runMultipleUpdates = true;
    public bool multipleTriggers = true;
    public bool displayDialogueAtLocation = false;
    public UnityEvent onTriggerEvent;

    // Cached Data
    private bool hasTriggered = false;


    // FUNCTIONS //
    // Basic Functions
    private void OnTriggerEnter(Collider other)
    {
        // If the object has the right tag, runs the trigger
        if (other.CompareTag(requiredColliderTag))
        {
            if (!hasTriggered || multipleTriggers)
            {
                // Runs the stage update and dialogue as well as the on trigger event
                if (displayDialogueAtLocation)
                {
                    Quests.UpdateStages(questUpdateName, runMultipleUpdates, gameObject);
                }

                else
                {
                    Quests.UpdateStages(questUpdateName, runMultipleUpdates);
                }

                if(onTriggerEvent != null)
                {
                    onTriggerEvent.Invoke();
                }

                // Caches that it has triggered
                hasTriggered = true;
            }
        }
    }


    // Interface Funtions
    public string SaveDataToString()
    {
        return hasTriggered.ToString();
    }

    public void LoadDataFromString(string data)
    {
        hasTriggered = false;
        bool.TryParse(data, out hasTriggered);
    }
}
