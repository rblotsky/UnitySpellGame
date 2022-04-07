using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    // DATA //
    // References
    public GameDataManager dataManager;
    private ScenePersistentObjectManager persistentObjectManager;

    // Static Data
    public static TravelSceneDialogue currentTravelDialogue;
    public static string currentNewScene;


    // FUNCTIONS //
    // Basic Functions
    private void Start()
    {
        // Gets some scene references
        persistentObjectManager = FindObjectOfType<ScenePersistentObjectManager>();

        // Sets StaticDataRef transition manager to this
        DataRef.transitionManagerReference = this;
    }


    // External Functions
    public static void ResetCachedStaticData()
    {
        currentTravelDialogue = null;
        currentNewScene = null;
    }

    public void BasicSceneTransition(string newScene)
    {
        // Saves game data for scene change
        SaveSceneBasedData(newScene);

        // Resets timescale to 1
        Time.timeScale = 1;

        // Loads new scene
        SceneManager.LoadScene(newScene, LoadSceneMode.Single);
    }

    public void RunSceneTransitionWithTravelScene(string newScene, string travelScene, TravelSceneDialogue travelDialogue)
    {
        // Saves scene based data
        SaveSceneBasedData(newScene);

        // Resets timescale to 1
        Time.timeScale = 1;

        // Sets new scene and travel dialogue variable
        currentTravelDialogue = travelDialogue;
        currentNewScene = newScene;

        // Switches to travel scene
        SceneManager.LoadScene(travelScene, LoadSceneMode.Single);
    }


    // Scene Management Functions
    public void SaveSceneBasedData(string newSceneName)
    {
        // Saves scene data, if there is a persistent object manager
        if (persistentObjectManager != null)
        {
            persistentObjectManager.SaveSceneData();
        }

        // If there is a player in the scene, refreshes stats
        if(DataRef.playerReference != null)
        {
            DataRef.playerReference.RefreshPlayerStats();
        }

        // Saves game
        dataManager.SaveGameProgress(DataRef.baseData.saveName, newSceneName);
    }
}
