using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using System;

public static class MenuItems
{
    [MenuItem("Tools/Merchants/Generate Trade Ranges")]
    public static void GenerateAllMerchantTradeRanges()
    {
        MerchantTradeData[] allMerchantTrades = Resources.LoadAll<MerchantTradeData>("MerchantTrades");

        foreach(MerchantTradeData data in allMerchantTrades)
        {
            data.GenerateSelectionRanges();
        }
    }

    [MenuItem("Tools/DataPersistentObjects/Set Object IDs")]
    public static void SetObjectIDs()
    {
        // Creates list of objects that require a new ID
        List<DataPersistentObject> requireNewID = new List<DataPersistentObject>();

        // Finds all existing SceneDataPersistent objects
        DataPersistentObject[] allPersistent = GameObject.FindObjectsOfType<DataPersistentObject>(true);

        // Sets current highest ID
        int highestID = 0;

        // Searches through all SceneDataPersistent to find the highest ID, then assign new IDs starting from the highest one.
        foreach (DataPersistentObject persistentObject in allPersistent)
        {
            if (persistentObject.objectID == 0)
            {
                requireNewID.Add(persistentObject);
                continue;
            }

            else if (persistentObject.objectID > highestID)
            {
                highestID = persistentObject.objectID;
                continue;
            }
        }

        // Adds IDs to all non-IDed objects
        foreach (DataPersistentObject persistentObject in requireNewID)
        {
            persistentObject.objectID = highestID + 1;
            Undo.RecordObject(persistentObject, "Set Value");
            EditorUtility.SetDirty(persistentObject);

            Debug.Log("[DataPersistentObject] New ID assigned (" + persistentObject.name + ", " + persistentObject.objectID + ")");
            highestID++;
        }

#if (UNITY_EDITOR)
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
#endif

        // Finishes setting IDs
        Debug.Log("Set all IDs!");
    }

    [MenuItem("Tools/DataPersistentObjects/Reset Object IDs")]
    public static void ResetObjectIDs()
    {
        // Finds all existing SceneDataPersistent objects
        DataPersistentObject[] allPersistent = GameObject.FindObjectsOfType<DataPersistentObject>(true);

        // Iterates through all DataPersistentObjects and sets their ID to 0
        foreach (DataPersistentObject persistentObject in allPersistent)
        {
            persistentObject.objectID = 0;
        }

#if (UNITY_EDITOR)
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
#endif

        // Finishes resetting IDs
        Debug.Log("Reset all IDs!");
    }

    [MenuItem("Tools/DataPersistentObjects/Check For Invalid IDs")]
    public static void CheckForInvalidIDs()
    {
        DataPersistentObject[] allPersistent = GameObject.FindObjectsOfType<DataPersistentObject>(true);
        List<int> foundIDs = new List<int>();

        // Iterates through all DataPersistentObjects and checks if ID 0
        foreach (DataPersistentObject persistentObject in allPersistent)
        {
            if (foundIDs.Contains(persistentObject.objectID)) 
            {
                Debug.LogWarning("[DataPersistentObject] Found duplicate ID: " + persistentObject.objectID + ", object: " + persistentObject.name, persistentObject.gameObject);
            }

            if(persistentObject.objectID == 0)
            {
                Debug.LogWarning("[DataPersistentObject] Found ID 0 on: " + persistentObject.name, persistentObject.gameObject);
            }
        }

        Debug.Log("Checked for invalid IDs!");
    }

    [MenuItem("Tools/Open Debug Window")]
    public static void OpenDebugWindow()
    {
        EditorWindow.GetWindow<DebugDataViewerWindow>("Player Debug");
    }

    [MenuItem("Tools/Generate All Resource IDs")]
    public static void GenerateResourceIDs()
    {
        GameResource.GenerateAllIDs();
    }

    [MenuItem("Tools/Reset All Resource IDs")]
    public static void ResetResourceIDs()
    {
        GameResource[] allResources = Resources.LoadAll<GameResource>("");

        foreach(GameResource resource in allResources)
        {
            resource.id = 0;
            EditorUtility.SetDirty(resource);
        }
    }
}
