using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Misc Item", menuName = "Usables/Misc Item")]
public class MiscUsable : Usable
{
    // FUNCTIONS //
    // Basic Functions
    protected override void Awake()
    {
        itemType = UsableType.Misc_Item;
    }


    // Item Management
    public override bool UseItem(PlayerComponent playerToUse)
    {
        return false;
    }
}
