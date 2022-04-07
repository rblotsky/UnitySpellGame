using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

[CreateAssetMenu(fileName = "New Throwable Potion", menuName = "Usables/Throwable Potion")]
public class ThrowablePotion : Usable
{
    // DATA //
    // References
    [Header("Potion Data")]
    public GameObject throwableObject;
    public GameObject explodeParticles;

    // Data
    public float throwPower = 10f;
    public CombatEffects effects;
    public DamageType[] damageTypes = new DamageType[3] { DamageType.Player, DamageType.Objects, DamageType.Mobs };
    public float effectRadius;

    // Constants
    public static readonly float THROW_POWER_MULT = 100f;
    public static readonly float PARTICLE_AMOUNT_BY_RADIUS_MULTIPLIER = 0.5f;


    // OVERRIDES //
    // Basic Functions
    protected override void Awake()
    {
        itemType = UsableType.Throwable_Potion;
    }


    // Item Management
    public override bool UseItem(PlayerComponent playerToUse)
    {
        // Instantiates potion
        Vector3 playerPos = playerToUse.GetPlayerPosition();
        GameObject thrownPotion = Instantiate(throwableObject, playerPos+playerToUse.transform.forward*2, throwableObject.transform.rotation);
        Rigidbody potionRB = thrownPotion.GetComponent<Rigidbody>();
        UnityCallEventComponent potionCollision = thrownPotion.GetComponent<UnityCallEventComponent>();

        // Adds force and collision event listeners
        if (potionRB != null)
        {
            potionRB.AddForceAtPosition(playerToUse.transform.forward*throwPower*THROW_POWER_MULT, playerPos, ForceMode.Acceleration);
        }

        if (potionCollision != null)
        {
            potionCollision.onColliderEnter += HandlePotionCollide;
        }

        // Returns true to consume usable
        return true;
    }

    public override string GetItemDescription()
    {
        // Shows radius, throw power and effects
        StringBuilder description = new StringBuilder();
        description.Append("Radius: ");
        description.Append(effectRadius);
        description.Append("\nThrow Power: ");
        description.Append(throwPower);
        description.Append("\nEffects: \n");
        description.Append(effects.GetDisplayText(true));

        description.Append("\nDamage Types:");
        foreach(DamageType type in damageTypes)
        {
            description.Append("\n" + type.ToString().Replace('_', ' '));
        }

        return description.ToString();
    }

    public override string GetItemName()
    {
        StringBuilder nameBuilder = new StringBuilder();

        // Adds info for which combat effects are used
        if (effects.healthInstant > 0)
        {
            nameBuilder.Append("Healing, ");
        }

        if(effects.healthInstant < 0)
        {
            nameBuilder.Append("Damage, ");
        }

        if (effects.healthChange > 0)
        {
            nameBuilder.Append("Regeneration, ");
        }

        if (effects.healthChange < 0)
        {
            nameBuilder.Append("Poison, ");
        }

        if (effects.speedForDuration > 0)
        {
            nameBuilder.Append("Speed, ");
        }

        if (effects.speedForDuration < 0)
        {
            nameBuilder.Append("Slowness, ");
        }

        // Removes final comma
        if (nameBuilder.Length > 0)
        {
            nameBuilder.Remove(nameBuilder.Length - 2, 1);
        }

        // Adds "Potion"
        nameBuilder.Append("Throwable Potion");

        // Returns completed name
        return nameBuilder.ToString();
    }


    // FUNCTIONS //
    // Potion Management
    public void HandlePotionCollide(Collision collision, UnityCallEventComponent potion)
    {
        // If potion is already destroyed, does nothing
        if (!potion.enabled)
        {
            return;
        }

        // Gets explode position
        Vector3 explodePosition = potion.transform.position;

        // Destroys potion object
        Destroy(potion.gameObject);

        // Runs particles
        GameObject particleObject = Instantiate(explodeParticles, explodePosition, explodeParticles.transform.rotation);
        ParticleSystem particles = particleObject.GetComponent<ParticleSystem>();
        ParticleSystem.ShapeModule shapeModule = particles.shape;
        ParticleSystem.EmissionModule emissionModule = particles.emission;

        // Updates scale of emitter
        Vector3 scale = shapeModule.scale;
        scale.x = effectRadius;
        scale.y = effectRadius;
        shapeModule.scale = scale;

        // Increases emission rate according to radius
        ParticleSystem.MinMaxCurve emissionCurve = emissionModule.rateOverTime;
        emissionCurve.constant = emissionCurve.Evaluate(particles.time) * (effectRadius * PARTICLE_AMOUNT_BY_RADIUS_MULTIPLIER);
        emissionModule.rateOverTime = emissionCurve;
        particles.Play();

        // Applies combat effects
        Collider[] affectedColliders = Physics.OverlapSphere(explodePosition, effectRadius, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Collide);

        foreach (Collider collider in affectedColliders)
        {
            CombatEntity combatEntity = collider.GetComponent<CombatEntity>();

            if (combatEntity != null)
            {
                combatEntity.ApplyEffects(effects, explodePosition, damageTypes, false, this);
            }
        }
    }
}
