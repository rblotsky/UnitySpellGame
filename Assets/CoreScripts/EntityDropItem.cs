using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDropItem : MonoBehaviour
{
    // STORED DATA //
    public Usable[] items;


    // FUNCTIONS //
    // Basic Functions
    private void OnEnable()
    {
        // Adds events
        CombatEntity entityReference = GetComponent<CombatEntity>();

        if(entityReference != null)
        {
            entityReference.onDeath += DropItems;
        }
    }

    private void OnDisable()
    {
        // Removes events
        CombatEntity entityReference = GetComponent<CombatEntity>();

        if (entityReference != null)
        {
            entityReference.onDeath -= DropItems;
        }
    }


    // Dropping Item
    public void DropItems()
    {
        Debug.Log("Dropping items!");

        foreach (Usable item in items)
        {
            item.DropItem(transform.position);
        }
    }
}
