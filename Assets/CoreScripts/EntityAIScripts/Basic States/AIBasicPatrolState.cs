using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBasicPatrolState : AIStateBase
{
    // DATA //
    // Patrol Data
    public Transform[] patrolPoints;

    // Cached Data
    private int currentPatrolDestination;


    // OVERRIDES //
    public override bool CheckStateConditions(StateBasedAI entity)
    {
        // Can always patrol
        return true;
    }

    public override void RunStateFunctionality(StateBasedAI entity)
    {
        // If it's reached its current patrol point, set to next
        if (entity.AtPosition(GetCurrentPatrolLocation()))
        {
            currentPatrolDestination++;
        }

        // Moves toward current patrol destination
        
    }


    // NEW FUNCTIONS //
    protected void ClampPatrolIndexToArray()
    {
        // Clamps patrol destination index to possible indices
        currentPatrolDestination = Mathf.Clamp(currentPatrolDestination, 0, patrolPoints.Length - 1);
    }

    protected Vector3 GetCurrentPatrolLocation()
    {
        // If there are no patrol points, returns nullVector3
        if(patrolPoints.Length < 1)
        {
            return GameUtility.nullVector3;
        }

        // If the patrol location is past the maximum, sets to 0
        if(currentPatrolDestination >= patrolPoints.Length)
        {
            currentPatrolDestination = 0;
        }

        // Clamps patrol location to the array
        ClampPatrolIndexToArray();

        // Returns the patrol point
        return patrolPoints[currentPatrolDestination].position;
    }

}
