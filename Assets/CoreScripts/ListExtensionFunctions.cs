using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class ListExtensionFunctions
{
    public static void MoveItemToFrontOfList<T>(this List<T> list, T item)
    {
        // Removes item from list, then re-adds at front
        list.RemoveAt(list.IndexOf(item));
        list.Insert(0, item);
    }

    public static void MoveItemToFrontOfList<T>(this List<T> list, int index)
    {
        // Removes item at index, then re-adds at front
        T itemToMove = list[index];
        list.RemoveAt(index);
        list.Insert(0, itemToMove);
    }
}
