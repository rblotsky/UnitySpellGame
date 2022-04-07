using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

[CreateAssetMenu(fileName = "New Potion", menuName = "Usables/Potion")]
public class Potion : Usable
{
    // POTION DATA //
    [Header("Potion Effects")]
    public CombatEffects effects;


    // OVERRIDES //
    // Basic Functions
    protected override void Awake()
    {
        itemType = UsableType.Potion;
    }


    // Item Management
    public override bool UseItem(PlayerComponent playerToUse)
    {
        // Applies effects
        playerToUse.component_CombatEntity.ApplyEffects(effects, playerToUse.GetPlayerPosition(), true, playerToUse.component_CombatEntity);
        return true;
    }

    public override string GetItemDescription()
    {
        return "Effects: \n" + effects.GetDisplayText(true);
    }

    public override string GetItemName()
    {
        StringBuilder nameBuilder = new StringBuilder();

        // Adds info for which combat effects are used
        if(effects.healthInstant != 0)
        {
            nameBuilder.Append(CombatEffects.effectNamesLong[0] + ", ");
        }

        if (effects.healthChange != 0)
        {
            nameBuilder.Append(CombatEffects.effectNamesLong[1] + ", ");
        }

        if (effects.speedForDuration != 0)
        {
            nameBuilder.Append(CombatEffects.effectNamesLong[2] + ", ");
        }

        // Removes final comma
        if(nameBuilder.Length > 0)
        {
            nameBuilder.Remove(nameBuilder.Length - 2, 1);
        }

        // Adds "Potion"
        nameBuilder.Append("Potion");

        // Returns completed name
        return nameBuilder.ToString();
    }
}
