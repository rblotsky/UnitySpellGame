using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Newly Instantiated Explosive Keyword Rename This Using Keyword_X Format", menuName = "Spells/Keywords/Explosive")]
public class KeywordExplosive : SpellKeyword
{
    // DATA //
    // References
    public GameObject explosionParticles;
    public float explosionRadius = 1f;

    // Singleton Pattern
    public static KeywordExplosive singleton;


    // OVERRIDES //
    public override void AddEvents(SpellController controllerToAddTo)
    {
        controllerToAddTo.onSpellDamageDealtBeforeKeywords += RunExplosion;
    }


    // FUNCTIONS //
    // Basic Functions
    private void Awake()
    {
        if(singleton == null)
        {
            singleton = this;
        }

        else if(singleton != this)
        {
            Debug.LogError("\"" + name + "\" has found an existing singleton SpellKeyword of the same type!");
            Debug.Break();
        }
    }

    
    // Functionality
    public void RunExplosion(SpellController controllerUsed, CombatEntity damagedEntity, Vector3 damagePosition)
    {
        Debug.Log("Running Explosion Effect! DamagedEntity: " + damagedEntity.name);

        // Runs explosion
        GameUtility.BasicPhysicsExplosion(damagePosition, explosionRadius, controllerUsed.properties.elements, controllerUsed.properties.damagedEntities, controllerUsed.spellObject);

        // Instantiates and runs particle effects
        ParticleSystem particles = Instantiate(explosionParticles, damagePosition, explosionParticles.transform.rotation).GetComponent<ParticleSystem>();

        ParticleSystem.ShapeModule newShape = particles.shape;
        newShape.radius = explosionRadius;
        particles.Play();
    }
}
