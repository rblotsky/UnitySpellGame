using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // DATA //
    // References
    public Image spriteArea;
    public Sprite defaultSprite;
    public UITooltip mainHoverPanel;
    public Usable itemInSlot;

    // Cached Data
    protected bool isMouseHovering;

    // Events
    public UnityEvent OnSlotUpdate;


    // FUNCTIONS //
    // Basic Functions
    private void Update()
    {
        // If the player is hovering over this slot
        if (isMouseHovering)
        {
            // If there is no item in the item slot, closes hover panel
            if (itemInSlot == null)
            {
                CloseHoverPanel();
            }

            // Otherwise, updates it.
            else
            {
                // If it's already opened, updates it
                if (mainHoverPanel.gameObject.activeInHierarchy)
                {
                    UpdateHoverPanel();
                }

                // Otherwise, opens it
                else
                {
                    OpenHoverPanel();
                }
            }
        }
    }


    // External Updating Functions
    public virtual void UpdateItem(Usable newItem)
    {
        // Updates the itemInSlot and UI data
        itemInSlot = newItem;
        UpdateSlotSprite();
        OnSlotUpdate.Invoke();
    }


    // Interface Functions
    public void OnPointerEnter(PointerEventData pointerData)
    {
        isMouseHovering = true;
    }

    public void OnPointerExit(PointerEventData pointerData)
    {
        isMouseHovering = false;
        CloseHoverPanel();
    }


    // UI Updating Functions
    public virtual void OpenHoverPanel()
    {
        // Does nothing if there is no hover panel or the item is null
        if(mainHoverPanel == null || itemInSlot == null)
        {
            return;
        }

        // Activates the hover panel
        mainHoverPanel.gameObject.SetActive(true);

        // Updates the text and position
        UpdateHoverPanel();
    }

    public virtual void CloseHoverPanel()
    {
        // Does nothing if there is no hover panel
        if (mainHoverPanel == null)
        {
            return;
        }

        mainHoverPanel.gameObject.SetActive(false);
    }

    public virtual void UpdateHoverPanel()
    {
        // Updates the text and position of the panel
        mainHoverPanel.UpdateText(itemInSlot.GetTooltipText());
        mainHoverPanel.UpdatePosition(Input.mousePosition);
    }

    public virtual void UpdateSlotSprite()
    {
        if (itemInSlot == null)
        {
            spriteArea.sprite = defaultSprite;
        }

        else if (itemInSlot.displaySprite != null)
        {
            spriteArea.sprite = itemInSlot.displaySprite;
        }

        else
        {
            spriteArea.sprite = defaultSprite;
        }
    }
}
