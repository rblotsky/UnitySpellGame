using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using Newtonsoft.Json;

public class PlayerEquipment
{
    // Data Structures //
    public enum SearchType
    {
        Equal,
        Greater,
        Less,
        Less_Equal,
        Greater_Equal,
    }


    // INVENTORY DATA //
    // References
    public Usable[] inventory = new Usable[6];

    // Cached data
    public int selectedSlot = 0;

    // Events
    public delegate void NoInputDelegate();
    public event NoInputDelegate onInventoryUpdate;

    // Constants
    public static int EMPTY_SELECTED_SLOT_VALUE = -999;

    // Properties
    public int InventorySize { get { return inventory.Length; } }
    public Usable SelectedItem { get { return inventory[selectedSlot]; } set { UpdateSlot(selectedSlot, value, false); } }
    public bool IsEmpty { get { return GetNumOfItem(null, true) == InventorySize; } }

    // Singleton Pattern
    public static PlayerEquipment main;


    // FUNCTIONS //
    // Inventory Management
    public bool AddItemToFirstEmptySlot(Usable itemToAdd, bool runEvent)
    {
        // Stops if there is no item given
        if(itemToAdd == null)
        {
            return false;
        }

        // Loops through inventory slots, if one is empty adds item and returns true
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                // Updates slot with new item
                UpdateSlot(i, itemToAdd, runEvent);

                return true;
            }
        }

        // Returns false if none are added
        return false;
    }

    public void SetSelectedSlot(int newSelectedSlot, bool runEvent)
    {
        // Does nothing if it's the same slot as already selected or empty value
        if(newSelectedSlot == selectedSlot || newSelectedSlot == EMPTY_SELECTED_SLOT_VALUE)
        {
            return;
        }

        // Sets new slot as selected
        selectedSlot = newSelectedSlot;

        // Runs PlayerComponent OnInventoryUpdate event
        if (runEvent)
        {
            if(onInventoryUpdate != null)
            {
                onInventoryUpdate();
            }
        }
    }
    
    public void UseSelectedItem()
    {
        Usable itemToUse = SelectedItem;

        if(itemToUse != null)
        {
            // true = remove, false = dont
            if(itemToUse.UseItem(DataRef.playerReference))
            {
                RemoveItemFromInventory(selectedSlot, true);
            }
        }
    }

    public void DropSelectedItem()
    {
        Usable itemToDrop = SelectedItem;

        if (itemToDrop != null)
        {
            if (itemToDrop.DropItem(DataRef.playerReference.GetPlayerPosition()))
            {
                RemoveItemFromInventory(selectedSlot, true);
            }
        }
    }

    public bool RemoveItemFromInventory(Usable itemToRemove, bool runEvent, bool trueComparison)
    {
        // Returns whether an item was removed
        // Searches through all items, removes first which matches itemToRemove
        for (int i = 0; i < inventory.Length; i++)
        {
            if (trueComparison)
            {
                if (inventory[i] == itemToRemove)
                {
                    return RemoveItemFromInventory(i, runEvent);
                }
            }

            else
            {
                if (inventory[i] == null)
                {
                    if (itemToRemove == null)
                    {
                        return RemoveItemFromInventory(i, runEvent);
                    }
                }

                else
                {
                    if (inventory[i].MechanicalComparison(itemToRemove))
                    {
                        return RemoveItemFromInventory(i, runEvent);
                    }
                }
            }
        }

        return false;
    }

    public bool RemoveItemFromInventory(Usable itemToRemove, bool runEvent, int amount, bool trueComparison)
    {
        // Returns whether an item was removed
        // Finds the indices of the given item
        List<int> itemIndices = new List<int>();

        for(int i = 0; i < inventory.Length; i++)
        {
            if (trueComparison)
            {
                if(inventory[i] == itemToRemove)
                {
                    itemIndices.Add(i);
                }
            }

            else
            {
                if (inventory[i] == null)
                {
                    if(itemToRemove == null)
                    {
                        itemIndices.Add(i);
                        continue;
                    }
                }

                else
                {
                    if (inventory[i].MechanicalComparison(itemToRemove))
                    {
                        itemIndices.Add(i);
                    }
                }
            }

            // Stops searching if itemindices is at amount requested
            if(itemIndices.Count == amount)
            {
                break;
            }
        }

        // If the number of indices found is equal to amount requested, removes them all and returns true
        if(itemIndices.Count == amount)
        {
            foreach(int index in itemIndices)
            {
                RemoveItemFromInventory(index, runEvent);
            }

            return true;
        }

        // Otherwise, if indices found is below requested, returns false
        else
        {
            Debug.Log("[PlayerEquipment] RemoveItemFromInventory called but amount of item below requested amount, no items removed.");
            return false;
        }
    }

    public bool RemoveItemFromInventory(int index, bool runEvent)
    {
        // Return value is equal to whether there was an item there
        bool returnValue = inventory[index] != null;

        // Removes, returns return value
        UpdateSlot(index, null, runEvent);
        return returnValue;
    }

    public void ResetInventory(bool runEvent)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            UpdateSlot(i, null, runEvent);
        }
    }

    /// <summary>
    /// Sets item at given slot to newItem
    /// </summary>
    /// <param name="index">Indexing starts at 0</param>
    /// <param name="newItem">Item to place at given slot</param>
    /// <param name="doRunEvents">Whether to run PlayerComponent OnInventoryUpdate event</param>
    private void UpdateSlot(int index, Usable newItem, bool doRunEvents)
    {
        // Sets item at index to 
        inventory[index] = newItem;

        // Runs Inventory Update events
        if (doRunEvents)
        {
            if (onInventoryUpdate != null)
            {
                onInventoryUpdate();
            }
        }
    }


    // Checking Functions
    public bool RunInventorySearch(Usable item, int amount, SearchType searchQuery = SearchType.Equal, bool trueComparison = false)
    {
        // Gets the number of items, then compares using search query passed as argument
        int numOfItem = GetNumOfItem(item, trueComparison);

        switch (searchQuery)
        {
            case SearchType.Equal:
                return numOfItem == amount;

            case SearchType.Greater:
                return numOfItem > amount;

            case SearchType.Less:
                return numOfItem < amount;

            case SearchType.Greater_Equal:
                return numOfItem >= amount;

            case SearchType.Less_Equal:
                return numOfItem <= amount;
        }

        // Returns false by default. Note: This will literally never happen. The compiler just complains when its not here because it thinks the switch statement
        // might not run any of the cases (it will, because those are all the possible values of SearchType)
        return false;
    }

    public bool InventoryContains(Usable itemToSearch, bool trueEquality)
    {
        return GetNumOfItem(itemToSearch, trueEquality) > 0;
    }

    public int GetNumOfItem(Usable itemToSearch, bool trueEquality)
    {
        int numFound = 0;

        // Adds 1 to numFound for every copy found
        foreach(Usable item in inventory)
        {
            if (trueEquality)
            {
                if (item == itemToSearch)
                {
                    numFound++;
                }
            }

            else
            {
                if (item == null)
                {
                    if (itemToSearch == null)
                    {
                        numFound++;
                        continue;
                    }
                }

                else
                {
                    if (item.MechanicalComparison(itemToSearch))
                    {
                        numFound++;
                    }
                }
            }
        }

        return numFound;
    }


    // Data Management
    public Queue<string> SaveDataToStringQueue(Formatting format = Formatting.None)
    {
        Queue<string> queue = new Queue<string>();
        List<string> inventoryNames = new List<string>();
        foreach (Usable item in inventory)
        {
            if (item != null)
            {
                inventoryNames.Add(item.id.ToString());
            }

            else
            {
                inventoryNames.Add("none");
            }
        }

        queue.Enqueue(JsonConvert.SerializeObject(inventoryNames, format));
        return queue;
    }

    public void LoadDataFromText(string text)
    {
        List<string> inventoryNames = new List<string>(JsonConvert.DeserializeObject<List<string>>(text));

        for(int i = 0; i < inventoryNames.Count; i++) 
        {
            Usable item = UsableList.FindItem(inventoryNames[i]);
            UpdateSlot(i, item, false);
        }
    }


    // Singleton Pattern
    public static void SetMainReference()
    {
        main = new PlayerEquipment();
        main.ResetInventory(false);
    }
}