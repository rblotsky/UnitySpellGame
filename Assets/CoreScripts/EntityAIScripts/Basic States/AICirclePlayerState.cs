using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICirclePlayerState : AIStateBase
{
    // DATA //
    // Combat Data
    public float minPlayerOffset = 3;


    // OVERRIDES //
    public override bool CheckStateConditions(StateBasedAI entity)
    {
        // Can only enter this state if player is visible
        return entity.canSeePlayerCached;
    }

    public override void RunStateFunctionality(StateBasedAI entity)
    {
        Vector3 playerPosition = entity.lastKnownPlayerLocation;

        // Sets rotator to look at player
        entity.entityRotator.StartNewLookAtPointRotation(playerPosition);

        //TODO: When moving in for an attack, lowers minOffset and then increases back when finishes attacking
        /* Absolutely no clue how to implement this.
         */

        AttemptMovement(entity, false);
    }

    public override float CheckMoveDirAttractiveness(Vector3 direction, StateBasedAI entity)
    {
        // Goes toward within minDistance of player
        float playerLocationWeight = GameUtility.DirAttractToPoint(direction, entity.transform.position, entity.lastKnownPlayerLocation, minPlayerOffset, 1, entity.movementDamping) * 0.3f;
        float lastDirectionWeight = Vector3.Dot(direction, entity.lastMovementDirection.moveDirection) * 0.3f;
        float avoidanceWeight = GetColliderAvoidanceAttractiveness(direction, entity) * 0.4f;

        return playerLocationWeight + lastDirectionWeight + avoidanceWeight;
    }
}
