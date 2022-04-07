using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Map Object", menuName = "Maps/Map Object")]
public class MapObject : GameResource
{
    // Map Data //
    // Base Data
    [Header("Base Data")]
    public string mapName;
    public GameObject mapPrefab;
    public MapLocation[] locationsOnMap;

    // Properties
    public bool IsKnown
    {
        get
        {
            // Is known if the player knows at least one location on the map.
            foreach(MapLocation location in locationsOnMap)
            {
                if (location.IsVisible)
                {
                    return true;
                }
            }

            return false;
        }
    }


    // FUNCTIONS //
    // Setup
    public void SetupMap(MapObject thisMap)
    {
        // Sets up all map locations
        foreach(MapLocation location in locationsOnMap)
        {
            location.SetupLocation();
        }
    }
}
