using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public struct CombatEffects : IEquatable<CombatEffects>
{
    // DATA //
    // Basic Values
    public int healthInstant;
    public int healthChange;
    public int speedForDuration;
    public int effectDuration;

    // Properties
    public int TotalElements { get { return healthInstant + healthChange + speedForDuration + effectDuration; } }
    public int[] ElementList { get { return new int[4] { healthInstant, healthChange, speedForDuration, effectDuration }; } }
    public bool IsZero { get { return healthInstant == 0 && healthChange == 0 && speedForDuration == 0 && effectDuration == 0; } }

    // Constants
    public static readonly float SPEED_STUN_AMOUNT_RAW = -100f;
    public static readonly string[] effectNamesShort = new string[4] { "HP", "HP/s", "% [S]", "s" };
    public static readonly string[] effectNamesLong = new string[4] { "HP (Instant)", "HP/s (Timed)", "% Speed (Timed)", "Effect Duration" };
    public static readonly string stunEffectText = "STUN!";
    public static readonly Color speedEffectColour = new Color(141, 197, 255);
    public static readonly Color healthEffectColour = new Color(255, 149, 131);
    public static readonly Color healthChangeEffectColour = new Color(154, 255, 143);
    public static readonly CombatEffects zero = new CombatEffects();


    // CONSTRUCTORS //
    public CombatEffects(int newHealth, int newPoison, int newSpeed, int newDuration)
    {
        healthInstant = newHealth;
        healthChange = newPoison;
        speedForDuration = newSpeed;
        effectDuration = newDuration;
    }

    public CombatEffects(int[] newElements)
    {
        healthInstant = newElements[0];
        healthChange = newElements[1];
        speedForDuration = newElements[2];
        effectDuration = newElements[3];
    }

    public CombatEffects(CombatEffects[] newElements)
    {
        // Sets all values to 0 to start
        healthInstant = 0;
        healthChange = 0;
        speedForDuration = 0;
        effectDuration = 0;

        // For each element in list, adds to existing elements
        foreach(CombatEffects elements in newElements)
        {
            healthInstant += elements.healthInstant;
            healthChange += elements.healthChange;
            speedForDuration += elements.speedForDuration;
            effectDuration += elements.effectDuration;
        }
    }


    // OVERRIDES //
    public override string ToString()
    {
        // Returns a CSV formatted version of the elements, in brackets
        return "(" + healthInstant + "," + healthChange + "," + speedForDuration + "," + effectDuration + ")";
    }


    // FUNCTIONS //
    // Member Functions
    public CombatEffects Add(CombatEffects secondElements)
    {
        healthInstant += secondElements.healthInstant;
        healthChange += secondElements.healthChange;
        speedForDuration += secondElements.speedForDuration;
        effectDuration += secondElements.effectDuration;

        return this;
    }

    public CombatEffects Subtract(CombatEffects secondElements)
    {
        healthInstant -= secondElements.healthInstant;
        healthChange -= secondElements.healthChange;
        speedForDuration -= secondElements.speedForDuration;
        effectDuration -= secondElements.effectDuration;

        return this;
    }

    public bool Equals(CombatEffects other)
    {
        // Returns whether all elements are equal
        return healthInstant == other.healthInstant && healthChange == other.healthChange && speedForDuration == other.speedForDuration && effectDuration == other.effectDuration;
    }

    public string GetDisplayText(bool useLongNames)
    {
        string displayText = "";
        string[] effectNamesToUse;

        if (useLongNames)
        {
            effectNamesToUse = effectNamesLong;
        }

        else
        {
            effectNamesToUse = effectNamesShort;
        }

        if (healthInstant != 0)
        {
            displayText += healthInstant + " " + GameUtility.GenerateHTMLColouredText(effectNamesToUse[0], healthEffectColour) + "\n";
        }

        if (healthChange != 0)
        {
            displayText += healthChange + " " + GameUtility.GenerateHTMLColouredText(effectNamesToUse[1], healthChangeEffectColour) + "\n";
        }

        if (speedForDuration == SPEED_STUN_AMOUNT_RAW)
        {
            displayText += "<b>" + GameUtility.GenerateHTMLColouredText(stunEffectText, speedEffectColour) + "</b>" + "\n";
        }

        else if (speedForDuration != 0)
        {
            displayText += speedForDuration + " " + GameUtility.GenerateHTMLColouredText(effectNamesToUse[2], speedEffectColour) + "\n";
        }

        if (effectDuration != 0)
        {
            displayText += effectDuration + " " + effectNamesToUse[3] + "\n";
        }

        return displayText;
    }


    // Static Functions
    public static CombatEffects AddElements(CombatEffects firstElements, CombatEffects secondElements)
    {
        // Adds all elements together
        CombatEffects returnElements = new CombatEffects();

        returnElements.healthInstant = firstElements.healthInstant + secondElements.healthInstant;
        returnElements.healthChange = firstElements.healthChange + secondElements.healthChange;
        returnElements.speedForDuration = firstElements.speedForDuration + secondElements.speedForDuration;
        returnElements.effectDuration = firstElements.effectDuration + secondElements.effectDuration;

        return returnElements;
    }

    public static CombatEffects SubtractElements(CombatEffects firstElements, CombatEffects secondElements)
    {
        // Subtracts second from first elements
        CombatEffects returnElements = new CombatEffects();

        returnElements.healthInstant = firstElements.healthInstant - secondElements.healthInstant;
        returnElements.healthChange = firstElements.healthChange - secondElements.healthChange;
        returnElements.speedForDuration = firstElements.speedForDuration - secondElements.speedForDuration;

        return returnElements;
    }

    public static bool TryParse(string text, out CombatEffects parsedValue)
    {
        // parsedValue is set to an empty CombinedElements
        parsedValue = new CombatEffects();

        // Gets list of values from text
        string[] values = text.Replace('(', ' ').Replace(')', ' ').Trim().Split(',');

        // If the length isn't 5 returns false and uses empty parsedValue
        if(values.Length != 4)
        {
            return false;
        }

        // If the length is correct, returns true and parses the values
        else
        {
            // Creates list of integer values
            List<int> intValues = new List<int>();

            // Iterates through all string values
            foreach(string value in values)
            {
                // Tries parsing each value in the values list
                int integerValue;

                // If it's an integer, adds to intValues
                if(int.TryParse(value, out integerValue))
                {
                    intValues.Add(integerValue);
                }

                // If it isn't an integer, returns false and uses empty parsedValue
                else
                {
                    return false;
                }
            }

            // If loop is completed, returns true and sets parsedValue to a CombinedElements using integer values
            parsedValue = new CombatEffects(intValues.ToArray());
            return true;
        }
    }

    public static CombatEffects Parse(string text)
    {
        // Gets string of values from given text (removes brackets and spaces)
        string[] values = text.Replace('(', ' ').Replace(')', ' ').Trim().Split(',');

        // Creates empty list of integers
        int[] intValues = new int[4] { 0, 0, 0, 0};
        int intValuesIndex = 0;

        // Iterates through list of values
        for(int i = 0; i < values.Length; i++)
        {
            // If the int values index is higher than possible indexes in intValues, breaks loop
            if(intValuesIndex >= intValues.Length)
            {
                break;
            }

            // If the value is an integer, parses it and adds it to the intValues list
            int outInteger;

            if(int.TryParse(values[i], out outInteger))
            {
                intValues[intValuesIndex] = outInteger;
                intValuesIndex++;
            }
        }

        // Returns a new CombinedElements created using intValues list
        return new CombatEffects(intValues);
    }
}
