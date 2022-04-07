using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDisplayer : MonoBehaviour
{
    // This displays the player's currently selected item in their 'hand'

    // STORED DATA //
    // References
    public GameObject itemHolder;
    public GameObject currentHeldItem;
    public Usable currentItem;


    public void DisplayItem(Usable item)
    {
        // Does something to display the item the player is holding

        // IDEAS:
        /* 
         * 1. Get MeshFilter components for all child objects of item
         *    and then store the mesh and position of each.
         *    Then, create a new mesh built using all of those meshes with
         *    their relative positions in the player item holder.
         */
    }
}
