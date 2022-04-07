using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePersistentObjectManager : MonoBehaviour
{
    // DATA //
    // Instance Data
    public Dictionary<int, DataPersistentObject> persistentObjectTable = new Dictionary<int, DataPersistentObject>();
    public List<DataPersistentObject> allPersistentObjects = new List<DataPersistentObject>();


    // FUNCTIONS //
    // Basic Functions
    private void Start()
    {
        // Updates persistent object dictionary and list
        SetupObjectList();
        SetupObjectTable();

        // Sets ScenePersistentDataList current manager to this
        ScenePersistentDataList.currentObjectManager = this;

        // Loads Scene Data
        LoadSceneData();
    }


    // Data Management and Assignment
    public void LoadSceneData()
    {
        // Gets data from persistentDataManager and assigns it to objects
        foreach (string dataString in ScenePersistentDataList.GetSceneData(SceneManager.GetActiveScene().name))
        {
            string[] objectData = dataString.Trim().Split('|');

            DataPersistentObject assignedObject;

            // Gets object from dictionary and runs its data loading function using given data
            if (persistentObjectTable.TryGetValue(int.Parse(objectData[0]), out assignedObject))
            {
                Debug.Log("[DataPersistentObject] Object \"" + name + "\" loading data (" + dataString + ")");
                assignedObject.LoadData(objectData);
            }
        }
    }

    public void SaveSceneData()
    {
        List<string> sceneObjectData = new List<string>();

        // Finds all objects from dictionary, saves their data, and writes it to file.
        foreach (int key in persistentObjectTable.Keys)
        {
            DataPersistentObject objectAtKey;

            // Saves data from object, if it's not null
            if (persistentObjectTable.TryGetValue(key, out objectAtKey))
            {
                if(objectAtKey != null)
                {
                    sceneObjectData.Add(objectAtKey.SaveData());
                }

                else
                {
                    // If object is null, assumes it has been destroyed and saves default destroyed data for given key
                    sceneObjectData.Add(DataPersistentObject.SaveDestroyedObjectData(key));
                }
            }
        }

        // Saves all data to persistent data list
        ScenePersistentDataList.SetSceneData(SceneManager.GetActiveScene().name, sceneObjectData.ToArray());

        Debug.Log("Finished saving Data Persistent Objects!");
    }

    public void SetupObjectTable()
    {
        persistentObjectTable = new Dictionary<int, DataPersistentObject>();

        // Iterates through all persistent objects, then adds them to the dictionary
        foreach (DataPersistentObject persistentObject in allPersistentObjects)
        {
            if (!persistentObjectTable.ContainsKey(persistentObject.objectID))
            {
                persistentObjectTable.Add(persistentObject.objectID, persistentObject);
            }

            else
            {
                Debug.LogWarning("[ScenePersistentObjectManager] Found duplicate object ID! " + persistentObject.objectID, persistentObject.gameObject);
            }
        }

        Debug.Log("Set up Object Dictionary!");
    }

    [ContextMenu("Setup List (Debug Only)")]
    public void SetupObjectList()
    {
        // Adds all data persistent objects to list
        allPersistentObjects = new List<DataPersistentObject>(FindObjectsOfType<DataPersistentObject>(true));
        Debug.Log("Added all DataPersistentObjects to list!");
    }

}
