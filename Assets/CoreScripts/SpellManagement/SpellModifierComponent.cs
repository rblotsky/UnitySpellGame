using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Modifier Component", menuName = "Spells/Modifier Component")]
public class SpellModifierComponent : PlayerSpellComponent
{
    // DATA //
    public PlayerSpellProperties properties;


    // OVERRIDES
    public override string GetDisplayDescription()
    {
        return "Effects:\n" + properties.GetIngamePropertyDescription();
    }

    public override string GetGeneratedName()
    {
        string generatedName = "";

        // Checks each property - if it has a value, adds the name of that property to the generated name
        if(properties.radius != 0)
        {
            generatedName += "Radius";
        }

        if(properties.duration != 0)
        {
            if (!string.IsNullOrWhiteSpace(generatedName))
            {
                generatedName += "+";
            }

            generatedName += "Duration";
        }

        if (properties.amount != 0)
        {
            if (!string.IsNullOrWhiteSpace(generatedName))
            {
                generatedName += "+";
            }

            generatedName += "Amount";
        }

        if (properties.elements.TotalElements != 0)
        {
            if (!string.IsNullOrWhiteSpace(generatedName))
            {
                generatedName += "+";
            }

            generatedName += "Combat Effect";
        }

        if (properties.damagedEntities.Length != 0)
        {
            if (!string.IsNullOrWhiteSpace(generatedName))
            {
                generatedName += "+";
            }

            generatedName += "Damage Type";
        }

        if (properties.keywords.Length != 0)
        {
            if (!string.IsNullOrWhiteSpace(generatedName))
            {
                generatedName += "+";
            }

            generatedName += "Keyword";
        }

        // Adds final part to name
        if (string.IsNullOrWhiteSpace(generatedName))
        {
            generatedName += "Empty Modifier (No Effect)";
        }

        else
        {
            generatedName += " Modifier";
        }

        return generatedName;
    }
}
