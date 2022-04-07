using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeSpellController : SpellController
{
    // DATA //
    // Basic Data
    public int numIncrements;
    public ParticleSystem particles;
    public float spreadAngle;


    // FUNCTIONS //
    // Main Function
    public IEnumerator RunSpell()
    {
        // Gets properties
        PlayerSpellProperties properties = spellObject.GetSpellPropertiesWithModifiers();

        // Prevents player from casting spells
        PlayerStats.canPlayerCastSpell = false;

        // Collects affected colliders
        List<CombatEntity> affectedEntities = new List<CombatEntity>();

        // Gets data
        float incrementDistance = properties.radius / numIncrements;
        float incrementDelay = properties.duration / numIncrements;
        Vector3 startPos = entity.transform.position;
        Vector3 forward = entity.transform.forward;

        // Starts particles
        if (particles != null)
        {
            particles.Play();
            particles.transform.position = startPos;
            particles.transform.LookAt(startPos + forward * properties.radius);
        }

        // Runs increments
        for (int i = 0; i < numIncrements; i++)
        {
            float cosAngle = Mathf.Cos(spreadAngle * Mathf.Deg2Rad);
            float tanAngle = Mathf.Tan(spreadAngle * Mathf.Deg2Rad);

            // Draws ray animations
            Vector3 location1 = startPos + (Quaternion.AngleAxis(spreadAngle, Vector3.up) * (forward * ((incrementDistance * i) / cosAngle)));
            Vector3 location2 = startPos + (Quaternion.AngleAxis(-spreadAngle, Vector3.up) * (forward * ((incrementDistance * i) / cosAngle)));
            Vector3 centralLocation = startPos + forward * incrementDistance * i;

            //Debug.DrawLine(startPos, startPos + forward * (properties.radius * Mathf.Cos(spreadAngle)), Color.yellow, 2f);
            //Debug.DrawLine(startPos, location1, Color.cyan, incrementDelay);
            //Debug.DrawLine(startPos, location2, Color.cyan, incrementDelay);
            //Debug.DrawLine(location1, location2, Color.red, incrementDelay);

            // Displays particles
            if (particles != null)
            {
                ParticleSystem.ShapeModule shapeModule = particles.shape;
                shapeModule.radius = incrementDistance * i * tanAngle;
                particles.transform.position = centralLocation;
            }

            // Gets hit CombatEntities and applies damage to them
            Ray rayToCast = new Ray(location1, location2 - location1);
            RaycastHit[] allHits = Physics.RaycastAll(rayToCast, i * incrementDistance * tanAngle * 2);
            //Debug.DrawRay(location1, (location2 - location1).normalized * (i * incrementDistance * tanAngle * 2), Color.green, 1f);

            foreach (RaycastHit hit in allHits)
            {
                CombatEntity cb = hit.collider.GetComponent<CombatEntity>();
                if (cb != null)
                {
                    if (!affectedEntities.Contains(cb))
                    {
                        affectedEntities.Add(cb);
                        if (cb.ApplyEffects(properties.elements, startPos, properties.damagedEntities, false, cb))
                        {
                            if (onSpellDamageDealtBeforeKeywords != null)
                            {
                                onSpellDamageDealtBeforeKeywords(this, cb, startPos);
                            }
                        }
                    }
                }
            }

            yield return new WaitForSeconds(incrementDelay);
        }

        Debug.DrawLine(startPos, startPos + (forward * properties.radius), Color.cyan, 1);

        // Stops particles
        if (particles != null)
        {
            particles.Stop();
        }

        // Finishes controller
        FinishController(entity);
    }
}
