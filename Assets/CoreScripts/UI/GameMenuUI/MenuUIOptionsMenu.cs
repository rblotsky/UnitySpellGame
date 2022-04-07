using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuUIOptionsMenu : MonoBehaviour
{
    // DATA //
    // References
    public Slider textSpeedSlider;


    // FUNCTIONS //
    // Setup
    public void SetupMenu()
    {
        textSpeedSlider.value = GameSettings.settings.charReadSpeed;
    }


    // UI Events
    public void ReturnToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenuScene", LoadSceneMode.Single);
    }

    public void OnDataUpdate()
    {
        // Updates settings
        if(textSpeedSlider != null)
        {
            GameSettings.settings.charReadSpeed = textSpeedSlider.value;
        }

        // Saves settings
        GameSettings.SaveSettings();
    }
}
