using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MapUILocationInfoPanel : MonoBehaviour
{
    // DATA //
    // UI References
    [Header("UI References")]
    public GameObject panelObject;
    public TextMeshProUGUI nameTextBox;
    public TextMeshProUGUI descriptionTextBox;
    public TextMeshProUGUI markedStageTextBox;
    public TextMeshProUGUI infoPointTextBox;
    public Button travelButton;

    
    // FUNCTIONS //
    // UI Updating
    public void UpdateUI(MapUILocationSlot locationToDisplay, bool canTravel)
    {
        // Updates name, description, and travel button state
        nameTextBox.SetText(locationToDisplay.location.displayName);
        descriptionTextBox.SetText(locationToDisplay.location.description);

        // Button state is active if location is visitable AND told that travel is allowed AND it isn't the player's current location
        travelButton.interactable = locationToDisplay.location.IsUsable && canTravel && MapDataList.playerMapLocation != locationToDisplay.location;

        // Updates marked stage and info points text boxes
        markedStageTextBox.SetText(locationToDisplay.cachedDataContainer.GenerateMarkedStageDisplayText());
        infoPointTextBox.SetText(locationToDisplay.cachedDataContainer.GenerateInfoPointsDisplayText());
        
    }

    public void ClearUI()
    {
        // Resets all UI elements to being empty, disables panel
        nameTextBox.SetText("");
        descriptionTextBox.SetText("");
        markedStageTextBox.SetText("");
        infoPointTextBox.SetText("");
        travelButton.interactable = false;
        panelObject.SetActive(false);
    }
}
