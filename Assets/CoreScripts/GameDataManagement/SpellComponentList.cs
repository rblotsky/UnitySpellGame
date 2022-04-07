using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpellComponentList 
{ 

    // Main lists
    [Header("Master List")]
    public static PlayerSpellComponent[] masterList;

    // FUNCTIONS
    // Finding components
    public static PlayerSpellComponent GetSpellComponent(string componentID)
    {
        // Finds spell component
        foreach(PlayerSpellComponent component in masterList)
        {
            if (component.id.ToString().Equals(componentID))
            {
                return component;
            }
        }

        // Returns null if none found
        return null;
    }

    // Setup
    public static void SetupFunction()
    {
        // Gets a list of all spell components from Resources
        masterList = Resources.LoadAll<PlayerSpellComponent>("Spell Components");
    }
}
