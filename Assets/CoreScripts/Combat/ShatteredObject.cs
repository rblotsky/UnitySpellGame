using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatteredObject : MonoBehaviour
{
    // OBJECT DATA //
    // References
    public Rigidbody[] shatterFragments;

    // Instance Data
    public bool variableSize;


    // FUNCTIONS //
    // Fragment Setup
    public void SetFragmentSize(Vector3 rootScale)
    {
        // If size is variable, modifies by root size
        if (variableSize)
        {
            foreach (Rigidbody fragment in shatterFragments)
            {
                fragment.transform.localScale = Vector3.Scale(fragment.transform.localScale, rootScale);
            }
        }
    }


    // Physics Application
    public void ApplyForceToFragments(Vector3 velocity)
    {
        // Adds the force to each fragment individually
        foreach(Rigidbody fragment in shatterFragments)
        {
            fragment.AddForce(velocity, ForceMode.VelocityChange);
        }
    }

    
    // Fragment Operation
    public virtual IEnumerator RunShatterFragments(float removalDelay)
    {
        // Something to make them fall instead of retaining original shape


        // Waits and then removes them all (removes noCollisionDelay since that time has already been waited)
        yield return new WaitForSeconds(removalDelay);

        // Removes all fragments
        Destroy(gameObject);
    }
}
