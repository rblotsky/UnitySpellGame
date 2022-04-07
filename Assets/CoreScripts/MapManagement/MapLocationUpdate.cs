using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapLocationUpdate
{
    // DATA //
    // Basic Data
    public MapLocationUpdateType updateType;
    public MapLocation location;
    public string newSceneTransfer;
    public bool newValue;


    // FUNCTIONS //
    // Main Functionality
    public void RunLocationUpdate()
    {
        if(updateType == MapLocationUpdateType.Set_Usable)
        {
            if (newValue) location.IsVisible = newValue;
            location.IsUsable = newValue;
        }

        else if (updateType == MapLocationUpdateType.Set_Visible)
        {
            location.IsVisible = newValue;
        }

        else if (updateType == MapLocationUpdateType.Update_Scene_Transfer)
        {
            location.CurrentTransferName = newSceneTransfer;
        }
    }
}
