using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateBase : MonoBehaviour
{
    // Data Structures //
    protected struct ObstacleData
    {
        public Vector3 closestPoint;
        public float distance;
    }


    // DATA //
    // Cached Data
    protected Dictionary<Collider, ObstacleData> obstacleDataTable = new Dictionary<Collider, ObstacleData>();
    protected Dictionary<CharmBehaviour, float> charmDataTable = new Dictionary<CharmBehaviour, float>();

    // Constants
    public static readonly float POINT_DISTANCE_DIVISOR = 20f;
    public static readonly float LOWEST_ALLOWED_ATTRACTIVENESS = -0.2f;
    public static readonly float IMMEDIATE_ACCEPT_ATTRACTIVENESS = 0.5f;
    public static readonly float MIN_DIST_FROM_AVOIDED_COLLIDER = 5;


    // FUNCTIONS //
    // Checking
    public virtual bool CheckStateConditions(StateBasedAI entity)
    {
        // Attempts transitioning to this state, returns true if it should switch to this as active state.
        Debug.LogError("[AIStateBase] Running base CheckStateConditions!");
        return false;
    }

    public virtual float CheckMoveDirAttractiveness(Vector3 direction, StateBasedAI entity)
    {
        return GetColliderAvoidanceAttractiveness(direction, entity) * -1;
    }

    public float CheckMoveDirAttractivenessOnlyCharms(Vector3 direction, StateBasedAI entity)
    {
        float charmWeight = GetCharmAvoidanceAttractiveness(direction, entity) * 0.5f;
        float lastDirectionWeight = Vector3.Dot(direction, entity.lastMovementDirection.moveDirection) * 0.3f;
        float avoidanceWeight = GetColliderAvoidanceAttractiveness(direction, entity) * 0.2f;

        return charmWeight + lastDirectionWeight + avoidanceWeight;
    }

    protected float GetColliderAvoidanceAttractiveness(Vector3 dir, StateBasedAI entity)
    {
        // By default moves away from nearby obstacles
        float returnVal = 0.0f;
        float multiplier = 1.0f / obstacleDataTable.Keys.Count;
        Vector3 pos = entity.transform.position;

        foreach (Collider collider in obstacleDataTable.Keys)
        {
            Vector3 point = obstacleDataTable[collider].closestPoint;
            float distanceToPoint = obstacleDataTable[collider].distance;
            returnVal += GameUtility.DirAttractToPoint(dir, pos, point, distanceToPoint, entity.colliderAvoidanceDistance, 1, entity.movementDamping);
        }

        return returnVal*multiplier;
    }

    protected float GetCharmAvoidanceAttractiveness(Vector3 dir, StateBasedAI entity)
    {
        // By default moves away from nearby obstacles
        float returnVal = 0.0f;
        float multiplier = 1.0f / charmDataTable.Keys.Count;
        Vector3 pos = entity.transform.position;

        foreach (CharmBehaviour charm in charmDataTable.Keys)
        {
            // Modifies optimal distance according to whether charm is attractor/repulsor
            float optimalCharmDistance = 0.5f;
            if (!charm.attractor) 
            {
                optimalCharmDistance = charm.distance;
            }

            // Gets charm point, distance, and calculates the attraction to the point
            Vector3 point = charm.transform.position;
            float distanceToPoint = charmDataTable[charm];
            returnVal += GameUtility.DirAttractToPoint(dir, pos, point, distanceToPoint, optimalCharmDistance, 1, entity.movementDamping);
        }

        return returnVal * multiplier;
    }

    protected bool AttemptMovement(StateBasedAI entity, bool lookInDirection)
    {
        // Generates tuples of most attractive directions
        (Vector3 direction, float attractiveness) mostAttractive = (GameUtility.nullVector3, 0);

        // Stores distance from current position to each obstacle
        obstacleDataTable.Clear();

        foreach(Collider collider in entity.collidersToAvoid)
        {
            if(collider == null)
            {
                continue;
            }

            ObstacleData data = new ObstacleData();
            data.closestPoint = collider.ClosestPointOnBounds(entity.transform.position);
            data.distance = Vector3.Distance(entity.transform.position, data.closestPoint);
            obstacleDataTable.Add(collider, data);
        }

        // Stores distance from current position to each charm
        charmDataTable.Clear();

        foreach (CharmBehaviour charm in entity.charmEffects)
        {
            if (charm == null)
            {
                continue;
            }

            float distance = Vector3.Distance(entity.transform.position, charm.transform.position);
            charmDataTable.Add(charm, distance);
        }

        foreach (Vector3 direction in entity.movementDirections)
        {
            float attractiveness = 0;

            // If there are no nearby charms, moves normally. If there are any at all, ignores everything except them.
            if (charmDataTable.Keys.Count == 0)
            {
                attractiveness = CheckMoveDirAttractiveness(direction, entity);
            }

            else
            {
                attractiveness = CheckMoveDirAttractivenessOnlyCharms(direction, entity);
            }

            // Checks if attractiveness beats best attractiveness
            if (mostAttractive.attractiveness < attractiveness)
            {
                mostAttractive.direction = direction;
                mostAttractive.attractiveness = attractiveness;
            }

            if (mostAttractive.attractiveness >= IMMEDIATE_ACCEPT_ATTRACTIVENESS)
            {
                break;
            }

            Debug.DrawLine(entity.transform.position, direction + entity.transform.position, Color.HSVToRGB(Mathf.Clamp01(attractiveness), 100, 100), Time.deltaTime);
        }

        if (mostAttractive.direction != GameUtility.nullVector3 && mostAttractive.attractiveness > LOWEST_ALLOWED_ATTRACTIVENESS)
        {
            entity.MoveInDirection(mostAttractive.direction, lookInDirection, Time.deltaTime);
            return true;
        }

        else
        {
            return false;
        }
    }


    // Standard Calculations
    public static float CalcDirAttractRefPoint(Vector3 position, Vector3 direction, Vector3 refPoint, bool goToward)
    {
        return 0.1f;
    }


    // Behaviour Functions
    public virtual void RunStateFunctionality(StateBasedAI entity)
    {
        // Runs the State Behaviour
        UnityEngine.Debug.LogError("[AIStateBase] Running base RunStateFunctionality!");
    }
    
}
