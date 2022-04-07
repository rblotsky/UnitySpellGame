using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[DisallowMultipleComponent]
public class CombatEntityAnimator : MonoBehaviour
{
    // Main class for animations on CombatEntities
    // Runs burning, other status effects, damage, death, movement, etc. animations

    // Data Structures
    public enum DestroyAnimationType
    {
        Collider_Removal,
        Baked_Shatter,
        Vanish,
        Particle_Effect,
        None,
        Shatter_And_Particle,
    }


    // DATA //
    // Basic Data and References
    [Header("Basic Data/References")]
    public bool autoAddEventListeners = true;
    public bool autoAddReferences = true;
    public float particleRemovalTime = 2f;
    public float dataDisplayerVertOffset = 2f;
    public CombatEntity objectCombatEntity;
    public Collider objectCollider;
    public Rigidbody objectRigidbody;
    public Renderer[] objectRenderers;
    public SoundEffectPlayer soundPlayer;
    public OverlayUIAttackNotifications attackNotifications;

    // Prefabs
    [Header("Prefabs")]
    public GameObject dataDisplayerPrefab;

    // Destroy Animation
    [Header("Destruction Animation")]
    public GameObject shatteredObject;
    public GameObject deathParticles;
    public DestroyAnimationType destructionAnimationType = DestroyAnimationType.Vanish;
    public float completeDestructionDelay;

    // Damage Animation
    [Header("Damage Animation")]
    public GameObject attackApplyParticles;
    public GameObject attackSpeedEffectParticles;
    public GameObject attackPoisonEffectParticles;

    // Sound Effects
    [Header("Sound Effects")]
    public SoundEffect effectApplySound;
    public SoundEffect deathSound;
    public SoundEffect ambientSound;
    public float randomSoundMinDelay = 3;
    public float randomSoundMaxDelay = 20;

    // Cached Data
    private float nextRandomSound = 0;

    // Constants
    public static readonly float RANDOM_SOUND_CHANCE_PER_FRAME = 0.01f;


    // FUNCTIONS //
    // Basic Functions
    private void Awake()
    {
        // Gets references to attached components, if necessary
        if (autoAddReferences)
        {
            objectCollider = GetComponentInChildren<Collider>();
            objectRigidbody = GetComponentInChildren<Rigidbody>();
            objectCombatEntity = GetComponentInChildren<CombatEntity>();
            objectRenderers = GetComponentsInChildren<Renderer>();
            soundPlayer = GetComponentInChildren<SoundEffectPlayer>();
            attackNotifications = FindObjectOfType<OverlayUIAttackNotifications>();
        }
    }

    private void Start()
    {
        // Instantiates the name text prefab
        if (dataDisplayerPrefab != null)
        {
            // Calculates the uppermost part of the collider
            Vector3 highAbove = new Vector3(transform.position.x, transform.position.y + 50, transform.position.z);
            Vector3 topOfCollider = objectCollider.ClosestPointOnBounds(highAbove);

            // Instantiates data displayer above that point
            GameObject instantiatedDisplayer = Instantiate(dataDisplayerPrefab, topOfCollider, dataDisplayerPrefab.transform.rotation, transform);
            Vector3 displayerPosition = instantiatedDisplayer.transform.position;
            displayerPosition.y += dataDisplayerVertOffset;
            instantiatedDisplayer.transform.position = displayerPosition;

            // Gets the data displayer reference and assigns its combat entity reference
            UICombatEntityDataDisplayer dataDisplayer = instantiatedDisplayer.GetComponentInChildren<UICombatEntityDataDisplayer>();

            if(dataDisplayer != null)
            {
                dataDisplayer.SetEntityReference(objectCombatEntity);
            }
        }
    }

    private void OnEnable()
    {
        // Adds event listeners
        objectCombatEntity.onDeath += HandleObjectDeath;
        objectCombatEntity.onAttackApply += HandleEffectApply;
    }

    private void OnDisable()
    {
        // Removes event listeners
        objectCombatEntity.onDeath -= HandleObjectDeath;
        objectCombatEntity.onAttackApply -= HandleEffectApply;
    }

    private void Update()
    {
        // If it hasn't played random sound in longer than current delay, plays it
        if(Time.time > nextRandomSound)
        {
            // Plays the random sound
            SoundEffect.TryPlaySoundEffect(ambientSound, soundPlayer);

            // Caches next play time
            nextRandomSound = Time.time + Random.Range(randomSoundMinDelay, randomSoundMaxDelay);
        }
    }


    // Object Death
    public void HandleObjectDeath()
    {
        // Plays a death sound and then animation
        SoundEffect.TryPlaySoundEffect(deathSound, soundPlayer);

        // Checks animation type, runs accordingly
        if (destructionAnimationType == DestroyAnimationType.Collider_Removal)
        {
            StartCoroutine(ColliderRemovalDestroy());
        }

        else if (destructionAnimationType == DestroyAnimationType.Baked_Shatter)
        {
            BakedShatterDestroy();
        }

        else if (destructionAnimationType == DestroyAnimationType.Vanish)
        {
            StartCoroutine(VanishDestroy());
        }

        else if (destructionAnimationType == DestroyAnimationType.Particle_Effect)
        {
            StartCoroutine(ParticleEffectDestroy());
        }
    }


    // Object Attack Apply
    public void HandleEffectApply(CombatEffects attackInfo, Vector3 attackSourcePosition)
    {
        // Plays sound and runs animation
        SoundEffect.TryPlaySoundEffect(effectApplySound, soundPlayer);

        attackNotifications.NewPanel(attackInfo, transform.position, gameObject);

        // Instantiates particles for health
        if (attackInfo.healthInstant != 0)
        {
            if (attackApplyParticles != null)
            {
                Instantiate(attackApplyParticles, transform.position, transform.rotation, transform);
            }
        }

        // Instantiates particles for poison
        if (attackInfo.healthChange != 0)
        {
            if (attackPoisonEffectParticles != null)
            {
                Instantiate(attackPoisonEffectParticles, transform.position, transform.rotation, transform);
            }
        }

        // Instantiates particles for speed
        if (attackInfo.speedForDuration != 0)
        {
            if (attackSpeedEffectParticles != null)
            {
                Instantiate(attackSpeedEffectParticles, transform.position, transform.rotation, transform);
            }
        }
    }


    // Destruction Animation Coroutines
    public void BakedShatterDestroy()
    {
        // Gets the velocity at time of destruction
        Vector3 destroyVelocity = Vector3.zero;

        if (objectRigidbody != null) destroyVelocity = objectRigidbody.velocity;

        // Instantiates shattered version and removes normal version
        objectCollider.enabled = false;
        GameObject shatteredInstance = Instantiate(shatteredObject, transform.position, transform.rotation);

        // Gets object fragments and modifies them with mass and force of original
        ShatteredObject shatterFragments = shatteredInstance.GetComponent<ShatteredObject>();
        shatterFragments.SetFragmentSize(transform.lossyScale);
        shatterFragments.ApplyForceToFragments(destroyVelocity);
        shatterFragments.StartCoroutine(shatterFragments.RunShatterFragments(completeDestructionDelay));

        // Disables object instead of destroying so its status is saved.
        gameObject.SetActive(false);
    }

    public IEnumerator ColliderRemovalDestroy()
    {
        objectCollider.enabled = false;
        yield return new WaitForSeconds(completeDestructionDelay);
        gameObject.SetActive(false);
    }

    public IEnumerator VanishDestroy()
    {
        yield return new WaitForSeconds(completeDestructionDelay);
        gameObject.SetActive(false);
    }

    public IEnumerator ParticleEffectDestroy()
    {
        if (deathParticles != null)
        {
            Instantiate(deathParticles, transform.position, deathParticles.transform.rotation);
        }

        yield return new WaitForSeconds(completeDestructionDelay);
        gameObject.SetActive(false);
    }


    // Damage Animation Coroutines
}
