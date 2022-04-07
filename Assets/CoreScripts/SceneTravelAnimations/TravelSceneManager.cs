using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TravelSceneManager : MonoBehaviour
{
    // DATA //
    // References
    public TravelSceneOverlayUIManager sceneOverlayManager;

    // Cached Data
    private TravelSceneDialogue cachedTravelDialogue;
    private string cachedNewSceneName;
    private Camera thisSceneCamera;
    private AudioListener thisSceneAudioListener;


    // FUNCTIONS //
    // Basic Functions
    private void Start()
    {
        // Gets scene references
        sceneOverlayManager = FindObjectOfType<TravelSceneOverlayUIManager>();
        thisSceneCamera = Camera.main;
        thisSceneAudioListener = FindObjectOfType<AudioListener>();

        // Gets new scene and travel dialogue, then resets it in SceneTransitionManager
        cachedTravelDialogue = SceneTransitionManager.currentTravelDialogue;
        cachedNewSceneName = SceneTransitionManager.currentNewScene;
        SceneTransitionManager.ResetCachedStaticData();

        // Runs travel scene
        StartCoroutine(RunTravelScene());
    }


    // Managing Scene
    public IEnumerator RunTravelScene()
    {
        Debug.Log("Started running travel scene!");
        // Sets timescale to 1 (It may have been modified prior to loading this scene)
        Time.timeScale = 1;

        // Starts loading the next scene asynchronously (NOTE: This will run awake on the objects in that scene, need to prevent it from doing that.)
        Scene thisScene = SceneManager.GetActiveScene();
        AsyncOperation nextSceneStatus = SceneManager.LoadSceneAsync(cachedNewSceneName, LoadSceneMode.Additive);
        nextSceneStatus.allowSceneActivation = false;

        // Starts animations before fadein runs
        //TODO: Add animations to travel scene

        // Opens the scene w/ fade in, runs dialogue, and then waits until next scene is finished loading (waits until 0.9 because it isn't allowed to fully load yet)
        yield return StartCoroutine(sceneOverlayManager.RunFade(-1));
        sceneOverlayManager.dialogueDisplayer.QueueDialogue(cachedTravelDialogue, null);
        yield return sceneOverlayManager.dialogueDisplayer.currentDialogueCoroutine;

        // Fades out, then loads new scene
        yield return StartCoroutine(sceneOverlayManager.RunFade(1));

        // Removes current camera, loads new scene fully
        if (thisSceneCamera != null)
        {
            thisSceneCamera.enabled = false;
        }

        if(thisSceneAudioListener != null)
        {
            thisSceneAudioListener.enabled = false;
        }

        nextSceneStatus.allowSceneActivation = true;
        yield return nextSceneStatus;
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }
}
