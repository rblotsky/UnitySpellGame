using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerSpellComponent : GameResource
{
    // DATA //
    // Basic Data //
    public Sprite spellManagerDisplaySprite;

    // Properties
    public string DisplayName { get { return GetGeneratedName(); } }


    // FUNCTIONS //
    // Utility
    public virtual string GetDisplayDescription()
    {
        return "Base Description";
    }

    public virtual string GetGeneratedName()
    {
        return "Empty Name";
    }
}
