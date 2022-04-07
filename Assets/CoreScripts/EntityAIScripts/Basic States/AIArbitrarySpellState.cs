using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIArbitrarySpellState : AIStateBase
{
    // DATA //
    // References
    [Header("Spell Data")]
    [Tooltip("Use injected properties to avoid creating unnecessary modifier components.")]
    public PlayerSpellObject spellObject;

    // Extra Data
    [Header("Extras")]
    public float desiredPlayerOffset = 3f;
    public float attackDelay = 6f;
    public float reqPlayerLookAngle = 20f;

    // Cached Data
    private float lastAttackInitiated = -10f;


    // OVERRIDES //
    public override bool CheckStateConditions(StateBasedAI entity)
    {
        // Can only enter this state if player is visible
        return entity.canSeePlayerCached && Time.time - lastAttackInitiated >= attackDelay;
    }

    public override void RunStateFunctionality(StateBasedAI entity)
    {
        // Gets player position
        Vector3 playerPosition = entity.lastKnownPlayerLocation;

        // Attacks player if in range and didn't already attack recently
        if (Time.time - lastAttackInitiated >= attackDelay && spellObject.shapeComponent.AICheckUseConditions(entity, playerPosition, spellObject.GetSpellPropertiesWithModifiers()))
        {
            // Also checks if look rotation at player is within the required look angle
            if (Quaternion.Angle(Quaternion.LookRotation(playerPosition - entity.transform.position), entity.transform.rotation) < reqPlayerLookAngle)
            {
                CastSpell(entity);
            }
        }

        // Rotates and moves
        entity.entityRotator.StartNewLookAtPointRotation(playerPosition);
        AttemptMovement(entity, false);
    }

    public override float CheckMoveDirAttractiveness(Vector3 direction, StateBasedAI entity)
    {
        // Goes toward within minDistance of player
        float playerLocationWeight = GameUtility.DirAttractToPoint(direction, entity.transform.position, entity.lastKnownPlayerLocation, desiredPlayerOffset, 1) * 0.3f;
        float lastDirectionWeight = Vector3.Dot(direction, entity.lastMovementDirection.moveDirection) * 0.3f;
        float avoidanceWeight = GetColliderAvoidanceAttractiveness(direction, entity) * 0.4f;

        return playerLocationWeight + lastDirectionWeight + avoidanceWeight;
    }


    // FUNCTIONS //
    // Spell Casting
    public void CastSpell(StateBasedAI entity)
    {
        if(spellObject != null)
        {
            DataRef.playerReference.StartCoroutine(spellObject.shapeComponent.UseComponent(entity.combatEntity,spellObject));
            lastAttackInitiated = Time.time;
        }
    }
}
