using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public static class MapDataList
{
    // Data Structures //
    public class MapLocationData
    {
        public string locationName;
        public bool locationUsable = false;
        public bool locationVisible = false;
        public string locationTransferName = "Default";
    }


    // DATA //
    // Master Lists
    public static MapObject[] mapObjectMasterList;
    public static MapLocation[] mapLocationMasterList;

    // Dictionaries
    public static Dictionary<string, MapLocationData> mapLocationDataTable;
    public static Dictionary<string, MapLocation> mapLocationTable;

    // Player Map Data
    public static MapLocation playerMapLocation = null;


    // FUNCTIONS //
    // Setup
    public static void SetupFunction()
    {
        // Sets up master lists from resources folder
        mapObjectMasterList = Resources.LoadAll<MapObject>("Maps");
        mapLocationMasterList = Resources.LoadAll<MapLocation>("Maps");

        // Sets up dictionaries
        mapLocationDataTable = new Dictionary<string, MapLocationData>();
        mapLocationTable = new Dictionary<string, MapLocation>();

        foreach(MapLocation location in mapLocationMasterList)
        {
            mapLocationTable.Add(location.id.ToString(), location);

            MapLocationData locationData = new MapLocationData();
            locationData.locationName = location.id.ToString();
            mapLocationDataTable.Add(location.id.ToString(), locationData);
        }

        // Resets player map location to null
        playerMapLocation = null;

        // Sets up all maps
        foreach(MapObject map in mapObjectMasterList)
        {
            map.SetupMap(map);
        }
    }


    // Data Management
    public static Queue<string> SaveDataList()
    {
        Queue<string> queue = new Queue<string>();

        foreach(string name in mapLocationDataTable.Keys)
        {
            MapLocationData data = GetLocationData(name);
            if (data != null)
            {
                queue.Enqueue(JsonConvert.SerializeObject(data));
            }
        }

        return queue;
    }

    public static void LoadDataList(string saveFileText)
    {
        string[] lines = saveFileText.Split('\n');

        foreach(string line in lines)
        {
            MapLocationData newData = JsonConvert.DeserializeObject<MapLocationData>(line.Trim());

            if (newData != null)
            {
                if (mapLocationDataTable.ContainsKey(newData.locationName))
                {
                    mapLocationDataTable[newData.locationName] = newData;
                }

                else
                {
                    mapLocationDataTable.Add(newData.locationName, newData);
                }
            }
        }
    }
    

    // Searching
    /// <summary>
    /// Gets the map location whose current transfer scene is this scene. NOTE: This can yield unexpected results with scenes used by multiple locations.
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    public static MapLocation GetLocationByCurrentTransferSceneName(string sceneName)
    {
        // Searches through all map locations
        foreach(MapLocation location in mapLocationMasterList)
        {
            // If the current scene transfer goes to this scene, returns this location
            foreach (MapLocation.SceneTransferData transferData in location.sceneTransfers)
            {
                if (transferData.newSceneName.Equals(sceneName))
                {
                    return location;
                }
            }
        }

        // Returns null if no locations found
        return null;
    }

    public static MapLocationData GetLocationData(string id)
    {
        MapLocationData returnData = null;
        mapLocationDataTable.TryGetValue(id, out returnData);
        return returnData;
    }
    
}
