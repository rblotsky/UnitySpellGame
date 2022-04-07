using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyVelocityModifier : MonoBehaviour
{
    // DATA //
    public float speedModifier;
    public Rigidbody attachedRigidbody;
    public bool shouldApplyChanges = true;

    // Cached Data
    private float lastFrameModifier = 1;


    // FUNCTIONS //
    // Basic Functions
    private void Awake()
    {
        attachedRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if(attachedRigidbody != null)
        {
            Vector3 currentVelocity = attachedRigidbody.velocity;
            Vector3 velocityWithoutMod = currentVelocity;

            if (lastFrameModifier != 0)
            {
                velocityWithoutMod = currentVelocity / lastFrameModifier;
            }

            if (shouldApplyChanges)
            {
                attachedRigidbody.velocity = velocityWithoutMod * speedModifier;
                lastFrameModifier = speedModifier;
            }
        }
    }


    // Management Functions
    public void SetModifier(float percentModifier)
    {
        speedModifier = 1 + (percentModifier / 100.000f);
    }
}
