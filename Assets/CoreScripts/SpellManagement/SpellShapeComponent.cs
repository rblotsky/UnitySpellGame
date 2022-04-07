using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellShapeComponent : PlayerSpellComponent
{
    // COMPONENT DATA //
    // Basic Data
    public GameObject spellPrefabObject;
    public PlayerSpellProperties baseProperties;
    public string spellShapeName;
    [TextArea(1, 3)]
    public string spellShapeDescription;


    // OVERRIDES //
    public override string GetDisplayDescription()
    {
        return "<b>Spell Shape</b>\n" + spellShapeDescription + "\n\n<b>Base Properties</b>\n" + baseProperties.GetIngamePropertyDescription();
    }

    public override string GetGeneratedName()
    {
        return spellShapeName + " Shape Component";
    }


    // FUNCTIONS //
    // Component Management
    public virtual IEnumerator UseComponent(CombatEntity castingEntity, PlayerSpellObject spell)
    {
        Debug.LogError("[SpellShapeComponent] " + name + " is using base UseComponent function. ");
        yield break;
    }

    public virtual bool AICheckUseConditions(StateBasedAI entity, Vector3 enemyPos, PlayerSpellProperties properties)
    {
        // By default, returns whether the enemy is within the spell's radius
        return (entity.transform.position - enemyPos).sqrMagnitude <= properties.radius * properties.radius;
    }
}
