using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;
using Newtonsoft.Json;

[System.Serializable]
public class ScenePersistentData
{
    // Stores all persistent data for a scene
    public string sceneName;
    public List<string> persistentData;

    
    // Constructors
    public ScenePersistentData(string scene, List<string> newPersistentData)
    {
        sceneName = scene;
        persistentData = newPersistentData;
    }

    public ScenePersistentData(string scene, string[] newPersistentData)
    {
        sceneName = scene;
        persistentData = new List<string>(newPersistentData);
    }

    public ScenePersistentData()
    {
        sceneName = null;
        persistentData = new List<string>();
    }


    // Functions
    public void AddDataLine(string dataLine)
    {
        // Adds given line to scene name
        persistentData.Add(dataLine);
    }
}
