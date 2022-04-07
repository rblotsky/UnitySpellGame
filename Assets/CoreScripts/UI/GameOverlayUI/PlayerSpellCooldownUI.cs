using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpellCooldownUI : MonoBehaviour
{
    // DATA //
    // References
    public UIProgressBar progressBar;

    // Cached Data
    PlayerComponent player;


    // FUNCTIONS //
    // Basic Functions
    private void Awake()
    {
        // Finds the player in the scene
        player = FindObjectOfType<PlayerComponent>();
    }

    private void LateUpdate()
    {
        // Only updates progress bar if the time remaining isn't the same as before
        if(player.RemainingSpellCooldown != progressBar.value)
        {
            // Updates progress bar to remaining cooldown out of total cooldown
            progressBar.SetValue(player.RemainingSpellCooldown);
            progressBar.SetMaxValue(PlayerStats.mainStats.spellCooldown);
        }
    }
}
