using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBasicLookForPlayerState : AIStateBase
{
    // DATA //
    // Look Rotation Data
    public float lookAroundClockwise = 30;
    public float lookAroundCounterClockwise = 30;


    // OVERRIDES //
    public override bool CheckStateConditions(StateBasedAI entity)
    {
        // If AI hasn't forgot seeing player, and doesn't see them, it can enter this state
        return !entity.ForgotPlayerSighting() && !entity.canSeePlayerCached;
    }

    public override void RunStateFunctionality(StateBasedAI entity)
    {
        // Walks to last known player location, then looks around
        // If not at last known player location, goes there
        if (!entity.AtPosition(entity.lastKnownPlayerLocation))
        {
            AttemptMovement(entity, true);
        }
    }

    public override float CheckMoveDirAttractiveness(Vector3 direction, StateBasedAI entity)
    {
        // Moving toward last player location is attractive, moving away not attractive.
        //TODO: Have this potentially generate a path toward the player and follow the navpath if the location is too far away?

        float playerLocationWeight = GameUtility.DirAttractToPoint(direction, entity.transform.position, entity.lastKnownPlayerLocation, 0, 1)* 0.3f;
        float lastDirectionWeight = Vector3.Dot(direction, entity.lastMovementDirection.moveDirection)* 0.3f;
        float avoidanceWeight = GetColliderAvoidanceAttractiveness(direction, entity) * 0.4f;

        return playerLocationWeight + lastDirectionWeight + avoidanceWeight;
    }
}
