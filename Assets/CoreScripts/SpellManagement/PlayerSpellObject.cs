using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

[System.Serializable]
public class PlayerSpellObject : IAttacker
{
    // DATA //
    // Components
    public SpellShapeComponent shapeComponent = null;
    public List<SpellModifierComponent> spellModifiers = new List<SpellModifierComponent>();
    public PlayerSpellProperties injectedProperties;

    // Properties
    public bool IsValid { get { return shapeComponent != null; } }


    // CONSTRUCTORS //
    public PlayerSpellObject(SpellShapeComponent newShape, SpellModifierComponent[] newModifiers)
    {
        // Sets all components
        shapeComponent = newShape;
        spellModifiers = new List<SpellModifierComponent>(newModifiers);
    }

    public PlayerSpellObject(SpellShapeComponent newShape, List<SpellModifierComponent> newModifiers)
    {
        // Sets all components
        shapeComponent = newShape;
        spellModifiers = newModifiers;
    }

    public PlayerSpellObject()
    {
        // Makes null shape and empty spellModifiers list
        shapeComponent = null;
        spellModifiers = new List<SpellModifierComponent>();
    }


    // FUNCTIONS //
    // Getting Data
    public PlayerSpellProperties GetSpellPropertiesWithModifiers()
    {
        // Uses base properties from shape component
        PlayerSpellProperties combinedProperties = shapeComponent.baseProperties;

        // Adds each modifier, then returns
        foreach(SpellModifierComponent modifier in spellModifiers)
        {
            if (modifier != null)
            {
                combinedProperties.Add(modifier.properties);
            }
        }

        // Adds injected properties
        combinedProperties.Add(injectedProperties);

        return combinedProperties;
    }

    public bool ContainsComponent(PlayerSpellComponent component)
    {
        if (shapeComponent == component)
        {
            return true;
        }

        else if (component is SpellModifierComponent)
        {
            if (spellModifiers.Contains((SpellModifierComponent)component))
            {
                return true;
            }
        }

        return false;
    }
    
    public string GetSpellDescription()
    {
        return GetSpellPropertiesWithModifiers().GetIngamePropertyDescription();
    }

    public string GetSpellName()
    {
        StringBuilder name = new StringBuilder();
        name.Append(shapeComponent.GetGeneratedName());
        name.Replace("Shape Component", "Spell");
        return name.ToString();
    }
}
