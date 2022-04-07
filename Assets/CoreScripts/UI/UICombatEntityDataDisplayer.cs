using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICombatEntityDataDisplayer : MonoBehaviour
{
    // Data Structures //
    public enum EffectDisplayType
    {
        Raw_Value,
        Enable_Icons,
        Plus_Minus_Zero_Display,
        None,
    }



    // DATA //
    // References
    private CombatEntity entityReference;
    protected CombatEntityEffectController effectController;
    [Header("Value/Sign Display")]
    public UIProgressBar healthBar;
    public TextMeshProUGUI healthChangeEffectText;
    public TextMeshProUGUI speedEffectText;
    [Header("Icons Display")]
    public GameObject healthChangeEffectDisplayer;
    public GameObject speedEffectDisplayer;
    public Sprite positiveValue;
    public Sprite negativeValue;

    // Basic Data
    [Header("Display")]
    public EffectDisplayType displayType = EffectDisplayType.Raw_Value;
    public Color mobsHealthColour = Color.red;
    public Color worldObjectHealthColour = Color.green;
    public bool autoAssignColour = false;

    // Cached Data
    private bool eventsAdded = false;


    // FUNCTIONS //
    private void OnEnable()
    {
        // Adds all required events if necessary
        AddEventsIfNotAdded();
    }

    private void OnDisable()
    {
        // Removes all events
        RemoveEventsIfAdded();
    }


    // Events Management
    private void AddEventsIfNotAdded()
    {
        // If events haven't been added, adds them and caches that they are added
        if (!eventsAdded)
        {
            // Adds the events
            if (effectController != null)
            {
                effectController.onHealthChangeEffectUpdate += UpdateHealthChangeEffect;
                effectController.onSpeedEffectUpdate += UpdateSpeedEffect;
                UpdateHealthChangeEffect();
                UpdateSpeedEffect();
            }

            if (entityReference != null)
            {
                entityReference.onHealthModify += UpdateHealth;
                UpdateHealth();
            }

            // Caches that events are added
            eventsAdded = true;
        }
    }

    private void RemoveEventsIfAdded()
    {
        // If already added events, removes them and caches that they are not added
        if (eventsAdded)
        {
            if (effectController != null)
            {
                effectController.onHealthChangeEffectUpdate -= UpdateHealthChangeEffect;
                effectController.onSpeedEffectUpdate -= UpdateSpeedEffect;
            }

            if(entityReference != null)
            {
                entityReference.onHealthModify -= UpdateHealth;
            }

            // Caches that events are not added
            eventsAdded = false;
        }
    }

    public void SetEntityReference(CombatEntity entity)
    {
        // Removes all events
        RemoveEventsIfAdded();

        // Caches the new combat entity and controller
        entityReference = entity;
        effectController = entity.effectController;

        // Adds events
        AddEventsIfNotAdded();

        // Updates progress bar colour
        if(autoAssignColour && entityReference != null)
        {
            if(entityReference.acceptedDamageType == DamageType.Mobs)
            {
                healthBar.SetBarColour(mobsHealthColour);
            }

            else if(entityReference.acceptedDamageType == DamageType.Objects)
            {
                healthBar.SetBarColour(worldObjectHealthColour);
            }
        }
    }


    // UI Management
    public void UpdateHealth(int healthModifiedAmount = 0)
    {
        if (healthBar != null)
        {
            healthBar.SetMaxValue(entityReference.maxHealth);
            healthBar.SetValue(entityReference.health);
        }
    }

    public void UpdateHealthChangeEffect()
    {
        if (displayType == EffectDisplayType.Raw_Value)
        {
            string effectText = "";

            if (effectController.HealthChangeAmount >= 0)
            {
                effectText += "+";
            }

            effectText += effectController.HealthChangeAmount;

            healthChangeEffectText.SetText(effectText);
        }

        else if(displayType == EffectDisplayType.Plus_Minus_Zero_Display)
        {
            if (effectController.HealthChangeAmount > 0)
            {
                healthChangeEffectText.SetText("+");
            }

            else if (effectController.HealthChangeAmount < 0)
            {
                healthChangeEffectText.SetText("-");
            }

            else
            {
                healthChangeEffectText.SetText("");
            }
        }

        else if(displayType == EffectDisplayType.Enable_Icons)
        {

        }
    }

    public void UpdateSpeedEffect()
    {
        if (displayType == EffectDisplayType.Raw_Value)
        {
            string effectText = "";

            if (effectController.SpeedModifier >= 0)
            {
                effectText += "+";
            }

            effectText += effectController.SpeedModifier + "%";

            if (effectController.SpeedModifier == CombatEffects.SPEED_STUN_AMOUNT_RAW)
            {
                effectText = "<b>STUN!</b>";
            }

            speedEffectText.SetText(effectText);
        }

        else if(displayType == EffectDisplayType.Plus_Minus_Zero_Display)
        {
            if (effectController.SpeedModifier > 0)
            {
                speedEffectText.SetText("+");
            }

            else if (effectController.SpeedModifier < 0)
            {
                speedEffectText.SetText("-");
            }

            else
            {
                speedEffectText.SetText("");
            }
        }

        else if(displayType == EffectDisplayType.Enable_Icons)
        {

        }
    }
}
