using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class SpellScroll : Usable
{
    // SCROLL DATA //
    public PlayerSpellObject spell;


    // OVERRIDES //
    // Basic Functions
    protected override void Awake()
    {
        itemType = UsableType.Spell_Scroll;
    }


    // Item Management
    public override bool MechanicalComparison(Usable other)
    {
        if(other is SpellScroll)
        {
            SpellScroll otherScroll = (SpellScroll)other;

            if (spell != null && otherScroll.spell != null)
            {
                return spell.shapeComponent == otherScroll.spell.shapeComponent;
            }
        }

        return false;
    }

    public override bool UseItem(PlayerComponent playerToUse)
    {
        if(spell != null)
        {
            if (spell.shapeComponent != null)
            {
                playerToUse.StartCoroutine(spell.shapeComponent.UseComponent(playerToUse.component_CombatEntity, spell));
                return true;
            }
        }

        return false;
    }

    public override string GetItemDescription()
    {
        StringBuilder description = new StringBuilder();

        description.Append("Spell:\n");

        if(spell != null)
        {
            if(spell.shapeComponent != null)
            {
                description.Append(spell.shapeComponent.GetGeneratedName() + "\n");
            }

            foreach(SpellModifierComponent modifier in spell.spellModifiers)
            {
                if (modifier != null)
                {
                    description.Append(spell.shapeComponent.GetGeneratedName());
                }
            }
        }

        return description.ToString();
    }
}
