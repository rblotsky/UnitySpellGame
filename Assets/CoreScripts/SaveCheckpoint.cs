using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveCheckpoint : MonoBehaviour
{
    // DATA //
    // References
    public GameDataManager dataManager;
    public GameObject[] onUseDisabledObjects;

    // Cached Data
    private bool hasBeenUsed = false;


    // FUNCTIONS //
    // Basic Functions
    private void OnTriggerEnter(Collider other)
    {
        // Saves game if player enters
        if (other.CompareTag("Player"))
        {
            if (!hasBeenUsed && !PlayerStats.isPlayerPaused && !PlayerStats.isRunningDialogue)
            {
                dataManager.SaveGameProgress(DataRef.baseData.saveName, SceneManager.GetActiveScene().name);

                // Disables extra components
                foreach(GameObject obj in onUseDisabledObjects)
                {
                    obj.SetActive(false);
                }
            }
        }
    }
}
