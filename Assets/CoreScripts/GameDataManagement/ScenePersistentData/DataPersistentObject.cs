using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if (UNITY_EDITOR)
using UnityEditor.SceneManagement;
#endif

[DisallowMultipleComponent]
[RequireComponent(typeof(DuplicatePersistentCatcher))]
public class DataPersistentObject : MonoBehaviour
{
    // DATA //
    // Persistent Data
    public int objectID;

    // Cached Data
    private List<IDataPersistentComponent> dataPersistentComponents = new List<IDataPersistentComponent>();


    // FUNCTIONS //
    // Basic Functions
    private void Awake()
    {
        if(objectID == 0)
        {
            Debug.LogWarning("[DataPersistentObject] " + name + " has an objectID of 0! ", gameObject);
        }

        MonoBehaviour[] components = gameObject.GetComponents<MonoBehaviour>();

        foreach (MonoBehaviour component in components)
        {
            if(component is IDataPersistentComponent)
            {
                dataPersistentComponents.Add((IDataPersistentComponent)component);
            }
        }
    }


    // Data Saving & Loading
    public string SaveData()
    {
        // Saves all object data in a string separated by "|" characters.
        string objectData = objectID + "|" + transform.position + "|" + transform.rotation.eulerAngles + "|" + gameObject.activeInHierarchy.ToString();

        foreach(IDataPersistentComponent component in dataPersistentComponents)
        {
            objectData += "|" + component.SaveDataToString();
        }

        return objectData;
    }

    public void LoadData(string[] dataList)
    {
        // Updates object data by parsing array of values from the string
        transform.position = GameUtility.ParseVector3(dataList[1]);
        transform.rotation = Quaternion.Euler(GameUtility.ParseVector3(dataList[2]));
        gameObject.SetActive(bool.Parse(dataList[3]));

        List<string> otherComponentsData = new List<string>(dataList);
        otherComponentsData.RemoveRange(0, 4);

        for(int i = 0; i < dataPersistentComponents.Count; i++)
        {
            if (otherComponentsData.Count > i)
            {
                Debug.Log("[DataPersistentObject] Object \"" + name + "\" loaded data for component: " + dataPersistentComponents[i].ToString(), gameObject);
                dataPersistentComponents[i].LoadDataFromString(otherComponentsData[i]);
            }
        }

        Debug.Log("[DataPersistentObject] Object \"" + name + "\" finished loading data!", gameObject);
    }


    // Destroyed Object Data Saving
    public static string SaveDestroyedObjectData(int ID)
    {
        // Default data for a destroyed object
        string objectData = ID + "|" + Vector3.zero + "|" + Vector3.zero + "|" + false;
        return objectData;
    }
}
