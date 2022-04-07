using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ExplosiveObject : MonoBehaviour, IAttacker
{
    // OBJECT DATA //
    // References
    public MeshRenderer[] objRenderers;

    // Instance Data
    [Header("Explosion Info")]
    public float explosionRadius;
    public float objectDisableDelay = 2f;
    public CombatEffects explosionDamage;
    public DamageType[] explosionDamageTypes = new DamageType[1] { DamageType.All };
    public ParticleSystem explosionParticles;


    // FUNCTIONS //
    // Basic Functions
    private void Awake()
    {
        // Gets mesh renderers for child objects
        objRenderers = GetComponentsInChildren<MeshRenderer>();
    }


    // Main Functionality
    public void ExplodeObject()
    {
        StartCoroutine(Explode());
    }

    public IEnumerator Explode()
    {
        // Disables mesh renderers for all child objects (to make object invisible)
        foreach (MeshRenderer renderer in objRenderers)
        {
            renderer.enabled = false;
        }

        // Creates explosion particle effect
        if (explosionParticles != null)
        {
            explosionParticles.Play();
        }

        // Disables collider
        GetComponent<Collider>().enabled = false;

        // Runs a basic physics explosion at the object's location
        GameUtility.BasicPhysicsExplosion(transform.position, explosionRadius, explosionDamage, explosionDamageTypes, this);

        // Waits before disabling
        yield return new WaitForSeconds(objectDisableDelay);

        // Disables the object
        gameObject.SetActive(false);
    }
}
