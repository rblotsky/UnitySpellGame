using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Cone Shape", menuName = "Spells/Shape Components/Cone Shape Component")]
public class ConeShapeComponent : SpellShapeComponent
{
    // DATA //
    // Basic Data
    public GameObject particlesPrefab;
    public int numIncrements = 50;
    public float spreadAngle = 20.0f;


    // OVERRIDES //
    public override IEnumerator UseComponent(CombatEntity castingEntity, PlayerSpellObject spell)
    {
        // Prevents player from casting spells
        if (castingEntity == DataRef.playerCombatEntity)
        {
            PlayerStats.canPlayerCastSpell = false;
        }

        // Spawns prefab and starts controller
        GameObject prefab = Instantiate(spellPrefabObject, castingEntity.transform.position, castingEntity.transform.rotation);
        SpellController controller = prefab.GetComponent<SpellController>();

        controller.SetSpellObject(spell);
        controller.StartController(castingEntity);

        // Finishes casting spell
        if (castingEntity == DataRef.playerCombatEntity)
        {
            DataRef.playerReference.StartSpellCooldown();
        }

        yield break;
    }
}
