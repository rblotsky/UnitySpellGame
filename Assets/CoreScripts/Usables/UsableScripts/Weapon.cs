using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Usables/Weapon")]
public class Weapon : Usable, IAttacker
{
    // WEAPON DATA //
    // Damages
    public CombatEffects elements;

    // Utility
    public float attackRange = 1.8f;
    public DamageType[] damageTypes = new DamageType[2] { DamageType.Mobs, DamageType.Objects };


    // OVERRIDES //
    // Basic Functions
    protected override void Awake()
    {
        itemType = UsableType.Weapon;
    }


    // Item Management
    public override bool UseItem(PlayerComponent playerToUse)
    {
        // Gets player's position
        Vector3 playerPosition = playerToUse.GetPlayerPosition();

        // Gets all nearby colliders, if they are tagged as CombatEntity attacks them.
        Collider[] nearbyColliders = Physics.OverlapSphere(playerPosition + (playerToUse.transform.forward * 1.5f), attackRange, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Collide);

        if (nearbyColliders.Length > 0)
        {
            foreach (Collider collider in nearbyColliders)
            {
                // Tries damaging the object as a CombatEntity
                CombatEntity attackedEntity = collider.gameObject.GetComponent<CombatEntity>();

                if (attackedEntity != null)
                {
                    // If it is a CombatEntity, applies damage
                    attackedEntity.ApplyEffects(elements, playerToUse.GetPlayerPosition(), damageTypes, false, this);
                }
            }
        }

        return false;
    }
}
