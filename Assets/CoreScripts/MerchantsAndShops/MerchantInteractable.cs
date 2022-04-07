using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MerchantInteractable : Interactable
{
    // MERCHANT DATA //
    // Basic Data
    [Header("Merchant Title (No Commas)")]
    public string merchantName;
    public string merchantSubtitle;
    public string merchantIdentifier;

    // References
    [Header("UI")]
    public GameObject merchantUIPrefab;

    // Selling Data
    [Header("Sell Data")]
    public MerchantTradeData defaultSellData;
    public MerchantTradeData SellData { get { return MerchantDataList.GetTradeData(MerchantDataList.GetMerchantData(merchantIdentifier).merchantSellTrades); } }

    // Buying Data
    [Header("Buy Data")]
    public MerchantTradeData defaultBuyData;
    public MerchantTradeData BuyData { get { return MerchantDataList.GetTradeData(MerchantDataList.GetMerchantData(merchantIdentifier).merchantBuyTrades); } }


    // OVERRIDES //
    protected override void Interact()
    {
        // Opens merchant UI, if not viewing a menu
        if (!DataRef.sceneMenuManagerReference.IsViewingMenu)
        {
            OpenMerchantUI();
        }
    }


    // FUNCTIONS //
    // Basic Functions
    protected override void Awake()
    {
        // Checks if there is already data saved for this merchant.
        MerchantData data = MerchantDataList.GetMerchantData(merchantIdentifier);

        // If no data is saved yet, saves the current data
        if (data == null)
        {
            if (defaultBuyData != null && defaultSellData != null)
            {
                data = new MerchantData(merchantIdentifier, 0, defaultBuyData.id.ToString(), defaultSellData.id.ToString());
            }
            else
            {
                data = new MerchantData(merchantIdentifier, 0, "none", "none");
            }

            MerchantDataList.SaveMerchantData(data);
        }

        base.Awake();
    }


    // Opening UI
    public virtual void OpenMerchantUI()
    {
        // Logs error if there is no UI Prefab
        if (merchantUIPrefab == null)
        {
            Debug.LogError("[MerchantInteractable] Failed opening Merchant UI as it does not have a UI Prefab! (\"" + name + "\")");
            return;
        }

        // Instantiates UI prefab
        GameObject instancedUIPrefab = Instantiate(merchantUIPrefab);

        // Gets its MerchantUIManager component, logs error if it can't find it
        MerchantUIManager instanceUIManager = instancedUIPrefab.GetComponent<MerchantUIManager>();

        if(instanceUIManager == null)
        {
            Debug.LogError("[MerchantInteractable] Failed opening Merchant UI as prefab does not have a MerchantUIManager component! (\"" + name + "\")");
            return;
        }

        // Sets up the UI manager
        instanceUIManager.StartUIManager(this);
    }


    // Trade Completion
    public virtual bool CompleteTrade(Usable merchantSoldItem, Usable merchantBoughtItem)
    {
        // Note: Returns true if trade completed, false if failed.
        // Gets player inventory reference
        PlayerEquipment playerInventory = PlayerEquipment.main;

        // Makes sure player has bought item. If not, returns false (trade could not be completed) and logs error.
        if (playerInventory.GetNumOfItem(merchantBoughtItem, false) < 1)
        {
            Debug.LogError("[MerchantInteractable] Could not complete trade (BUY " + merchantBoughtItem.name + ", SELL " + merchantSoldItem.name + ") because player does not have bought item.");
            return false;
        }

        // If the player has the bought item, removes bought item from inventory and adds sold item.
        else
        {
            playerInventory.RemoveItemFromInventory(merchantBoughtItem, true, false);
            playerInventory.AddItemToFirstEmptySlot(merchantSoldItem, true);
            return true;
        }
    }


    // Trade Data Requests
    public Usable[] GetBuyTrades()
    {
        float key = MerchantDataList.GetTradeKey(merchantIdentifier);

        if (BuyData != null)
        {
            return BuyData.GetTrades(key);
        }

        else
        {
            return new Usable[0];
        }
    }

    public Usable[] GetSellTrades()
    {
        float key = MerchantDataList.GetTradeKey(merchantIdentifier);

        if (SellData != null)
        {
            return SellData.GetTrades(key);
        }

        else
        {
            return new Usable[0];
        }
    }
    

    // Editor Functions
    [ContextMenu("Generate Identifier")]
    public void GenerateMerchantIdentifier()
    {
        // Creates new identifier text
        string newIdentifier = merchantName.Replace(" ", "").ToUpper() + "_" + merchantSubtitle.Replace(" ", "").ToUpper();

        // Sets identifier to the new text
        merchantIdentifier = newIdentifier;
    }
}
