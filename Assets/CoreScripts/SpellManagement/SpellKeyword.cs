using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellKeyword : GameResource
{
    // KEYWORD DATA //
    // Basic Data
    public string displayName;
    public string functionalityDescription;

    // FUNCTIONS //
    // NOTE: SINGLETON PATTERN MUST BE WRITTEN IN EACH SUBCLASS INDIVIDUALLY!
    
    // Main Functionality
    public virtual void AddEvents(SpellController controllerToAddTo)
    {
        Debug.LogError("[SpellKeyword] \"" + name + "\" is running base AddEvents function!");
    }
}
