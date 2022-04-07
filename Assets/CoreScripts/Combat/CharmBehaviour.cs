using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharmBehaviour : MonoBehaviour
{
    // DATA //
    // References
    public ParticleSystem attractorParticles;
    public ParticleSystem repulsorParticles;

    // Basic Data
    public bool attractor = true;
    public float distance = 1f;
    public float duration = 2f;

    // Static Data
    public static List<CharmBehaviour> allCharms = new List<CharmBehaviour>();

    // Cached Data
    private float startTime = 0f;


    // FUNCTIONS //
    // Basic Functions
    private void Update()
    {
        if(Time.time-startTime > duration)
        {
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        // Removes itself from the list
        allCharms.Remove(this);
    }


    // Management
    public void ActivateCharm()
    {
        // Sets up data
        startTime = Time.time;

        // Starts particles
        if (attractor)
        {
            attractorParticles.Play();
            ParticleSystem.ShapeModule shape = attractorParticles.shape;
            ParticleSystem.EmissionModule emission = attractorParticles.emission;
            shape.radius = distance;
            ParticleSystem.MinMaxCurve rate = emission.rateOverTime;
            rate.constant = rate.constant * distance;
        }

        else
        {
            repulsorParticles.Play();
            ParticleSystem.ShapeModule shape = repulsorParticles.shape;
            ParticleSystem.EmissionModule emission = repulsorParticles.emission;
            shape.radius = distance;
            ParticleSystem.MinMaxCurve rate = emission.rateOverTime;
            rate.constant = rate.constant * distance;
        }

        // Adds itself to the main list
        allCharms.Add(this);
    }
}
