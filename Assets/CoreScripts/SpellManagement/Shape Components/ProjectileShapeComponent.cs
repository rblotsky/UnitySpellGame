using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile Shape", menuName = "Spells/Shape Components/Projectile Shape Component")]
public class ProjectileShapeComponent : SpellShapeComponent
{
    // COMPONENT DATA //
    // Constants
    public static readonly float FIRE_DELAY = 0.5f;
    public static readonly float SPAWN_DISTANCE_FROM_ENTITY = 2f;


    // OVERRIDES //
    public override IEnumerator UseComponent(CombatEntity castingEntity, PlayerSpellObject spell)
    {
        // Prevents player from casting spells until this finishes
        if (castingEntity == DataRef.playerCombatEntity)
        {
            PlayerStats.canPlayerCastSpell = false;
        }

        // Gets properites
        PlayerSpellProperties properties = spell.GetSpellPropertiesWithModifiers();

        // Instantiates each projectile one by one
        for (int i = 0; i < properties.amount; i++)
        {
            Vector3 entityPos = castingEntity.transform.position;

            GameObject spellObject = Instantiate(spellPrefabObject, castingEntity.transform.position+(castingEntity.transform.forward*SPAWN_DISTANCE_FROM_ENTITY), spellPrefabObject.transform.rotation);
            SpellController controller = spellObject.GetComponent<SpellController>();

            // Starts controller
            controller.SetSpellObject(spell);
            controller.StartController(castingEntity);

            yield return new WaitForSeconds(FIRE_DELAY);
        }

        // Starts player cooldown if player is casting
        if (castingEntity == DataRef.playerCombatEntity)
        {
            DataRef.playerReference.StartSpellCooldown();
        }
    }
}
