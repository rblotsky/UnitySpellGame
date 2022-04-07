using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class MerchantUITradeArea : MonoBehaviour
{
    // DATA //
    // References
    public Button completeButton;
    public MerchantUIItemSlot buyingSlot;
    public MerchantUIItemSlot sellingSlot;

    // Cached Data
    private MerchantUIManager currentUIManager;
    private MerchantInteractable merchantToUse;


    // FUNCTIONS //
    // Setup Function
    public void Setup(MerchantUIManager newUIManager, MerchantInteractable newMerchant)
    {
        // Sets the new UI manager
        currentUIManager = newUIManager;

        // Sets the new merchant to use
        merchantToUse = newMerchant;

        // Sets up buying and selling slots
        buyingSlot.Setup(newUIManager);
        sellingSlot.Setup(newUIManager);

        // Resets buying and selling slots
        buyingSlot.UpdateItem(null);
        sellingSlot.UpdateItem(null);

        // Sets the button to disabled
        completeButton.interactable = false;
    }


    // UI Event Functions
    public void UpdateButtonInteractivity()
    {
        // If trade is possible, sets button to interactable. Otherwise, disables it.
        if (IsTradePossible())
        {
            completeButton.interactable = true;
        }

        else
        {
            completeButton.interactable = false;
        }
    }

    public void RunTrade()
    {
        // Runs the trade from the assigned merchant, if a trade is possible
        if (IsTradePossible())
        {
            // Completes the trade from the merchant
            merchantToUse.CompleteTrade(sellingSlot.itemInSlot, buyingSlot.itemInSlot);

            // Clears the buying slot
            buyingSlot.UpdateItem(null);
        }
    }


    // Utility Functions
    public bool IsTradePossible()
    {
        // Returns true if neither buying/selling slot is null and player has the buying item
        if (buyingSlot.itemInSlot != null && sellingSlot.itemInSlot != null)
        {
            // Makes sure player has the buying item
            if (PlayerEquipment.main.InventoryContains(buyingSlot.itemInSlot, true))
            {
                return true;
            }
        }

        // Otherwise, returns false
        return false;
    }
}
