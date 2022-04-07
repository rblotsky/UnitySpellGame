using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

[DisallowMultipleComponent]
public class MerchantUIManager : UIBase
{
    // DATA //
    // References
    [Header("Merchant UI Data")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public MerchantUIItemArea buyingArea;
    public MerchantUIItemArea sellingArea;
    public MerchantUIPlayerInventory playerInventoryDisplay;
    public MerchantUITradeArea tradeCompletionArea;

    // Constants
    public static readonly int MAX_REFRESH_FOR_NOT_EMPTY_TRADE_TRIES = 50;

    // Cached Data
    private MerchantInteractable assignedMerchant;


    // FUNCTIONS //
    // Setup Functions
    public void StartUIManager(MerchantInteractable merchant)
    {
        // Caches assigned merchant
        assignedMerchant = merchant;

        // Opens the UI instance
        DataRef.sceneMenuManagerReference.OpenNewUIInstance(this);

        // Sets up the UI
        Setup(assignedMerchant);
    }

    public void Setup(MerchantInteractable merchant)
    {
        // This sets up the UI when it is first opened. Gets data from merchantDataList, fills up slots, and writes text.
        assignedMerchant = merchant;

        // Sets the UI title
        SetTitle();

        // Gets selling and buying trades and keeps resetting key until receives non-empty for both. If over Maximum tries, gives up.
        int numTries = 0;
        Usable[] buyTrades = assignedMerchant.GetBuyTrades();
        Usable[] sellTrades = assignedMerchant.GetSellTrades();

        while (numTries < MAX_REFRESH_FOR_NOT_EMPTY_TRADE_TRIES)
        {
            // If there are trades in both categories, breaks look and uses those.
            if (buyTrades.Length > 0 && sellTrades.Length > 0)
            {
                break;
            }

            // Otherwise, regenerates key and tries again.
            MerchantDataList.ResetTradeKey(assignedMerchant.merchantIdentifier);

            buyTrades = assignedMerchant.GetBuyTrades();
            sellTrades = assignedMerchant.GetSellTrades();

            // Increments numTries
            numTries++;
        }

        // Updates UI according to buy/sell trades
        buyingArea.SetupItemArea(buyTrades, this);
        sellingArea.SetupItemArea(sellTrades, this);

        // Updates the trade area UI
        tradeCompletionArea.Setup(this, assignedMerchant);

        // Updates the player inventory UI
        playerInventoryDisplay.Setup(this);
    }


    // UI Event Functions
    public void SetMerchantSellingItem(Usable item)
    {
        // Sets the selling item
        tradeCompletionArea.sellingSlot.UpdateItem(item);
    }

    public void SetMerchantBuyingItem(Usable item)
    {
        // Sets the buying item
        tradeCompletionArea.buyingSlot.UpdateItem(item);
    }


    // UI Updating Functions
    public void SetTitle()
    {
        // Sets the name and description text boxes
        nameText.SetText(assignedMerchant.merchantName);
        descriptionText.SetText(assignedMerchant.merchantSubtitle);
    }


    // Utility Functions
    public bool IsItemInBuyTrades(Usable item)
    {
        // Returns whether the buy trades contain given item
        return assignedMerchant.GetBuyTrades().Contains(item);
    }

}
