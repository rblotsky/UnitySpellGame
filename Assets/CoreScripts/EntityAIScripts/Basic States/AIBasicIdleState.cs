using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBasicIdleState : AIStateBase
{
    // OVERRIDES //
    public override bool CheckStateConditions(StateBasedAI entity)
    {
        // Can always idle
        return true;
    }

    public override void RunStateFunctionality(StateBasedAI entity)
    {
        // Moves around randomly
        //AttemptMovement(entity, true);
    }

    public override float CheckMoveDirAttractiveness(Vector3 direction, StateBasedAI entity)
    {
        return 0;
    }

}
