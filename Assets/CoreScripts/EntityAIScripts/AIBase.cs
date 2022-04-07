using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIBase : MonoBehaviour
{
    // DATA //
    // References
    [Header("AIBase References")]
    public NavMeshAgent navAgent;
    public CombatEntity combatEntity;
    public BaseAIPerspective perspective;
    public EntityRotate entityRotator;
    public CharacterController movementController;

    // Basic Data
    [Header("AIBase Data")]
    public int totalMovementDirections = 8;
    public float entityMoveSpeed = 2f;
    public float avoidColliderScanRadius = 7f;

    // Constants
    public static readonly float POSITION_REACHED_DISTANCE = 1f;
    public static readonly float SCAN_INTERVAL = 0.5f;
    public static readonly string[] AVOID_COLLIDER_TAGS = new string[2] { "CombatEntity", "Obstacle", };
    public static readonly float CHARM_DISTANCE_DAMPING = 1.4f;

    // Cached Data
    public List<CharmBehaviour> charmEffects = new List<CharmBehaviour>();
    public List<Collider> collidersToAvoid = new List<Collider>();


    // FUNCTIONS //
    // Basic Functions
    protected virtual void Awake()
    {
        //Nothing in base class
    }

    protected virtual void OnEnable()
    {
        StartCoroutine(ColliderAvoidanceScanner());
    }

    protected virtual void OnDisable()
    {
        StopAllCoroutines();
    }


    // Management
    public bool AtPosition(Vector3 position)
    {
        // Returns true if given position is nullVector3
        if (position == GameUtility.nullVector3)
        {
            return true;
        }

        // Otherwise, returns whether it's within the position reached distance
        return (position - transform.position).sqrMagnitude <= POSITION_REACHED_DISTANCE * POSITION_REACHED_DISTANCE; ;
    }

    public void ModifyCharm(CharmBehaviour charm, bool addOrRemove)
    {
        // Adds/Removes charm. NOTE: This should not check if it is present, all charms should remove themselves when they are disabled.
        if (addOrRemove)
        {
            if (!charmEffects.Contains(charm))
            {
                charmEffects.Add(charm);
            }
        }

        else
        {
            if (charmEffects.Contains(charm))
            {
                charmEffects.Remove(charm);
            }
        }
    }

    public IEnumerator ColliderAvoidanceScanner()
    {
        while (enabled)
        {
            // Colliders
            collidersToAvoid.Clear();
            Collider[] foundColliders = Physics.OverlapSphere(transform.position, avoidColliderScanRadius);

            foreach (Collider collider in foundColliders)
            {
                bool passed = false;

                foreach (string tag in AVOID_COLLIDER_TAGS)
                {
                    if (collider.CompareTag(tag))
                    {
                        passed = true;
                        break;
                    }
                }

                if (passed)
                {
                    collidersToAvoid.Add(collider);
                }
            }

            // Charms
            charmEffects.Clear();

            foreach(CharmBehaviour charm in CharmBehaviour.allCharms)
            {
                if(Vector3.Distance(transform.position, charm.transform.position) <= charm.distance*CHARM_DISTANCE_DAMPING)
                {
                    charmEffects.Add(charm);
                }
            }

            yield return new WaitForSeconds(SCAN_INTERVAL);
        }
    }
}
