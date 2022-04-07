using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverlayUIItemSlot : UIItemSlot
{
    // DATA //
    // References
    public Image selectionBackground;
    public Color selectedColour;
    public Color unusableTintColour;

    // Cached Data
    protected Color defaultBackgroundColour;
    protected Color defaultTintColour;


    // OVERRIDES //
    public override void UpdateSlotSprite()
    {
        // If there is no item, resets sprite
        if (itemInSlot == null)
        {
            spriteArea.sprite = defaultSprite;
        }

        // If there is a sprite, displays it
        else if (itemInSlot.displaySprite != null)
        {
            // If there is an item, and it has a sprite, displays it
            spriteArea.sprite = itemInSlot.displaySprite;
        }   

        // If there is no sprite, uses default sprite
        else
        {
            spriteArea.sprite = defaultSprite;
        }

        // Updates the slot tint
        UpdateSlotColourTint();
    }


    // FUNCTIONS //
    // Setup Function
    public void Setup()
    {
        // Caches default background and tint colour
        defaultBackgroundColour = selectionBackground.color;
        defaultTintColour = spriteArea.color;
    }

    // UI Updating Functions
    public void UpdateSelectedStatus(bool status)
    {
        // If the slot is selected, sets to selected colour
        if (status)
        {
            selectionBackground.color = selectedColour;
        }

        // If it is not selected, sets to default colour.
        else
        {
            selectionBackground.color = defaultBackgroundColour;
        }
    }

    public void UpdateSlotColourTint()
    {   
        // If the slot is empty, sets to default tint and returns
        if(itemInSlot == null)
        {
            spriteArea.color = defaultTintColour;
            return;
        }

        // If usable, uses default tint
        if(itemInSlot.isUsable)
        {
            spriteArea.color = defaultTintColour;
        }

        // Otherwise, uses unusable tint
        else
        {
            spriteArea.color = unusableTintColour;
        }
    }
}
