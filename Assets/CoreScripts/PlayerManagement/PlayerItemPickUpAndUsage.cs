using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerItemPickUpAndUsage : MonoBehaviour
{
    // FUNCTIONS //
    // Selecting, Dropping, Using items
    private void Update()
    {
        // Does nothing if player references aren't assigned in scene data ref
        if(DataRef.playerReference == null)
        {
            return;
        }

        // Does nothing if player is viewing menu
        if (DataRef.sceneMenuManagerReference.IsViewingMenu)
        {
            return;
        }

        // Sets inventory selected slot
        PlayerEquipment.main.SetSelectedSlot(GetSlotSelection(), true);

        // Uses items when player clicks
        if (Input.GetMouseButtonDown(0))
        {
            PlayerEquipment.main.UseSelectedItem();
        }

        // Drops item when player presses Q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayerEquipment.main.DropSelectedItem();
        }
        
    }


    // Item Selection Function
    private int GetSlotSelection()
    {
        // Selecting inventory slots
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            return 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            return 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            return 2;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            return 3;
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            return 4;
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            return 5;
        }

        else
        {
            return PlayerEquipment.EMPTY_SELECTED_SLOT_VALUE;
        }
    }
}
