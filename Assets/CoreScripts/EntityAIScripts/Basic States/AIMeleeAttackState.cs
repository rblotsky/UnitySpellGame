using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMeleeAttackState : AIStateBase
{
    // DATA //
    // Combat Data
    public float minPlayerOffset = 0.5f;
    public float attkRange = 1.5f;
    public float attackDelay = 5f;
    public CombatEffects attackEffects;

    // Cached Data
    private float lastAttackedTime = -10f;


    // OVERRIDES //
    public override bool CheckStateConditions(StateBasedAI entity)
    {
        // Can only enter this state if player is visible
        return entity.canSeePlayerCached && Time.time-lastAttackedTime >= attackDelay;
    }

    public override void RunStateFunctionality(StateBasedAI entity)
    {
        // Gets player position
        Vector3 playerPosition = entity.lastKnownPlayerLocation;

        // Attacks player if in range and didn't attack recently (To avoid attacking like 20 times before it has time to switch state)
        if (Time.time - lastAttackedTime >= attackDelay)
        {
            if ((playerPosition - entity.transform.position).sqrMagnitude <= attkRange * attkRange)
            {
                // Attacks
                if(DataRef.playerCombatEntity == null)
                {
                    Debug.LogError("PLAYER COMBATENTITY NULL");
                }
                if (entity.transform == null)
                {
                    Debug.LogError("ENTITY TRANSFORM NULL");
                }
                if (entity.combatEntity == null)
                {
                    Debug.LogError("ENTITY COMBATENTITY NULL");
                }

                DataRef.playerCombatEntity.ApplyEffects(attackEffects, entity.transform.position, false, entity.combatEntity);
                lastAttackedTime = Time.time;
            }
        }
        

        // Rotates and moves
        entity.entityRotator.StartNewLookAtPointRotation(playerPosition);
        AttemptMovement(entity, false);
    }

    public override float CheckMoveDirAttractiveness(Vector3 direction, StateBasedAI entity)
    {
        // Goes toward within minDistance of player
        float playerLocationWeight = GameUtility.DirAttractToPoint(direction, entity.transform.position, entity.lastKnownPlayerLocation, minPlayerOffset, 1) * 0.3f;
        float lastDirectionWeight = Vector3.Dot(direction, entity.lastMovementDirection.moveDirection) * 0.3f;
        float avoidanceWeight = GetColliderAvoidanceAttractiveness(direction, entity) * 0.4f;

        return playerLocationWeight + lastDirectionWeight + avoidanceWeight;
    }
}
