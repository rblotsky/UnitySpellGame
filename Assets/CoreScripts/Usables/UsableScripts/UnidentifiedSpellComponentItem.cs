using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unidentified Spell Component Item", menuName = "Usables/Unidentified SpellComponentItem")]
public class UnidentifiedSpellComponentItem : SpellComponentItem
{
    // OVERRIDES //
    // Basic Functions
    protected override void Awake()
    {
        itemType = UsableType.Spell_Tome;
    }


    // Item Management
    public override bool MechanicalComparison(Usable other)
    {
        // Returns true if the other item is an unidentified spell component
        return other is UnidentifiedSpellComponentItem;
    }
}
