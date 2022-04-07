using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class MapUILocationSlot : MonoBehaviour, IPointerClickHandler
{
    // DATA //
    // UI References
    [Header("UI References")]
    public Image locationIcon;
    public TextMeshProUGUI locationName;
    public GameObject infoPointIcon;
    public MapUILocationSlotQuestMarker questMarker;
    public GameObject selectionHighlight;
    public UITooltip infoPointHoverPanel;

    // Location Reference
    [Header("Location Reference")]
    public MapLocation location;

    // Events
    public UILocationSlotPassEvent OnSelectLocation = new UILocationSlotPassEvent();

    // Constants
    public static readonly string EMPTY_INFO_POINT_HOVER_TEXT = "No info points.";

    // Cached Data
    [HideInInspector]
    public MapLocationQuestDataContainer cachedDataContainer;
    private bool isHoveringInfoPointIcon;


    // FUNCTIONS //
    // Basic Functions
    private void Update()
    {
        // If currently hovering over the hover panel, updates it
        // Note: This is necessary so it follows mouse position across the hover area.
        if (isHoveringInfoPointIcon)
        {
            UpdateInfoPointHoverPanel();
        }
    }


    // Interface Functions
    public void OnPointerClick(PointerEventData pointerData)
    {
        // Runs OnSelectLocation
        OnSelectLocation.Invoke(this);
    }


    // Setup
    public void SetupSlot()
    {
        // Logs error if map location reference isn't present
        if(location == null)
        {
            Debug.LogError("[MapUILocationSlot] " + name + " has no location reference!");
        }

        // Gets location quest data from location scriptableobject
        cachedDataContainer = location.GetMapLocationData();

        // Updates quest marker with info
        questMarker.SetupQuestMarker(cachedDataContainer);

        // Updates UI
        UpdateBaseUI();
    }


    // UI Updating
    public void UpdateBaseUI()
    {
        // Updates base UI elements
        locationName.SetText(location.displayName);
        locationIcon.sprite = location.icon;

        // Sets status of info point icon (if more than 0 points, active. Otherwise, inactive)
        infoPointIcon.SetActive(cachedDataContainer.infoPoints.Count > 0);
        gameObject.SetActive(location.IsVisible);
    }

    public void UpdateSelectionHighlight(bool selected)
    {
        // Updates the selection highlight based on whether it's selected
        if (selectionHighlight != null)
        {
            selectionHighlight.SetActive(selected);
        }
    }

    public void UpdateInfoPointHoverPanel()
    {
        // Opens/closes panel
        infoPointHoverPanel.gameObject.SetActive(isHoveringInfoPointIcon);

        // Updates hover panel text to generated info points string
        infoPointHoverPanel.UpdateText(cachedDataContainer.GenerateInfoPointsDisplayText());

        // Updates position
        infoPointHoverPanel.UpdatePosition(Input.mousePosition);
    }


    // UI Events
    public void InfoPointStartHover()
    {
        // Caches that it's hovering
        isHoveringInfoPointIcon = true;

        // Updates hover panel
        UpdateInfoPointHoverPanel();
    }

    public void InfoPointEndHover()
    {
        // Caches that it's not hovering
        isHoveringInfoPointIcon = false;

        // Updates hover panel
        UpdateInfoPointHoverPanel();
    }

}




// DATA STRUCTURES //
public class UILocationSlotPassEvent : UnityEvent<MapUILocationSlot>
{
}
