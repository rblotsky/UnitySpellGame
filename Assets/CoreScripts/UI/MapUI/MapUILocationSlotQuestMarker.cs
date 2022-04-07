using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Security.Cryptography.X509Certificates;

public class MapUILocationSlotQuestMarker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // DATA //
    // UI References
    public GameObject questMarkerObject;
    public TextMeshProUGUI stageCountText;
    public UITooltip hoverPanel;

    // Cached Data
    private MapLocationQuestDataContainer locationDataContainer;
    private bool isMouseHovering;


    // FUNCTIONS //
    // Basic Functions
    private void Update()
    {
        // If currently hovering over the hover panel, updates it
        // Note: This is necessary so it follows mouse position across the hover area.
        if (isMouseHovering)
        {
            UpdateHoverPanel();
        }
    }


    // Setup
    public void SetupQuestMarker(MapLocationQuestDataContainer newDataContainer)
    {
        // Caches data container, then updates UI
        locationDataContainer = newDataContainer;
        UpdateUI();
    }


    // Interface Functions
    public void OnPointerEnter(PointerEventData pointerData)
    {
        // Caches that mouse is hovering, opens hover panel
        isMouseHovering = true;
        UpdateHoverPanel();
    }

    public void OnPointerExit(PointerEventData pointerData)
    {
        // Caches that mouse isn't hovering, closes hover panel
        isMouseHovering = false;
        UpdateHoverPanel();
    }


    // UI Updating
    public void UpdateUI()
    {
        // If there are marked stages, enables marker and updates elements.
        if(locationDataContainer.markedStages.Count > 0)
        {
            // Enables marker and updates its marked stage counter
            questMarkerObject.SetActive(true);
            stageCountText.SetText(locationDataContainer.markedStages.Count.ToString());
        }

        // If no marked stages, disables marker.
        else
        {
            questMarkerObject.SetActive(false);
        }
    }

    public void UpdateHoverPanel()
    {
        // Sets active based on whether mouse is hovering
        hoverPanel.gameObject.SetActive(isMouseHovering);

        // Updates hover panel text and position
        hoverPanel.UpdateText(locationDataContainer.GenerateMarkedStageDisplayText());
        hoverPanel.UpdatePosition(Input.mousePosition);
    }
}