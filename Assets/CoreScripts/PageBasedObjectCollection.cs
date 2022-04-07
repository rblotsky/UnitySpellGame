using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageBasedObjectCollection<T>
{
    // COLLECTION DATA //
    // Instance Data
    public List<T> mainList;
    public int pageSize;

    // Properties
    public int totalPages { get { return (int)Mathf.Ceil(mainList.Count / pageSize); } }


    // CONSTRUCTORS //
    public PageBasedObjectCollection(List<T> list, int itemsPerPage)
    {
        mainList = list;
        pageSize = itemsPerPage;
    }

    public PageBasedObjectCollection(T[] list, int itemsPerPage)
    {
        mainList = new List<T>(list);
        pageSize = itemsPerPage;
    }

    public PageBasedObjectCollection(int itemsPerPage)
    {
        mainList = new List<T>();
        pageSize = itemsPerPage;
    }


    // FUNCTIONS //
    // Adding items
    public void AddItem(T item)
    {
        // Adds the item to the end of the list
        mainList.Add(item);
    }

    // Removing items
    public void RemoveItem(T item)
    {
        mainList.Remove(item);
    }

    public void RemoveItem(int index)
    {
        mainList.RemoveAt(index);
    }

    public void RemovePage(int pageIndex)
    {
        // Removes all items at given page
    }

    // Getting Pages
    public T[] GetPageObjects(int pageIndex)
    {

        // If page index is negative, returns empty list
        if(pageIndex < 0)
        {
            return new T[0];
        }

        // Gets start index of page (inclusive)
        int startIndex = pageIndex * pageSize;

        // If start index is higher than list size, returns empty list
        if (startIndex > mainList.Count - 1)
        {
            return new T[0];
        }

        // If the page size reaches past list size, lowers it to max size and returns page
        if (startIndex + pageSize > mainList.Count)
        {
            int lastPageSize = mainList.Count - startIndex;

            return mainList.GetRange(startIndex, lastPageSize).ToArray();
        }

        // Otherwise, returns page using regular page size
        else
        {
            return mainList.GetRange(startIndex, pageSize).ToArray();
        }
    }

    public List<T> GetPageObjectsAsList(int pageIndex)
    {
        // If page index is negative, returns empty list
        if (pageIndex < 0)
        {
            return new List<T>();
        }

        // Gets start index of page (inclusive)
        int startIndex = pageIndex * pageSize;

        // If start index is higher than list size, returns empty list
        if (startIndex > mainList.Count - 1)
        {
            return new List<T>();
        }

        // If the page size reaches past list size, lowers it to max size and returns page
        if (startIndex + pageSize > mainList.Count)
        {
            int lastPageSize = mainList.Count - startIndex;

            return mainList.GetRange(startIndex, lastPageSize);
        }

        // Otherwise, returns page using regular page size
        else
        {
            return mainList.GetRange(startIndex, pageSize);
        }
    }


}
