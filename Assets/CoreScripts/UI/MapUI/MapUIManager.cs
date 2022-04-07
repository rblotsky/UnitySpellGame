using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class MapUIManager : UIBase
{
    // DATA //
    // References
    [Header("Base References")]
    public MapObject mapReference;
    public MapUILocationSlot[] uiLocationSlots;
    public MapUILocationInfoPanel locationInfoPanel;
    public MapUIMapBackground mapBackground;
    public GameObject currentLocationMarker;

    // Cached Data
    private MapUILocationSlot selectedLocation;
    private bool canBeUsedToTravel;


    // OVERRIDES //
    public override void CloseUICustomDoDelete(bool doDeleteUI)
    {
        // Removes all event listeners
        RemoveEventListeners();

        // Closes all UI elements
        base.CloseUICustomDoDelete(doDeleteUI);
    }


    // FUNCTIONS //
    // Opening/Closing UI
    public void StartUIManager(bool canTravel)
    {
        // Caches whether can travel
        canBeUsedToTravel = canTravel;

        // Opens UI using scene UI manager
        DataRef.sceneMenuManagerReference.OpenNewUIInstance(this);

        // Sets up UI
        SetupUI();
    }


    // Setup
    public void SetupUI()
    {
        // Clears unnecessary cached data
        selectedLocation = null;

        // Adds event listeners to all necessary events
        AddEventListeners();

        // Sets up all location slots
        foreach(MapUILocationSlot locationSlot in uiLocationSlots)
        {
            locationSlot.SetupSlot();
        }

        // Updates UI
        UpdateLocationInfoPanel();
        SetCurrentLocationMarker();
    }

    public void AddEventListeners()
    {
        // Adds HandleUpdateSelectedMapLocation event to all location slots and map background
        foreach(MapUILocationSlot locationSlot in uiLocationSlots)
        {
            locationSlot.OnSelectLocation.AddListener(HandleUpdateSelectedMapLocation);
        }

        mapBackground.OnClickMapBackground.AddListener(HandleUpdateSelectedMapLocation);
    }

    public void RemoveEventListeners()
    {
        // Removes HandleUpdateSelectedMapLocation event to all location slots and map background
        foreach (MapUILocationSlot locationSlot in uiLocationSlots)
        {
            locationSlot.OnSelectLocation.RemoveListener(HandleUpdateSelectedMapLocation);
        }

        mapBackground.OnClickMapBackground.RemoveListener(HandleUpdateSelectedMapLocation);
    }


    // UI Updating
    public void UpdateLocationInfoPanel()
    {
        // If selected location is null, clears and closes info panel.
        if(selectedLocation == null)
        {
            locationInfoPanel.ClearUI();
            locationInfoPanel.gameObject.SetActive(false);
        }

        // Otherwise, opens and updates info panel.
        else
        {
            locationInfoPanel.gameObject.SetActive(true);
            locationInfoPanel.UpdateUI(selectedLocation, canBeUsedToTravel);
        }
    }

    public void UpdateLocationSlotSelectedStatus(MapUILocationSlot locationSlot, bool selected)
    {
        if (locationSlot != null)
        {
            locationSlot.UpdateSelectionHighlight(selected);
        }
    }

    public void SetCurrentLocationMarker()
    {
        //TODO: Set the current location marker to the right location
    }


    // UI Events
    public void HandleUpdateSelectedMapLocation(MapUILocationSlot locationToSelect)
    {
        // Does nothing if already selected this location
        if(locationToSelect == selectedLocation)
        {
            return;
        }

        // Otherwise, updates cached data and UI
        UpdateLocationSlotSelectedStatus(selectedLocation, false);
        UpdateLocationSlotSelectedStatus(locationToSelect, true);

        selectedLocation = locationToSelect;

        UpdateLocationInfoPanel();
    }

    public void HandleTravelSelectedLoation()
    {
        // Travels to selected location, if it isn't null.
        if(selectedLocation != null)
        {
            selectedLocation.location.TravelToLocation();
        }

        else
        {
            Debug.LogError("[MapUIManager] Attempting to travel to location but selectedLocation is null!");
        }
    }

    public void HandleClickExitButton()
    {
        if(DataRef.sceneMenuManagerReference != null)
        {
            DataRef.sceneMenuManagerReference.CloseUIInstance(this);
        }
    }
}
