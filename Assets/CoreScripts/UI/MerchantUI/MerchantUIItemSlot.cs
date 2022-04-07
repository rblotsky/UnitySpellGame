using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class MerchantUIItemSlot : UIItemSlot, IPointerDownHandler
{   
    // DATA //
    // Instance Data
    public MerchantUIItemSlotType slotType;

    // Cached Data
    private MerchantUIManager currentUIManager;


    // FUNCTIONS //
    // Interface Functions
    public void OnPointerDown(PointerEventData pointerEvent)
    {
        // If the slot type is a selling slot, sets it as the selling item
        if(slotType == MerchantUIItemSlotType.Sell_Slot)
        {
            currentUIManager.SetMerchantSellingItem(itemInSlot);
        }

        // If it's an inventory slot, sets it as the buying item
        else if(slotType == MerchantUIItemSlotType.Inventory_Slot)
        {
            // Makes sure this item is being bought by the merchant before setting it as buying item
            if (currentUIManager.IsItemInBuyTrades(itemInSlot))
            {
                currentUIManager.SetMerchantBuyingItem(itemInSlot);
            }
        }

        // If it's a trade area slot, does nothing but clears slot
        else if(slotType == MerchantUIItemSlotType.Trade_Area_Slot)
        {
            UpdateItem(null);
        }
    }


    // Setup Function
    public void Setup(MerchantUIManager merchantUIManager)
    {
        Debug.Log("Setting up item slot! " + name);
        currentUIManager = merchantUIManager;
    }    
}
