using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UsableList
{
    // DATA //
    // Main List
    public static Usable[] allUsables;


    // FUNCTIONS //
    // Setup
    public static void SetupFunction()
    {
        allUsables = Resources.LoadAll<Usable>("Usables");
    }


    // Searching Functions
    public static Usable FindItem(string id)
    {
        foreach(Usable item in allUsables)
        {
            if (item.id.ToString().Equals(id) == true)
            {
                return item;
            }
        }

        return null;
    }

    public static Usable[] GetItems(UsableType type)
    {
        // Finds all items of given merchant type and returns an array of them
        List<Usable> itemsOfType = new List<Usable>();

        foreach(Usable item in allUsables)
        {
            if(item.itemType == type)
            {
                itemsOfType.Add(item);
            }
        }

        return itemsOfType.ToArray();
    }
}
