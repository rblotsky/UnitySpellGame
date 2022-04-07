using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ward Shape", menuName = "Spells/Shape Components/Ward Shape Component")]
public class WardShapeComponent : SpellShapeComponent
{
    // COMPONENT DATA //
    public static WardSpellController currentWard;


    // OVERRIDES //
    public override IEnumerator UseComponent(CombatEntity castingEntity, PlayerSpellObject spell)
    {
        // Prevents player from casting spells immediately
        if (castingEntity == DataRef.playerCombatEntity)
        {
            PlayerStats.canPlayerCastSpell = false;
        }

        // Removes currentWard if there is one, then sets it to null
        if (currentWard != null)
        {
            currentWard.DestroyWard();
        }

        currentWard = null;

        // Instantiates and starts the controller
        GameObject spellObject = Instantiate(spellPrefabObject, castingEntity.transform.position, spellPrefabObject.transform.localRotation);
        WardSpellController controller = spellObject.GetComponent<WardSpellController>();

        controller.SetSpellObject(spell);
        controller.StartController(castingEntity);
        currentWard = controller;

        // Finishes Casting Spell
        if (castingEntity == DataRef.playerCombatEntity)
        {
            DataRef.playerReference.StartSpellCooldown();
        }

        yield break;
    }
}
