using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIProgressBar : MonoBehaviour
{
    // Progress Bar Data //
    // References
    public Image barBase;
    public Image barFill;
    public TextMeshProUGUI infoTextBox;

    // Text Type Data
    public UIValueDisplayType infoDisplayType;
    public bool displayZeroValue;
    public string baseValueUnits;

    // Progress Bar Values
    public float value;
    public float maxValue;
    public bool variableWidth = true; 

    // Cached Data
    private float initialWidth;

    // Constants
    public static readonly float DEFAULT_MAX_VALUE = 50;


    // FUNCTIONS //
    // Basic Functions
    private void Awake()
    {
        // Gets the bar base reference
        barBase = GetComponent<Image>();

        // Caches initial width
        initialWidth = barBase.rectTransform.rect.width;
    }


    // External Updating Functions
    public void SetValue(float newValue)
    {
        // Updates value
        value = newValue;

        // Updates UI
        UpdateFill();
        UpdateInfoDisplay();
    }

    public void SetMaxValue(float newValue)
    {
        // Updates max value
        maxValue = newValue;

        // Updates UI
        UpdateFill();
        UpdateInfoDisplay();
    }

    public virtual void SetBarColour(Color colour)
    {
        // Updates bar fill colour
        barFill.color = colour;
    }

    public virtual void SetInfoDisplayType(UIValueDisplayType displayType)
    {
        // Sets whether to use percent for value display
        infoDisplayType = displayType;

        // Updates UI
        UpdateInfoDisplay();
    }


    // UI Updating Functions
    protected virtual void UpdateInfoDisplay()
    {
        // Doesn't display anything if it's zero and shouldn't display zero
        if(value == 0 && !displayZeroValue)
        {
            infoTextBox.SetText("");
            return;
        }

        if (infoDisplayType == UIValueDisplayType.Percent_Of_Max)
        {
            // Sets to percent value from GetBarPercent + % symbol
            infoTextBox.SetText((GetBarPercent() * 100) + "%");
        }

        else if (infoDisplayType == UIValueDisplayType.Fraction_Of_Max)
        {
            // Sets to 'value / maxValue'
            infoTextBox.SetText(value + "/" + maxValue);
        }

        else if (infoDisplayType == UIValueDisplayType.Base_Value)
        {
            // Sets to 'value / maxValue'
            infoTextBox.SetText(value.ToString() + baseValueUnits);
        }
    }

    protected virtual void UpdateFill()
    {
        // Sets fill amount to decimal value from bar percent
        barFill.fillAmount = GetBarPercent();

        // Updates the width to be based on a mathematical formula to avoid massive changes between health values
        if (variableWidth)
        {
            float newWidth = (float)initialWidth * Mathf.Sqrt(maxValue / DEFAULT_MAX_VALUE);
            barBase.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
        }
    }


    // Utility Functions
    public float GetBarPercent()
    {
        return value / maxValue;
    }
}
