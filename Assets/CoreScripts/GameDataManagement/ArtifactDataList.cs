using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public static class ArtifactDataList
{
    // DATA //
    // Basic Data
    public static List<string> foundArtifacts = new List<string>();


    // FUNCTIONS //
    // Discovering
    public static bool AddNewArtifact(Artifact newItem)
    {
        // Tries adding, returns true if added and false if didn't
        if (!GetArtifacts().Contains(newItem))
        {
            foundArtifacts.Add(newItem.id.ToString());
            return true;
        }

        return false;
    }


    // Getting Data
    public static List<Artifact> GetArtifacts()
    {
        List<Artifact> returnList = new List<Artifact>();

        foreach(string id in foundArtifacts)
        {
            returnList.Add((Artifact)UsableList.FindItem(id));
        }

        return returnList;
    }


    // Data Management
    public static Queue<string> SaveDataList()
    {
        Queue<string> queue = new Queue<string>();
        queue.Enqueue(JsonConvert.SerializeObject(foundArtifacts));
        return queue;
    }

    public static void LoadDataList(string text)
    {
        foundArtifacts = JsonConvert.DeserializeObject<List<string>>(text.Trim());
    }
}
