using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpellController : SpellController
{
    // DATA //
    // References
    public Rigidbody attachedRB;

    // Cached
    private Vector3 fireDirectionNormalized;
    private Vector3 firedPosition;

    // Constants
    public static readonly float FLIGHT_SPEED_MULT = 100f;


    // OVERRIDES //
    // Controller Management
    public override void StartController(CombatEntity castingEntity)
    {
        // Adds force for movement, makes sure gravity/drag are disabled
        if (attachedRB != null)
        {
            attachedRB.AddForce(fireDirectionNormalized * properties.speed * FLIGHT_SPEED_MULT, ForceMode.Acceleration);
        }

        // Caches Data
        firedPosition = transform.position;
        fireDirectionNormalized = castingEntity.transform.forward;

        // Sets rotation
        transform.LookAt(transform.position + fireDirectionNormalized);

        // Runs base start
        base.StartController(castingEntity);
    }


    // FUNCTIONS //
    // Basic Functionality
    private void Update()
    {
        // Stops controller if went past radius
        if((transform.position-firedPosition).sqrMagnitude > properties.radius * properties.radius)
        {
            FinishController(entity);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If collided with a combatentity, damages it and also runs finishcontroller
        CombatEntity collisionCombatEntity = collision.collider.GetComponent<CombatEntity>();

        if(collisionCombatEntity != null)
        {
            if (collisionCombatEntity.ApplyEffects(properties.elements, transform.position, properties.damagedEntities, false, spellObject))
            {

                if (onSpellDamageDealtBeforeKeywords != null)
                {
                    onSpellDamageDealtBeforeKeywords(this, collisionCombatEntity, transform.position);
                }

                FinishController(entity);
            }
        }
    }

    
    // Controller Management
    public void SetMovementInfo(Vector3 direction)
    {
        fireDirectionNormalized = direction.normalized;
    }
}
