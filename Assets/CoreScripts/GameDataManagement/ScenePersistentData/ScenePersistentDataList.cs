using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public static class ScenePersistentDataList
{
    // DATA //
    // Data List
    public static Dictionary<string, string[]> persistentDataTable = new Dictionary<string, string[]>();

    // Properties
    public static ScenePersistentObjectManager currentObjectManager { get; set; }


    // FUNCTIONS //
    // Scene Data Management
    public static string[] GetSceneData(string sceneName)
    {
        // Creates struct for value to return
        string[] sceneData;

        // If there is data for the scene, returns it
        if (persistentDataTable.TryGetValue(sceneName, out sceneData))
        {
            return sceneData;
        }

        // Otherwise, returns with no data
        else
        {
            return new string[0];
        }
    }

    public static void SetSceneData(string sceneName, string[] sceneData)
    {
        // Does nothing if given null scene name
        if(sceneName == null)
        {
            return;
        }

        // If data for this scene already exists, overrides it.
        if (persistentDataTable.ContainsKey(sceneName))
        {
            Debug.Log("Overriding data entry: " + sceneName);
            persistentDataTable[sceneName] = sceneData;
            return;
        }

        // Otherwise, adds new entry
        else
        {
            Debug.Log("Creating new data entry: " + sceneName);
            persistentDataTable.Add(sceneName, sceneData);
        }

    }

    public static void ResetSceneData()
    {
        // Clears entire dictionary
        persistentDataTable.Clear();
    }

    public static List<ScenePersistentData> GetPersistentDataList()
    {
        List<ScenePersistentData> returnList = new List<ScenePersistentData>();

        foreach(string key in persistentDataTable.Keys)
        {
            returnList.Add(new ScenePersistentData(key, persistentDataTable[key]));
        }

        return returnList;
    }


    // Data Management
    public static Queue<string> SaveDataList()
    {
        // Creates line queue
        Queue<string> sceneDataLines = new Queue<string>();

        // Saves current scene data
        if (currentObjectManager != null)
        {
            currentObjectManager.SaveSceneData();
        }

        // Gets all data
        List<ScenePersistentData> allSceneData = GetPersistentDataList();

        // Enqueues strings for every data object
        foreach (ScenePersistentData sceneData in allSceneData)
        {
            // Enqueues JSON serialized data for scene
            string jsonSerialized = JsonConvert.SerializeObject(sceneData);
            sceneDataLines.Enqueue(jsonSerialized);
        }

        return sceneDataLines;
    }

    public static void LoadDataList(string saveFileText)
    {
        // Gets all lines and deserializes with JSON
        string[] sceneDataLines = saveFileText.Split('\n');

        foreach (string line in sceneDataLines)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                ScenePersistentData sceneData = JsonConvert.DeserializeObject<ScenePersistentData>(line.Trim());
                SetSceneData(sceneData.sceneName, sceneData.persistentData.ToArray());
            }
        }
    }
}
