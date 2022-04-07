using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Map Location", menuName = "Maps/Map Location")]
public class MapLocation : GameResource
{
    // Data Structures
    [System.Serializable]
    public class SceneTransferData
    {
        public string transferName;
        public string newSceneName;
        public string travelSceneName;
        public TravelSceneDialogue travelDialogue;
    }


    // Location Data //
    // Basic Data
    [Header("Basic Data")]
    public string displayName;
    [TextArea(1,7)]
    public string description;
    public bool defaultIsVisible;
    public bool defaultIsUsable;
    public Sprite icon;

    // Scene Transfers
    public SceneTransferData[] sceneTransfers;

    // Cached Data
    public bool IsVisible { get { return MapDataList.GetLocationData(id.ToString()).locationVisible; } set { MapDataList.GetLocationData(id.ToString()).locationVisible = value; } }
    public bool IsUsable { get { return MapDataList.GetLocationData(id.ToString()).locationUsable; } set { MapDataList.GetLocationData(id.ToString()).locationUsable = value; } }
    public string CurrentTransferName { get { return MapDataList.GetLocationData(id.ToString()).locationTransferName; } set { MapDataList.GetLocationData(id.ToString()).locationTransferName = value; } }


    // FUNCTIONS //
    // Setup
    public void SetupLocation()
    {
        // Resets is visible and is usable to defaults
        IsVisible = defaultIsVisible;
        IsUsable = defaultIsUsable;

        // Resets currentTransferName to first item in sceneTransfers
        if (sceneTransfers.Length > 0)
        {
            CurrentTransferName = sceneTransfers[0].transferName;
        }

        else
        {
            CurrentTransferName = "none";
        }
    }


    // Location Management
    public virtual void SetVisible(bool visible)
    {
        IsVisible = visible;
    }

    public virtual void SetUsable(bool usable)
    {
        IsUsable = usable;
    }

    public virtual MapLocationQuestDataContainer GetMapLocationData()
    {
        Debug.Log("Getting location data! " + name);

        // Gets data into a new data container
        MapLocationQuestDataContainer newDataContainer = new MapLocationQuestDataContainer();
        Quests.RunMapLocationDataRequests(name, newDataContainer);

        // Returns the new data container
        return newDataContainer;
    }


    // Scene Transfer Functions
    public virtual void SetSceneTransfer(string newSceneTransferName)
    {
        // Updates currentTransferName to the new one
        CurrentTransferName = newSceneTransferName;
    }

    public virtual SceneTransferData GetCurrentSceneTransfer()
    {
        // Searches through all scene transfers
        foreach (SceneTransferData transfer in sceneTransfers)
        {
            // If the transfer is the correct one, returns it
            if (transfer.transferName.Equals(CurrentTransferName))
            {
                return transfer;
            }
        }

        // Returns null if not found
        return null;
    }

    public virtual void TravelToLocation()
    {
        // If can't be used, logs error and returns
        if (!IsUsable)
        {
            Debug.LogError("[MapLocation] Attempting to travel to unusable location! (" + name + ")");
            return;
        }

        // Otherwise, travels to this location
        // Gets scene transfer data
        SceneTransferData transferData = GetCurrentSceneTransfer();

        // If transferData is null, logs error and returns
        if(transferData == null)
        {
            Debug.LogError("[MapLocation] Location \"" + name + "\" cannot travel to location because currentTransferName is an impossible value! (" + CurrentTransferName + ")");
            return;
        }

        // Otherwise, runs the transfer (using travel scene, if there is one)
        else
        {
            //TODO: Allow MapLocation.TravelToLocation() to run custom scene transitions (eg. caravan, animation, etc.)
            MapDataList.playerMapLocation = this;

            // If there is a travel scene, plays it.
            if (!string.IsNullOrEmpty(transferData.travelSceneName) && !string.IsNullOrWhiteSpace(transferData.travelSceneName))
            {
                DataRef.transitionManagerReference.RunSceneTransitionWithTravelScene(transferData.newSceneName, transferData.travelSceneName, transferData.travelDialogue);
            }

            // Otherwise, runs basic transition
            else
            {
                DataRef.transitionManagerReference.BasicSceneTransition(transferData.newSceneName);
            }
        }

    }
}
