using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[DisallowMultipleComponent]
[RequireComponent(typeof(RigidbodyVelocityModifier))]
public class CombatEntityEffectController : MonoBehaviour
{
    // DATA STRUCTURES //
    public struct EffectData
    {
        public float startTime;
        public float value;
        public float duration;
        public IAttacker attackData;

        public EffectData(float time, float eff, float dur)
        {
            startTime = time;
            value = eff;
            duration = dur;
            attackData = null;
        }
    }


    // DATA //
    // Basic Data
    public float baseSpeed = 0;
    public bool autoAddReferences = false;
    [HideInInspector]
    public List<EffectData> speedEffects = new List<EffectData>();
    [HideInInspector]
    public List<EffectData> healthChangeEffects = new List<EffectData>();

    // References
    public PlayerMovement playerMovementReference;
    public NavMeshAgent navAgentReference;
    public CombatEntity combatReference;
    public RigidbodyVelocityModifier velocityModifierReference;
    public AIBase aiBaseReference;

    // Cached Data
    private Coroutine healthChangeCoroutine = null;

    // Events
    public delegate void NoInputDelegate();
    public event NoInputDelegate onSpeedEffectUpdate;
    public event NoInputDelegate onHealthChangeEffectUpdate;

    // Constants
    public static readonly float HEALTH_MODIFY_INTERVAL = 1f;
    public static readonly float MAX_SPEED = 1000f;

    // Properties
    public float SpeedProperty { get; private set; }
    public float SpeedModifier { get; private set; }
    public float HealthChangeAmount { get; private set; }


    // FUNCTIONS //
    // Basic Functions
    private void Awake()
    {
        // If references must be added automatically, will do so. Logs warning if none present.
        if (autoAddReferences)
        {
            playerMovementReference = GetComponent<PlayerMovement>();
            navAgentReference = GetComponent<NavMeshAgent>();
            aiBaseReference = GetComponent<AIBase>();
            velocityModifierReference = GetComponent<RigidbodyVelocityModifier>();

            if(playerMovementReference == null && navAgentReference == null && aiBaseReference == null && velocityModifierReference == null)
            {
                Debug.LogWarning("[CombatEntityEffectController] Object \"" + name + "\" has no attached movement script to control!");
            }

            combatReference = GetComponent<CombatEntity>();
        }

        // Sets properties to base values
        SpeedProperty = baseSpeed;
        SpeedModifier = 0;

        // Updates move scripts w/ data
        UpdateMoveScripts();
    }

    private void Update()
    {
        // Checks top speed/poison effects, removes them if they've run out of time
        if(speedEffects.Count != 0) 
        {
            if(speedEffects[0].startTime+speedEffects[0].duration <= Time.time)
            {
                RemoveTopSpeedEffect();
            }
        }

        if (healthChangeEffects.Count != 0)
        {
            if (healthChangeEffects[0].startTime + healthChangeEffects[0].duration <= Time.time)
            {
                RemoveTopHealthEffect();
            }
        }
    }

    private void OnEnable()
    {
        if (healthChangeCoroutine == null)
        {
            healthChangeCoroutine = StartCoroutine(HealthChangeTimedEffect());
        }
    }


    // Main Management Functions
    public void UpdateMoveScripts()
    {
        // Updates the referenced movement scripts to use new values.
        if(playerMovementReference != null)
        {
            // Updates player movement script data
            playerMovementReference.moveSpeed = SpeedProperty;
        }

        if(navAgentReference != null)
        {
            // Updates nav agent script data
            navAgentReference.speed = SpeedProperty;
        }

        if(aiBaseReference != null)
        {
            aiBaseReference.entityMoveSpeed = SpeedProperty;
        }

        if(velocityModifierReference != null)
        {
            velocityModifierReference.SetModifier(SpeedProperty);
        }
    }

    public void RecalculateSpeedEffects()
    {
        // Recalculates effects
        float recalculatedEffects = 0;
        foreach (EffectData effect in speedEffects)
        {
            recalculatedEffects += effect.value;
        }

        // Adds together the speed and effects, then clamps to a minimum value of 0 to avoid backwards movement.
        // Modifier is clamped to a min of -100 to avoid breaking the effect displayer
        SpeedModifier = Mathf.Max(recalculatedEffects, -100);
        SpeedProperty = baseSpeed + baseSpeed*(recalculatedEffects/100);
        SpeedProperty = Mathf.Clamp(SpeedProperty, 0, MAX_SPEED);

        UpdateMoveScripts();

        // Runs update event
        if (onSpeedEffectUpdate != null)
        {
            onSpeedEffectUpdate();
        }
    }

    public void RecalculateHealthChangeEffects()
    {
        // Recalculates effects
        float recalculatedEffects = 0;
        foreach (EffectData effect in healthChangeEffects)
        {
            recalculatedEffects += effect.value;
        }

        HealthChangeAmount = recalculatedEffects;

        // Runs update event
        if (onHealthChangeEffectUpdate != null)
        {
            onHealthChangeEffectUpdate();
        }
    }

    public void UpdateBaseSpeed(float newValue)
    {
        baseSpeed = newValue;
        RecalculateSpeedEffects();
    }


    // Effect Management Functions
    public void AddSpeedEffect(float effectValue, float duration)
    {
        // Creates the effect object
        EffectData newEffect = new EffectData(Time.time, effectValue, duration);
        float finishTime = newEffect.startTime + newEffect.duration;

        // Adds it to the queue of speed effects, sorts it so it's in sequential order
        // Iterates through the current list of speed effects, adds this effect in current position
        for(int i = 0; i < speedEffects.Count; i++)
        {
            EffectData indexEffect = speedEffects[i];

            // If the index finishes first, inserts it and breaks
            if(indexEffect.startTime+indexEffect.duration > finishTime)
            {
                speedEffects.Insert(i, newEffect);
                break;
            }

            // If this is the last index, adds it at the end
            if(i == speedEffects.Count - 1)
            {
                speedEffects.Add(newEffect);
                break;
            }
        }

        // If no effects in list, adds one
        if(speedEffects.Count == 0)
        {
            speedEffects.Add(newEffect);
        }

        RecalculateSpeedEffects();
    }

    public void AddSpeedEffectFromAttack(float effectValue, float duration, IAttacker attacker)
    {
        // Checks if a speed effect with the attack attacker data exists - if it does, checks which value and duration to use
        // Uses highest effectValue and duration of both (new/existing)
        foreach(EffectData effect in speedEffects)
        {
            if (effect.attackData == attacker)
            {
                // Sets the effectValue and duration
                effectValue = Mathf.Max(effect.value, effectValue);
                duration = Mathf.Max(effect.duration, duration);
                speedEffects.Remove(effect);
                break;
            }
        }

        // Creates the new effect
        AddSpeedEffect(effectValue, duration);
    }

    public void AddHealthChangeEffectFromAttack(float effectValue, float duration, IAttacker attacker)
    {
        // Checks if a speed effect with the attack attacker data exists - if it does, checks which value and duration to use
        // Uses highest effectValue and duration of both (new/existing)
        foreach (EffectData effect in healthChangeEffects)
        {
            if (effect.attackData == attacker)
            {
                // Sets the effectValue and duration
                effectValue = Mathf.Max(effect.value, effectValue);
                duration = Mathf.Max(effect.duration, duration);
                healthChangeEffects.Remove(effect);
                break;
            }
        }

        // Creates the new effect
        EffectData newEffect = new EffectData(Time.time, effectValue, duration);
        float finishTime = newEffect.startTime + newEffect.duration;

        // Adds it to the queue of speed effects, sorts it so it's in sequential order
        // Iterates through the current list of speed effects, adds this effect in current position
        for (int i = 0; i < healthChangeEffects.Count; i++)
        {
            EffectData indexEffect = healthChangeEffects[i];

            // If the index finishes first, inserts it and breaks
            if (indexEffect.startTime + indexEffect.duration > finishTime)
            {
                healthChangeEffects.Insert(i, newEffect);
                break;
            }

            // If this is the last index, adds it at the end
            if (i == healthChangeEffects.Count - 1)
            {
                healthChangeEffects.Add(newEffect);
                break;
            }
        }

        // If no effects in list, adds one
        if (healthChangeEffects.Count == 0)
        {
            healthChangeEffects.Add(newEffect);
        }

        RecalculateHealthChangeEffects();
    }

    public void RemoveTopSpeedEffect()
    {
        speedEffects.RemoveAt(0);
        RecalculateSpeedEffects();
    }

    public void RemoveTopHealthEffect()
    {
        healthChangeEffects.RemoveAt(0);
        RecalculateHealthChangeEffects();
    }

    public IEnumerator HealthChangeTimedEffect()
    {
        while (isActiveAndEnabled)
        {
            if(combatReference != null)
            {
                if (HealthChangeAmount != 0)
                {
                    CombatEffects poisonElement = new CombatEffects(Mathf.RoundToInt(HealthChangeAmount), 0, 0, 0);
                    combatReference.ApplyEffects(poisonElement, transform.position, true, combatReference);
                }
            }

            yield return new WaitForSeconds(HEALTH_MODIFY_INTERVAL);
        }

        healthChangeCoroutine = null;
        yield return null;
    }
    
}
