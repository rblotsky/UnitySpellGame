using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayerDataDisplayer : UICombatEntityDataDisplayer
{
    enum HoverType
    {
        None,
        Health_Change,
        Speed,
    }


    // DATA //
    // References
    public UIProgressBar spellCooldownBar;
    public UITooltip tooltipPanel;

    // Cached Data
    private HoverType hoverType; 


    // FUNCTIONS //
    // Basic Functions
    private void Update()
    {
        // Runs tooltip panel
        if (hoverType != HoverType.None)
        {
            tooltipPanel.gameObject.SetActive(true);
            tooltipPanel.UpdatePosition(Input.mousePosition);
            
            if(hoverType == HoverType.Health_Change)
            {
                UpdateHealthChangeText();
            }

            else
            {
                UpdateSpeedText();
            }
        }

        else
        {
            tooltipPanel.gameObject.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        // Only updates progress bar if the time remaining isn't the same as before
        if (DataRef.playerReference.RemainingSpellCooldown != spellCooldownBar.value)
        {
            // Updates progress bar to remaining cooldown out of total cooldown
            spellCooldownBar.SetValue(DataRef.playerReference.RemainingSpellCooldown);
            spellCooldownBar.SetMaxValue(PlayerStats.mainStats.spellCooldown);
        }
    }


    // UI Event Functions
    public void StartHoverSpeedText()
    {
        // Starts hover
        hoverType = HoverType.Speed;
        UpdateSpeedText();
    }

    public void StartHoverHealthChangeText()
    {
        // Starts hover
        hoverType = HoverType.Health_Change;
        UpdateHealthChangeText();
    }


    // Management
    private void UpdateSpeedText()
    {
        // Sets the text to a list of all the speed effects
        string text = "";

        foreach (CombatEntityEffectController.EffectData effect in effectController.speedEffects)
        {
            text += "<b>" + GameUtility.GenerateHTMLColouredText(effect.value.ToString(), CombatEffects.speedEffectColour) + "</b> for " + (Time.time - (effect.duration + effect.startTime)) + "s\n";
        }

        if (!string.IsNullOrEmpty(text))
        {
            tooltipPanel.UpdateText(text);
        }

        else
        {
            FinishHover();
        }
    }

    private void UpdateHealthChangeText()
    {
        // Sets the text to a list of all the health change effects
        string text = "";

        foreach (CombatEntityEffectController.EffectData effect in effectController.healthChangeEffects)
        {
            text += "<b>" + GameUtility.GenerateHTMLColouredText(effect.value.ToString(), CombatEffects.healthChangeEffectColour) + "</b> for " + (Time.time - (effect.duration + effect.startTime)) + "s\n";
        }

        if (!string.IsNullOrEmpty(text))
        {
            tooltipPanel.UpdateText(text);
        }

        else
        {
            FinishHover();
        }
    }

    public void FinishHover()
    {
        hoverType = HoverType.None;
    }
}
