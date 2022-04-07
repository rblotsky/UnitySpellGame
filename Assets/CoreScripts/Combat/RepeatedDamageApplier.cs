using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatedDamageApplier : MonoBehaviour, IAttacker
{
    // DATA //
    // Instance Data
    public CombatEffects appliedDamage;
    public DamageType[] damageTypes = new DamageType[1] { DamageType.All };
    public float damageInterval = 1f;

    // Cached Data
    private List<Collider> currentColliders = new List<Collider>();


    // FUNCTIONS //
    // Basic Functions
    private void OnTriggerEnter(Collider other)
    {
        if (!currentColliders.Contains(other))
        {
            currentColliders.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        currentColliders.Remove(other);
    }

    private void OnEnable()
    {
        StartCoroutine(DamageApplier());
    }


    // Functionality
    public IEnumerator DamageApplier()
    {
        // Applies damage to each current collider while enabled
        while (enabled)
        {
            foreach(Collider collider in currentColliders)
            {
                CombatEntity combatEntity = collider.GetComponent<CombatEntity>();

                if(combatEntity != null)
                {
                    combatEntity.ApplyEffects(appliedDamage, transform.position, damageTypes, false, this);
                }
            }

            // Waits for interval to pass
            yield return new WaitForSeconds(damageInterval);
        }
    }
}
