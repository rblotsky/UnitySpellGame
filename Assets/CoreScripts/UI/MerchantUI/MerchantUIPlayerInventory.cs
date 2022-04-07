using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantUIPlayerInventory : MonoBehaviour
{
    // DATA //
    // References
    public MerchantUIItemSlot[] inventorySlots;

    // FUNCTIONS //
    // Setup Function
    public void Setup(MerchantUIManager uiManager)
    {
        // Sets up all the inventory slots
        foreach(MerchantUIItemSlot slot in inventorySlots)
        {
            slot.Setup(uiManager);
        }

        // Updates all the slots
        UpdateInventorySlots();
    }


    // UI Updating Functions
    public void UpdateInventorySlots()
    {
        // Gets player inventory reference
        PlayerEquipment playerInventory = PlayerEquipment.main;

        // Fills each slot with player's inventory item at that slot
        for (int slotIndex = 0; slotIndex < inventorySlots.Length; slotIndex++)
        {
            // If the slot index is above player's inventory size, logs error.
            if(slotIndex+1 > playerInventory.InventorySize)
            {
                Debug.LogError("[MerchantUIPlayerInventory] Error in UpdateInventorySlots: Slot index " + slotIndex + " is higher than inventory size.");
            }

            // If the slot index is within inventory size, sets the item for that slot.
            else
            {
                inventorySlots[slotIndex].UpdateItem(playerInventory.inventory[slotIndex]);
            }
        }
    }

}
