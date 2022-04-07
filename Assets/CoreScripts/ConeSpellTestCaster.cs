using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeSpellTestCaster : MonoBehaviour
{
    [Header("Used in actual spell")]
    public float range;
    public float spreadAngle;
    public float castTime = 1.5f;
    public ParticleSystem particles;
    [Header("Debug")]
    public int numIncrements = 50;


    [ContextMenu("Cast Spell")]
    public void CastSpell()
    {
        StartCoroutine(SpellCoroutine());
    }

    public IEnumerator SpellCoroutine()
    {
        // Collects affected colliders
        List<CombatEntity> affectedEntities = new List<CombatEntity>();

        // Gets increment distance and delay
        float incrementDistance = range/numIncrements;
        float incrementDelay = castTime / numIncrements;
        Debug.Log("Num: " + numIncrements + " Dist: " + incrementDistance + " Delay: " + incrementDelay);

        // Starts particles
        particles.Play();
        particles.transform.position = transform.position;
        particles.transform.LookAt(transform.position + transform.forward * range);

        // Runs increments
        for (int i = 0; i < numIncrements; i++)
        {
            float cosAngle = Mathf.Cos(spreadAngle * Mathf.Deg2Rad);

            // Draws ray animations
            Vector3 location1 = transform.position + (Quaternion.AngleAxis(spreadAngle, Vector3.up) * (transform.forward * ((incrementDistance * i) / cosAngle)));
            Vector3 location2 = transform.position + (Quaternion.AngleAxis(-spreadAngle, Vector3.up) * (transform.forward * ((incrementDistance * i) / cosAngle)));
            Vector3 centralLocation = transform.position + transform.forward*incrementDistance*i;

            Debug.DrawLine(transform.position, transform.position + transform.forward * (range * Mathf.Cos(spreadAngle)), Color.yellow, 2f);
            Debug.DrawLine(transform.position, location1, Color.cyan, incrementDelay);
            Debug.DrawLine(transform.position, location2, Color.cyan, incrementDelay);
            Debug.DrawLine(location1, location2, Color.red, incrementDelay);

            // Displays particles
            ParticleSystem.ShapeModule shapeModule = particles.shape;
            shapeModule.radius = Vector3.Distance(location1,location2)*0.5f;
            particles.transform.position = centralLocation;

            // Gets hit CombatEntities and applies damage to them
            List<RaycastHit> allHits = new List<RaycastHit>();
            allHits.AddRange(Physics.RaycastAll(location1, centralLocation - location1, Vector3.Distance(location1, centralLocation)));
            allHits.AddRange(Physics.RaycastAll(location2, centralLocation - location2, Vector3.Distance(location1, centralLocation)));

            foreach(RaycastHit hit in allHits)
            {
                CombatEntity cb = hit.collider.GetComponent<CombatEntity>();
                if(cb != null)
                {
                    if (!affectedEntities.Contains(cb))
                    {
                        affectedEntities.Add(cb);
                        cb.ApplyEffects(new CombatEffects(1, 1, 1, 1), transform.position, false, cb);
                    }
                }
            }

            yield return new WaitForSeconds(incrementDelay);
        }

        Debug.DrawLine(transform.position, transform.position + (transform.forward * range), Color.cyan, 1);

        // Stops particles
        particles.Stop();
        particles.Clear();

        yield return null;
    }
}
