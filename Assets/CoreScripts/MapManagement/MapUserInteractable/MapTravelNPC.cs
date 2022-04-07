using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapTravelNPC : Interactable
{
    // DATA //
    // Map Reference
    public MapObject mapUsed;

    // Instance Data
    public bool canTravelOnMap = true;


    // OVERRIDES //
    protected override void Interact()
    {
        // If there is no map reference assigned, logs error and returns.
        if(mapUsed == null)
        {
            Debug.LogError("[MapTravelNPC] (" + name + ") MapTravelNPC cannot have null mapUsed reference!");
            return;
        }

        // If the map reference has no UI manager, logs error and returns
        if(mapUsed.mapPrefab == null)
        {
            Debug.LogError("[MapTravelNPC] (" + name + ") mapUsed cannot have a null mapPrefab if it is used with a MapTravelNPC! (Map: " + mapUsed.name + ")");
            return;
        }

        // Otherwise, opens map prefab.
        else
        {
            // Instantiates map prefab and starts UI manager
            MapUIManager mapUI = Instantiate(mapUsed.mapPrefab).GetComponent<MapUIManager>();
            DataRef.sceneMenuManagerReference.OpenNewUIInstance(mapUI);
            mapUI.StartUIManager(canTravelOnMap);
        }
    }
}
