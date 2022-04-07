using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntensityVaryingLight : MonoBehaviour
{
    // DATA //
    public Light referencedLight;
    public float defaultIntensity;
    public float variationFactor;


    // FUNCTIONS //
    // Basic Functions
    private void Update()
    {
        // Uses perlin noise to make light flicker
        float noise = Mathf.PerlinNoise(Time.time, Time.deltaTime);
        referencedLight.intensity = defaultIntensity + (noise-0.5f) * variationFactor;
    }
}
