using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class PlayerComponent : MonoBehaviour
{
    // BASE COMPONENT DATA //
    // Attached Objects/Components
    [Header("Attached References")]
    public PlayerMovement component_MoveScript;
    public PlayerItemPickUpAndUsage component_ItemScript;
    public CombatEntity component_CombatEntity;
    public PlayerItemDisplayer component_ItemDisplayer;
    public CombatEntityEffectController component_EffectController;
    public Collider component_Collider;
    public GameDataManager gameDataManager;
    public Camera sceneMainCamera;
    public SoundEffectPlayer component_SoundPlayer;
    [Space]

    // Data 
    public bool isPaused = false;

    // UI Management
    [Header("UI Management")]
    public OverlayUIManager overlayManager;

    // Sounds
    [Header("Sounds")]
    public SoundEffect spellCastSound;

    // Constants
    public static float SPELL_COOLDOWN_INCREMENT = 0.01f;

    // Properties
    public float RemainingSpellCooldown { get; private set; } = 0f;

    // Events
    public delegate void IntegerInputDelegate(int input);
    public delegate void NoInputDelegate();
    public event IntegerInputDelegate onSpellCast;
    public event NoInputDelegate onStartUp;


    // FUNCTIONS //
    // Basic Functions
    private void Awake()
    {
        // Pauses player
        PlayerStats.isPlayerPaused = true;

        // Gets references to attached components
        component_MoveScript = GetComponent<PlayerMovement>();
        component_ItemScript = GetComponent<PlayerItemPickUpAndUsage>();
        component_CombatEntity = GetComponent<CombatEntity>();
        component_ItemDisplayer = GetComponent<PlayerItemDisplayer>();
        component_Collider = GetComponent<Collider>();
        component_EffectController = GetComponent<CombatEntityEffectController>();
        overlayManager = FindObjectOfType<OverlayUIManager>();
        sceneMainCamera = Camera.main;
        component_SoundPlayer = GetComponent<SoundEffectPlayer>();

        // Updates CombatEntity and PlayerMovement components with data from PlayerStats
        component_CombatEntity.SetupData(PlayerStats.mainStats.health, PlayerStats.mainStats.maxHealth, PlayerStats.mainStats.defense);
        component_EffectController.UpdateBaseSpeed(PlayerStats.mainStats.moveSpeed);
        component_MoveScript.sprintMult = PlayerStats.mainStats.sprintMult;

        // Starts sound effect player cleanup
        StartCoroutine(SoundEffectPlayerGarbageCollector());
    }

    private void Start()
    {
        // Updates StaticDataRef player-based references
        DataRef.UpdatePlayerRefs(this);

        // Sets ground level
        GameUtility.SetGroundLevel(GetPlayerPosition());

        // Runs StartUp event
        if (onStartUp != null)
        {
            onStartUp();
        }

        PlayerStats.isPlayerPaused = false;

        // Immediately saves progress. Game progress should be saved every time a new scene is loaded.
        gameDataManager.SaveGameProgress(DataRef.baseData.saveName, SceneManager.GetActiveScene().name);
    }

    private void OnDisable()
    {
        // Pauses player
        PlayerStats.isPlayerPaused = true;

        // Refreshes PlayerStats with latest data
        RefreshPlayerStats();

        // Clears all player events
        onSpellCast = null;
        onStartUp = null;

        // Resets StaticDataRef for all scene references
        DataRef.ResetAllSceneRefs();
    }

    void FixedUpdate()
    {
        // Does not do anything if component paused
        if (!PlayerStats.isPlayerPaused)
        {
            // Spell Casting
            if (Input.GetKey(KeyCode.E))
            {
                if (PlayerStats.canPlayerCastSpell && !DataRef.sceneMenuManagerReference.IsViewingMenu)
                {
                    CastFirstSpell();
                }
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                if (PlayerStats.canPlayerCastSpell && !DataRef.sceneMenuManagerReference.IsViewingMenu)
                {
                    CastSecondSpell();
                }
            }
        }
    }


    // Spells
    private void CastFirstSpell()
    {
        if (PlayerSpells.main.spell1.shapeComponent != null)
        {
            // Prevents player from casting a new spell, casts first spell
            StartCoroutine(PlayerSpells.main.spell1.shapeComponent.UseComponent(component_CombatEntity, PlayerSpells.main.spell1));
            PlayerStats.canPlayerCastSpell = false;

            // Plays sound from the player's audio manager
            SoundEffect.TryPlaySoundEffect(spellCastSound, component_SoundPlayer);

            // Runs spellcast event for first spell
            if (onSpellCast != null)
            {
                onSpellCast(1);
            }
        }
    }

    private void CastSecondSpell()
    {
        if (PlayerSpells.main.spell2.shapeComponent != null)
        {
            // Prevents player from casting a new spell, casts second spell
            StartCoroutine(PlayerSpells.main.spell2.shapeComponent.UseComponent(component_CombatEntity, PlayerSpells.main.spell2));
            PlayerStats.canPlayerCastSpell = false;

            // Plays sound from the player's audio manager
            SoundEffect.TryPlaySoundEffect(spellCastSound, component_SoundPlayer);

            // Runs spellcast event for second spell
            if (onSpellCast != null)
            {
                onSpellCast(2);
            }
        }
    }

    public void StartSpellCooldown()
    {
        // Stops existing spellCooldown if there is one and starts new one
        StopCoroutine(SpellCooldown());
        StartCoroutine(SpellCooldown());
    }


    // Utility
    public Vector3 GetPlayerPosition()
    {
        return transform.position;
    }

    public Quaternion GetPlayerRotation()
    {
        return transform.rotation;
    }

    public void RefreshPlayerStats()
    {
        // This function refreshes playerStats with the current data available in the CombatEntity and PlayerMovement components
        PlayerStats.mainStats.health = component_CombatEntity.health;
        PlayerStats.mainStats.maxHealth = component_CombatEntity.maxHealth;
        PlayerStats.mainStats.defense = component_CombatEntity.defense;

        PlayerStats.mainStats.moveSpeed = component_EffectController.baseSpeed;
        PlayerStats.mainStats.sprintMult = component_MoveScript.sprintMult;
    }

    public void DisablePlayer()
    {
        // Disables this object
        gameObject.SetActive(false);
    }


    // Coroutines
    private IEnumerator SpellCooldown()
    {
        PlayerStats.canPlayerCastSpell = false;
        RemainingSpellCooldown = PlayerStats.mainStats.spellCooldown;

        int numIncrements = Mathf.RoundToInt(PlayerStats.mainStats.spellCooldown / SPELL_COOLDOWN_INCREMENT);

        for(int i = 0; i < numIncrements; i++)
        {
            // Updates cooldown time left
            RemainingSpellCooldown -= SPELL_COOLDOWN_INCREMENT;
            RemainingSpellCooldown = Mathf.Round(RemainingSpellCooldown*100)/100;

            // Waits for cooldown increment (as long as player isn't paused)
            yield return new WaitForSeconds(SPELL_COOLDOWN_INCREMENT);
        }

        PlayerStats.canPlayerCastSpell = true;
        yield return null;
    }

    public IEnumerator SoundEffectPlayerGarbageCollector()
    {
        // Only runs if it's been past the required time
        while (gameObject.activeInHierarchy) 
        {
            SoundEffectPlayer.CleanupFinishedSoundPlayers();
            yield return new WaitForSeconds(SoundEffectPlayer.CLEANUP_DELAY);
        }
    }
}
