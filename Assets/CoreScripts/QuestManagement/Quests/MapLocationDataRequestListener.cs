using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapLocationDataRequestListener
{
    // REQUEST DATA //
    public string expectedRequest;
    public bool addStageMarker;
    public MapLocationInfoPoint[] infoPoints;

    
    // FUNCTIONS //
    // Main Functionality
    public void RequestLocationInfo(string givenRequest, MapLocationQuestDataContainer dataContainer, QuestStage stageUsed)
    {
        // If the given request matches expected request, adds marked stage and info text to the data container
        // Note: stageUsed passed as reference because cannot use 'this' in ScriptableObjects
        if (givenRequest.Equals(expectedRequest))
        {
            // Adds data to container
            if (addStageMarker)
            {
                dataContainer.markedStages.Add(stageUsed);
            }

            dataContainer.infoPoints.AddRange(infoPoints);
        }
    }
}
