using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityCallEventComponent : MonoBehaviour
{
    // DATA //
    public delegate void ColliderDelegate(Collider collider, UnityCallEventComponent mainObject);
    public delegate void CollisionDelegate(Collision collision, UnityCallEventComponent mainObject);
    public delegate void MainObjectDelegate(UnityCallEventComponent mainObject);
    public event ColliderDelegate onTriggerEnter;
    public event ColliderDelegate onTriggerExit;
    public event CollisionDelegate onColliderEnter;
    public event CollisionDelegate onColliderExit;
    public event ColliderDelegate onTriggerStay;
    public event CollisionDelegate onColliderStay;
    public event MainObjectDelegate onUpdate;
    public event MainObjectDelegate onEnable;
    public event MainObjectDelegate onDisable;
    public event MainObjectDelegate onDestroy;


    // FUNCTIONS //
    private void OnTriggerEnter(Collider other)
    {
        if(onTriggerEnter != null)
        {
            onTriggerEnter(other, this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (onTriggerExit != null)
        {
            onTriggerExit(other, this);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (onTriggerStay != null)
        {
            onTriggerStay(other, this);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (onColliderEnter != null)
        {
            onColliderEnter(collision, this);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (onColliderExit != null)
        {
            onColliderExit(collision, this);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (onColliderStay != null)
        {
            onColliderStay(collision, this);
        }
    }

    private void OnEnable()
    {
        if (onEnable != null)
        {
            onEnable(this);
        }
    }

    private void OnDisable()
    {
        if (onDisable != null)
        {
            onDisable(this);
        }
    }

    private void OnDestroy()
    {
        if (onDestroy != null)
        {
            onDestroy(this);
        }
    }

    private void Update()
    {
        if(onUpdate != null)
        {
            onUpdate(this);
        }
    }
}
