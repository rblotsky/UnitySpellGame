using UnityEngine;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public static class PlayerStats
{
    // Data Structures //
    [System.Serializable]
    public class PlayerStatsRawValues
    {
        public CombatEffects defense;
        public int health;
        public int maxHealth;
        public float moveSpeed;
        public float sprintMult;
        public float spellCooldown;

        // Constructor //
        public PlayerStatsRawValues(CombatEffects def, int hp, int maxhp, int spd, int sprint, int cooldown)
        {
            defense = def;
            health = hp;
            maxHealth = maxhp;
            moveSpeed = spd;
            sprintMult = sprint;
            spellCooldown = cooldown;
        }

        public PlayerStatsRawValues()
        {
            defense = CombatEffects.zero;
            health = 0;
            maxHealth = 0;
            moveSpeed = 0;
            sprintMult = 0;
            spellCooldown = 0;
        }
    }

    // DATA //
    public static PlayerStatsRawValues mainStats = new PlayerStatsRawValues();
    public static PlayerStatsRawValues defaultStats = new PlayerStatsRawValues(new CombatEffects(), 50, 50, 20, 2, 2);

    // Utility
    public static bool isRunningDialogue = false;
    public static bool canPlayerCastSpell = true;
    public static bool isPlayerPaused = true;


    // FUNCTIONS //
    // Reset Function
    public static void ResetStats()
    {
        // NOTE: Update this to use a more easily editable but still static system later.
        // Resets everything to default values
        mainStats.health = defaultStats.health;
        mainStats.maxHealth = defaultStats.maxHealth;
        mainStats.defense = defaultStats.defense;
        mainStats.moveSpeed = defaultStats.moveSpeed;
        mainStats.sprintMult = defaultStats.sprintMult;
        mainStats.spellCooldown = defaultStats.spellCooldown;
    }


    // Data Management
    public static Queue<string> SaveDataToStringQueue(Formatting format = Formatting.None)
    {
        Queue<string> queue = new Queue<string>();
        queue.Enqueue(JsonConvert.SerializeObject(mainStats, format));
        return queue;
    }

    public static void LoadDataFromText(string text)
    {
        mainStats = JsonConvert.DeserializeObject<PlayerStatsRawValues>(text);
    }
}