using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TravelSceneOverlayUIManager : MonoBehaviour
{
    // DATA //
    // UI References
    public DialogueDisplayer dialogueDisplayer;
    public Image fadeOverlay;

    // Constants
    public static readonly float FADE_TIME_SECONDS = 1;
    public static readonly float NUM_FADE_INTERVALS = 30;


    // FUNCTIONS //
    // UI Management Functions
    public IEnumerator RunFade(int direction)
    {
        // Clamps direction to (-1,1)
        direction = Mathf.Clamp(direction, -1, 1);

        // Caches the amount of fade intervals and time for each
        float intervalFadeAmount = 1 / NUM_FADE_INTERVALS;
        float fadeIntervalTime = FADE_TIME_SECONDS / NUM_FADE_INTERVALS;

        // Fades in/out
        for(int i = 0; i < NUM_FADE_INTERVALS; i++)
        {
            // Adjusts colour for fade alpha
            Color fadeAdjustedColour = new Color(
                fadeOverlay.color.r, 
                fadeOverlay.color.g, 
                fadeOverlay.color.b, 
                fadeOverlay.color.a + direction * intervalFadeAmount);

            fadeOverlay.color = fadeAdjustedColour;

            yield return new WaitForSeconds(fadeIntervalTime);
        }
    }
}
