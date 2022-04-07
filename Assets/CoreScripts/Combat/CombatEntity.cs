using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;

[DisallowMultipleComponent]
[RequireComponent(typeof(CombatEntityEffectController))]
public class CombatEntity : MonoBehaviour, IAttacker
{
    // ENTITY DATA //
    // Basic Data
    [Header("Basic Data/References")]
    [Tooltip("The type of damage that affects this entity")]
    public DamageType acceptedDamageType;
    [Tooltip("If not set manually, this is set automatically.")]
    public Rigidbody entityRB;
    [Tooltip("If not set manually, this is set automatically.")]
    public CombatEntityEffectController effectController;
    [Space]

    // Health
    [Header("Health/Defense")]
    public int health;
    [Min(0)] public int maxHealth;

    // Defense
    public CombatEffects defense = new CombatEffects();

    // Events
    public delegate void OnHealthModifyEvent(int modifiedAmount);
    public delegate void NoInputDelegate();
    public delegate void AttackApplyDelegate(CombatEffects elements, Vector3 attackSourcePosition);
    public event OnHealthModifyEvent onHealthModify;
    public event NoInputDelegate onDeath;
    public event AttackApplyDelegate onAttackApply;


    // FUNCTIONS //
    // Basic Functions
    public void Awake()
    {
        // Gets references to other components
        // Only gets reference if it isn't assigned manually
        if (entityRB == null)
        {
            entityRB = GetComponent<Rigidbody>();
        }

        if(effectController == null)
        {
            effectController = GetComponent<CombatEntityEffectController>();
        }
    }


    // Data Setup
    public void SetupData(int newHealth, int newMaxHealth, CombatEffects newDefense)
    {
        health = newHealth;
        maxHealth = newMaxHealth;
        defense = newDefense;
    }


    // Health Modification
    public virtual void ModifyEntityHealth(int amountToChange)
    {
        // Adds health
        health += amountToChange;

        // Clamps health to max and checks for death
        CheckToRunDeathEvent();
        ClampHealthOverMax();

        // Runs health modify event
        if (onHealthModify != null)
        {
            onHealthModify(amountToChange);
        }
    }


    // Damage Application
    public void ApplyEffects(CombatEffects attackElements, Vector3 attackSourcePosition, bool ignoreDefense, IAttacker attacker)
    {
        //Debug.Log("[CombatEntity] Applying Effects: ATTACKER:" + attacker.ToString() + " ELEMENTS: " + attackElements.ToString() + "DAMAGED: " + name);

        // Calculates damage
        CombatEffects attackApplied = attackElements;

        if (!ignoreDefense)
        {
            attackApplied = GameUtility.CalculateDefense(attackElements, defense);
        }

        // Does nothing if the attack applied is zero
        if (attackApplied.IsZero)
        {
            return;
        }

        // Modifies health by damage value
        ModifyEntityHealth(attackApplied.healthInstant);

        // Applies knockback
        if (entityRB != null)
        {
            entityRB.AddForceAtPosition((transform.position - attackSourcePosition) * GameUtility.CalculateKnockbackForce(attackApplied), attackSourcePosition, ForceMode.Impulse);
        }

        // Adds element effects
        if (attackApplied.healthChange != 0)
        {
            effectController.AddHealthChangeEffectFromAttack(attackApplied.healthChange, attackApplied.effectDuration, attacker);
        }

        if (attackApplied.speedForDuration != 0)
        {
            effectController.AddSpeedEffectFromAttack(attackApplied.speedForDuration, attackApplied.effectDuration, attacker);
        }

        if (onAttackApply != null)
        {
            onAttackApply(attackApplied, attackSourcePosition);
        }
    }

    public bool ApplyEffects(CombatEffects damageElements, Vector3 attackSourcePosition, DamageType[] damageTypes, bool ignoreDefense, IAttacker attacker)
    {
        // Checks all damage types, and stops function if there are none of accepted type or ALL type.
        if (!damageTypes.Contains(acceptedDamageType) && !damageTypes.Contains(DamageType.All))
        {
            return false;
        }

        else
        {
            // Applies damage using main damage function
            ApplyEffects(damageElements, attackSourcePosition, ignoreDefense, attacker);
            return true;
        }
    }

    public void KillEntity()
    {
        ModifyEntityHealth(-maxHealth);
    }

    // Health Checks
    private void ClampHealthOverMax()
    {
        // Sets health to min value between health and max health
        health = Mathf.Min(health, maxHealth);
    }

    private void CheckToRunDeathEvent()
    {
        // If health is at zero or below, runs death event
        if (health <= 0 && onDeath != null)
        {
            onDeath();
        }
    }


    // Utility
    [ContextMenu("Default: Weak")]
    public void SetHealthDefaultWeak()
    {
        health = 10;
        maxHealth = 10;
    }

    [ContextMenu("Default: Normal")]
    public void SetHealthDefaultNormal()
    {
        health = 40;
        maxHealth = 40;
    }

    [ContextMenu("Default: Strong")]
    public void SetHealthDefaultStrong()
    {
        health = 150;
        maxHealth = 150;
    }

}
