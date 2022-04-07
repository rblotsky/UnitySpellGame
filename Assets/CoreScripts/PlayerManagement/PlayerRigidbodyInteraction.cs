using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRigidbodyInteraction : MonoBehaviour
{
    // This script pushes all rigidbodies that the character controller touches
    public float mass = 5f;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody hitBody = hit.collider.attachedRigidbody;
        Vector3 force;

        // Does nothing if there is no rigidbody or it's kinematic
        if (hitBody == null || hitBody.isKinematic)
        { 
            return; 
        }

        // If the object is below, uses gravity to push it
        if (hit.moveDirection.y < -0.3)
        {
            force = new Vector3(0, -0.5f, 0);
            force = force * Physics.gravity.magnitude * mass;
        }

        // Otherwise, uses velocity
        else
        {
            force = hit.controller.velocity * mass;
        }

        // Apply the push
        hitBody.AddForceAtPosition(force, hit.point, ForceMode.Force);
    }
}
