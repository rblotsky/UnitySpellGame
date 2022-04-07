using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIConeSpellState : AIStateBase
{
    // DATA //
    // References
    [Header("References")]
    public GameObject particlesPrefab;

    // Spell Data
    [Header("Spell Data")]
    public float range = 8f;
    public float spreadAngle = 30f;
    public float castTime = 1.5f;
    public CombatEffects attackEffects;
    public DamageType[] damageTypes = new DamageType[2] { DamageType.Mobs, DamageType.Player };
    public int numIncrements = 50;

    // Extra Data
    [Header("Extras")]
    public float desiredPlayerOffset = 3f;
    public float attackDelay = 6f;

    // Cached Data
    private float lastAttackInitiated = -10f;


    // OVERRIDES //
    public override bool CheckStateConditions(StateBasedAI entity)
    {
        // Can only enter this state if player is visible
        return entity.canSeePlayerCached && Time.time - lastAttackInitiated >= attackDelay+castTime;
    }

    public override void RunStateFunctionality(StateBasedAI entity)
    {
        // Gets player position
        Vector3 playerPosition = entity.lastKnownPlayerLocation;

        // Attacks player if in range and didn't already attack recently
        if (Time.time - lastAttackInitiated >= attackDelay+castTime && (playerPosition - transform.position).sqrMagnitude < range * range)
        {
            StartCoroutine(CastConeAttack(entity));
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
    // Spell Coroutine
    protected IEnumerator CastConeAttack(StateBasedAI entity)
    {
        // Collects affected colliders
        List<CombatEntity> affectedEntities = new List<CombatEntity>();

        // Gets data
        float incrementDistance = range / numIncrements;
        float incrementDelay = castTime / numIncrements;
        Vector3 startPos = entity.transform.position;
        Vector3 forward = entity.transform.forward;

        // Starts particles
        GameObject particleObject = Instantiate(particlesPrefab, startPos, particlesPrefab.transform.rotation);
        ParticleSystem particles = null;
        if (particleObject != null)
        {
            particles = particleObject.GetComponent<ParticleSystem>();
        }

        if (particles != null)
        {
            particles.Play();
            particles.transform.position = startPos;
            particles.transform.LookAt(startPos + forward * range);
        }

        // Caches cast time
        lastAttackInitiated = Time.time;

        // Runs increments
        for (int i = 0; i < numIncrements; i++)
        {
            float cosAngle = Mathf.Cos(spreadAngle * Mathf.Deg2Rad);
            float tanAngle = Mathf.Tan(spreadAngle * Mathf.Deg2Rad);

            // Draws ray animations
            Vector3 location1 = startPos + (Quaternion.AngleAxis(spreadAngle, Vector3.up) * (forward * ((incrementDistance * i) / cosAngle)));
            Vector3 location2 = startPos + (Quaternion.AngleAxis(-spreadAngle, Vector3.up) * (forward * ((incrementDistance * i) / cosAngle)));
            Vector3 centralLocation = startPos + forward * incrementDistance * i;

            Debug.DrawLine(startPos, startPos + forward * (range * Mathf.Cos(spreadAngle)), Color.yellow, 2f);
            Debug.DrawLine(startPos, location1, Color.cyan, incrementDelay);
            Debug.DrawLine(startPos, location2, Color.cyan, incrementDelay);
            Debug.DrawLine(location1, location2, Color.red, incrementDelay);

            // Displays particles
            if (particles != null)
            {
                ParticleSystem.ShapeModule shapeModule = particles.shape;
                shapeModule.radius = incrementDistance * i * tanAngle;
                particles.transform.position = centralLocation;
            }

            // Gets hit CombatEntities and applies damage to them
            Ray rayToCast = new Ray(location1, location2 - location1);
            RaycastHit[] allHits = Physics.RaycastAll(rayToCast, i*incrementDistance*tanAngle*2);
            Debug.DrawRay(location1, (location2 - location1).normalized*(i*incrementDistance*tanAngle*2), Color.green, 1f);

            foreach (RaycastHit hit in allHits)
            {
                CombatEntity cb = hit.collider.GetComponent<CombatEntity>();
                if (cb != null)
                {
                    if (!affectedEntities.Contains(cb))
                    {
                        affectedEntities.Add(cb);
                        cb.ApplyEffects(attackEffects, startPos, damageTypes, false, cb);
                    }
                }
            }

            yield return new WaitForSeconds(incrementDelay);
        }

        Debug.DrawLine(startPos, startPos + (forward * range), Color.cyan, 1);

        // Stops particles
        if (particles != null)
        {
            particles.Stop();
            Destroy(particleObject);
        }

        yield return null;
    }
}
