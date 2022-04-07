using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapUIMapBackground : MonoBehaviour, IPointerClickHandler
{
    //TODO: Add some cool effects and stuff to the map background so this class isn't totally useless.

    // DATA //
    // Events
    public UILocationSlotPassEvent OnClickMapBackground = new UILocationSlotPassEvent();


    // FUNCTIONS //
    // Interface Functions
    public void OnPointerClick(PointerEventData pointerData)
    {
        // Runs OnClickMapBackground and passes a null MapUILocationSlot - this is to deselect all existing locations
        OnClickMapBackground.Invoke(null);
    }
}
