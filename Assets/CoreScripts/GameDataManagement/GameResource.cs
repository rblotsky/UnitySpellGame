using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameResource : ScriptableObject
{
    // DATA //
    public int id;


    // FUNCTIONS //
    // Basic Functions
    private void Awake()
    {
        id = GetNewResourceID();
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
        Debug.Log("New resource ID generated: " + id + " (Object Name: " + name + ")");
    }

    
    // ID Management
    public static int GetNewResourceID()
    {
        GameResource[] allResources = Resources.LoadAll<GameResource>("");

        int highestID = 1;
        foreach(GameResource resource in allResources)
        {
            highestID = Mathf.Max(highestID, resource.id);
        }

        return highestID+1;
    }

    public static void GenerateAllIDs()
    {
        GameResource[] allResources = Resources.LoadAll<GameResource>("");

        // Adds all resources without ID to list and gives them new ID
        List<GameResource> needID = new List<GameResource>();
        int highestID = 1;

        foreach(GameResource resource in allResources)
        {
            if(resource.id == 0)
            {
                needID.Add(resource);
                continue;
            }

            highestID = Mathf.Max(highestID, resource.id);
        }

        // Gives new ID to all required resources
        foreach(GameResource resource in needID)
        {
            Debug.Log("New resource ID generated: " + (highestID+1) + " (Object Name: " + resource.name + ")");
            resource.id = highestID + 1;
#if UNITY_EDITOR
            EditorUtility.SetDirty(resource);
#endif
            highestID++;
        }
    }

}
