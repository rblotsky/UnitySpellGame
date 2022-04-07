using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class MapUILocationSlotInfoPointMarker : MonoBehaviour
{
    // DATA //
    // References
    public GameObject infoPointMarkerObject;
    public GameObject hoverPanel;

    // Cached Data
    protected MapLocationInfoPoint[] infoPoints;


    // FUNCTIONS //
    // Setup
    public void SetupInfoPointMarker(List<MapLocationInfoPoint> newInfoPoints)
    {
        // Caches new info points and updates UI
        infoPoints = newInfoPoints.ToArray();
    }


    // UI Updating
    public void UpdateUI()
    {
        //TODO: Implement MapUILocationSlotInfoPointMarker UpdateUI Function
    }
}
