using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneGeneralMenuManager : MonoBehaviour
{
    // DATA //
    // Main Data
    public Stack<UIBase> activeElements = new Stack<UIBase>();
    protected List<UIBase> elementsOpenedThisFrame = new List<UIBase>();
    public UIBase escapeMenu;

    // Properties
    public UIBase TopUIBase 
    { 
        get 
        {
            if (activeElements.Count > 0)
            {
                return activeElements.Peek();
            }

            else
            {
                return null;
            }
        } 
    }
    public bool IsViewingMenu { get { return activeElements.Count > 0; } }


    // FUNCTIONS //
    // Basic Functions
    private void Start()
    {
        // Sets StaticDataRef menu manager manager to this
        DataRef.sceneMenuManagerReference = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // If there are no menus active and there is an escape menu, opens the escape menu
            if (!IsViewingMenu && escapeMenu != null && !PlayerStats.isPlayerPaused)
            {
                OpenNewUIInstance(escapeMenu);
            }

            // If a menu is active, closes the top UI instance
            // Only closes top UI instance if there is one and it isn't in the not-close-by-escape items list
            if (TopUIBase != null && !elementsOpenedThisFrame.Contains(TopUIBase))
            {
                CloseTopUIInstance();
            }
        }

        // Clears the elementsOpenedThisFrame items list
        elementsOpenedThisFrame.Clear();
    }


    // UI Management
    public void CloseTopUIInstance()
    {
        CloseUIInstance(activeElements.Peek());
    }

    public void CloseUIInstance(UIBase itemToClose)
    {
        // Does nothing if no UI to close
        if(itemToClose == null)
        {
            return;
        }

        // If UI to close isn't in active menus, logs error and returns
        if (!activeElements.Contains(itemToClose))
        {
            Debug.LogError("[SceneGeneralMenuManager] CloseUIInstance itemToClose is not present in activeMenus! (\"" + itemToClose.name + "\")");
            return;
        }

        // Otherwise, closes that UI item and all items above it.
        while (true)
        {
            // Gets the current top UI item
            UIBase currentItem = activeElements.Peek();

            // Closes it
            //Debug.Log("Closing UI Element: " + currentItem.name);
            currentItem.CloseUI();
            activeElements.Pop();

            // If that UI item was the item to close, stops from going further
            if(currentItem == itemToClose)
            {
                break;
            }
        }

        // If there are no UI elements left, unpauses game and re-enables overlay
        if (!IsViewingMenu)
        {
            // Unpauses game
            Time.timeScale = 1;

            // If overlay UI is running, pauses it as well
            DataRef.overlayManagerReference.EnableOverlay();
        }
    }

    public void OpenNewUIInstance(UIBase itemToOpen)
    {
        // If this UI item is already open, doesn't open it.
        if (activeElements.Contains(itemToOpen))
        {
            return;
        }

        // Pauses game
        Time.timeScale = 0;

        // If overlay UI is running, pauses it as well
        if (DataRef.overlayManagerReference != null)
        {
            if (DataRef.overlayManagerReference.enabled)
            {
                DataRef.overlayManagerReference.DisableOverlay();
            }
        }

        // Opens the new UI instance and places it at top of existing menus, and adds it to elementsOpenedThisFrame
        itemToOpen.OpenUI();
        activeElements.Push(itemToOpen);
        elementsOpenedThisFrame.Add(itemToOpen);
        //Debug.Log("Opened UI Element: " + itemToOpen.name);
    }
}
