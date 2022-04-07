using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;

public class DebugDataViewerWindow : EditorWindow
{
    // DATA //
    bool playerStatsOpen = true;
    bool equipmentOpen = true;
    bool spellsOpen = true;
    bool questsOpen = true;
    bool merchantsOpen = true;
    Vector2 scrollPos = new Vector2(0, 0);


    // FUNCTIONS //
    // Basic Functions
    private void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        // Displays player stats
        if (PlayerStats.mainStats != null)
        {
            Queue<string> lines = PlayerStats.SaveDataToStringQueue(Newtonsoft.Json.Formatting.Indented);
            StringBuilder text = new StringBuilder();

            foreach (string line in lines)
            {
                text.AppendLine(line);
            }

            playerStatsOpen = EditorGUILayout.BeginFoldoutHeaderGroup(playerStatsOpen, "Player Stats", EditorStyles.foldout);
            EditorGUILayout.EndFoldoutHeaderGroup();

            if (playerStatsOpen)
            {
                EditorGUILayout.TextArea(text.ToString());
            }
        }

        // Displays player inventory
        if (PlayerEquipment.main != null)
        {
            Queue<string> lines = PlayerEquipment.main.SaveDataToStringQueue(Newtonsoft.Json.Formatting.Indented);
            StringBuilder text = new StringBuilder();

            foreach (string line in lines)
            {
                text.AppendLine(line);
            }

            equipmentOpen = EditorGUILayout.BeginFoldoutHeaderGroup(equipmentOpen, "Player Inventory", EditorStyles.foldout);
            EditorGUILayout.EndFoldoutHeaderGroup();

            if (equipmentOpen)
            {
                EditorGUILayout.TextArea(text.ToString());
            }
        }

        // Displays player spells
        if (PlayerSpells.main != null)
        {
            Queue<string> lines = PlayerSpells.main.SaveDataToStringQueue(Newtonsoft.Json.Formatting.Indented);
            StringBuilder text = new StringBuilder();

            foreach (string line in lines)
            {
                text.AppendLine(line);
            }

            spellsOpen = EditorGUILayout.BeginFoldoutHeaderGroup(spellsOpen, "Player Spells", EditorStyles.foldout);
            EditorGUILayout.EndFoldoutHeaderGroup();

            if (spellsOpen)
            {
                EditorGUILayout.TextArea(text.ToString());
            }
        }

        // Displays quests
        if (Quests.allQuests != null)
        {
            Queue<string> lines = Quests.SaveDataList(Newtonsoft.Json.Formatting.Indented);
            StringBuilder text = new StringBuilder();

            foreach (string line in lines)
            {
                text.AppendLine(line);
            }

            questsOpen = EditorGUILayout.BeginFoldoutHeaderGroup(questsOpen, "Quests", EditorStyles.foldout);
            EditorGUILayout.EndFoldoutHeaderGroup();

            if (questsOpen)
            {
                EditorGUILayout.TextArea(text.ToString());
            }
        }

        // Displays merchants
        if (MerchantDataList.merchantDataTable != null)
        {
            Queue<string> lines = MerchantDataList.SaveDataList(Newtonsoft.Json.Formatting.Indented);
            StringBuilder text = new StringBuilder();

            foreach (string line in lines)
            {
                text.AppendLine(line);
            }

            merchantsOpen = EditorGUILayout.BeginFoldoutHeaderGroup(merchantsOpen, "Merchants", EditorStyles.foldout);
            EditorGUILayout.EndFoldoutHeaderGroup();

            if (merchantsOpen)
            {
                EditorGUILayout.TextArea(text.ToString());
            }
        }

        EditorGUILayout.EndScrollView();
    }
}
