using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellController : MonoBehaviour
{
    // CONTROLLER DATA //
    // Base Data
    [HideInInspector]
    public PlayerSpellProperties properties;
    [HideInInspector]
    public PlayerSpellObject spellObject;

    // Delegates and Events
    public delegate void SpellDamageDelegate(SpellController controllerUsed, CombatEntity damagedEntity, Vector3 damagePosition);
    public delegate void SpellBaseDelegate(SpellController controllerUsed);
    public SpellDamageDelegate onSpellDamageDealtBeforeKeywords;
    public SpellBaseDelegate onControllerStart;
    public SpellBaseDelegate onControllerFinish;

    // Cached Data
    protected CombatEntity entity;

    // FUNCTIONS //
    public virtual void SetSpellObject(PlayerSpellObject spell)
    {
        // Sets all spell data
        spellObject = spell;
        properties = spell.GetSpellPropertiesWithModifiers();

        // Sets up spell controller
        foreach(SpellKeyword keyword in properties.keywords)
        {
            keyword.AddEvents(this);
        }
    }

    public virtual void StartController(CombatEntity castingEntity)
    {
        if (onControllerStart != null)
        {
            onControllerStart(this);
        }

        entity = castingEntity;
    }

    public virtual void FinishController(CombatEntity castingEntity)
    {
        if(onControllerFinish != null)
        {
            onControllerFinish(this);
        }

        onControllerFinish = null;
        onSpellDamageDealtBeforeKeywords = null;
        onControllerFinish = null;
        Destroy(gameObject);
    }
}
