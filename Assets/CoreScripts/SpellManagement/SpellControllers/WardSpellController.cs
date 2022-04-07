using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WardSpellController : SpellController
{
    // CONTROLLER DATA //
    // Constants
    private static float DAMAGE_INTERVAL = 1;

    // References
    public CapsuleCollider wardCollider;
    public ParticleSystem wardParticles;
    public MeshRenderer spellRenderer;


    // FUNCTIONS //
    // Startup Functions
    public override void StartController(CombatEntity castingEntity)
    {
        // Sets position
        transform.position = new Vector3(castingEntity.transform.position.x, GameUtility.groundLevel, castingEntity.transform.position.z);
        // Modifies size
        Vector3 scale = transform.localScale;
        scale.x = properties.radius;
        scale.y = properties.radius;
        transform.localScale = scale;

        ParticleSystem.ShapeModule particleShape = wardParticles.shape;
        particleShape.radius = properties.radius;

        // Starts ward coroutine
        StartCoroutine(DamageInWard());

        // Runs base
        base.StartController(castingEntity);
    }


    // Running Functions
    protected virtual IEnumerator DamageInWard()
    {
        float startTime = Time.time;

        while(Time.time-startTime < properties.duration)
        {
            Collider[] affectedColliders = Physics.OverlapSphere(transform.position, properties.radius, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Collide);

            foreach (Collider collider in affectedColliders)
            {
                // Checks for null in case the collider has been deleted since it was found
                if(collider == null)
                {
                    continue;
                }

                // Checks if they are combatentities by getting component, then applying damage
                CombatEntity entity = collider.GetComponent<CombatEntity>();

                if (entity != null)
                {
                    if (entity.ApplyEffects(properties.elements, transform.position, properties.damagedEntities, false, spellObject))
                    {
                        if (onSpellDamageDealtBeforeKeywords != null)
                        {
                            onSpellDamageDealtBeforeKeywords(this, entity, entity.transform.position);
                        }
                    }
                }

                else
                {
                    continue;
                }

            }

            yield return new WaitForSeconds(DAMAGE_INTERVAL);
        }

        DestroyWard();
        yield break;
    }

    public void DestroyWard()
    {
        WardShapeComponent.currentWard = null;
        FinishController(entity);
    }
}


