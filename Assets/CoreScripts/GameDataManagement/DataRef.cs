using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class DataRef : MonoBehaviour
{
    // Static Data References
    public static PlayerComponent playerReference { get; private set; }
    public static CombatEntity playerCombatEntity { get; private set; }
    public static OverlayUIManager overlayManagerReference { get; private set; }
    public static SceneGeneralMenuManager sceneMenuManagerReference { get; set; }
    public static SceneTransitionManager transitionManagerReference { get; set; }
    public static BaseGameData baseData;


    // FUNCTIONS //
    // Setup and Management
    public static void UpdatePlayerRefs(PlayerComponent newPlayer)
    {
        // If reference exists, sets all values according to that component's built-in values.
        if (newPlayer != null)
        {
            playerReference = newPlayer;
            playerCombatEntity = playerReference.component_CombatEntity;
            overlayManagerReference = newPlayer.overlayManager;
        }
    }

    public static void ResetAllSceneRefs()
    {
        // Resets all scene references to null.
        ResetPlayerRefs();
        sceneMenuManagerReference = null;
        transitionManagerReference = null;
    }

    public static void ResetPlayerRefs()
    {
        playerReference = null;
        playerCombatEntity = null;
        overlayManagerReference = null;
    }

    public static void RefreshBaseGameData()
    {
        baseData.currentSceneName = SceneManager.GetActiveScene().name;
    }

    public static void RefreshBaseGameData(string sceneName)
    {
        baseData.currentSceneName = sceneName;
    }

    public static void LoadBaseGameData(string text)
    {
        baseData = JsonConvert.DeserializeObject<BaseGameData>(text);
    }
}
