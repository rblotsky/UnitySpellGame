using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantUIItemArea : MonoBehaviour
{
    // DATA //
    // References
    public MerchantUIItemSlot[] itemSlots;

    // Cached Data
    private PageBasedObjectCollection<Usable> itemCollection;
    private int currentPage = 0;


    // FUNCTIONS //
    // Setup Function
    public void SetupItemArea(Usable[] items, MerchantUIManager uiManager)
    {
        // Creates the item pages
        itemCollection = new PageBasedObjectCollection<Usable>(items, itemSlots.Length);

        // Sets up all the item slots
        foreach(MerchantUIItemSlot slot in itemSlots)
        {
            slot.Setup(uiManager);
        }

        // Resets current page
        currentPage = 0;

        // Displays UI
        UpdateItemAreaUI();
    }


    // UI Event Functions
    public void UpdatePage(int amountToChange)
    {
        int newPage = currentPage += amountToChange;

        // If the new page is higher than the last page, or below 0, does nothing.
        if(newPage+1 > itemCollection.totalPages || newPage < 0)
        {
            return;
        }

        // Otherwise, sets currentPage to newPage and updates UI
        currentPage = newPage;
        UpdateItemAreaUI();
    }
    

    // UI Updating Functions
    public void UpdateItemAreaUI()
    {
        // Gets items on this page
        Usable[] itemsOnPage = itemCollection.GetPageObjects(currentPage);

        // For each itemslot, displays item in it
        for(int slotIndex = 0; slotIndex < itemSlots.Length; slotIndex++)
        {
            Debug.Log("Displaying Slot at Index: " + slotIndex);

            // Resets displayed usable
            itemSlots[slotIndex].UpdateItem(null);

            // If there is an item on this slot, sets the slot to display it.
            // Checks this by making sure the slot index isn't outside the length of the items array.
            if(itemsOnPage.Length > slotIndex)
            {
                Debug.Log("Updating item: " + itemSlots[slotIndex].name + ", with item " + itemsOnPage[slotIndex].name);
                itemSlots[slotIndex].UpdateItem(itemsOnPage[slotIndex]);
            }
        }
    }

}
