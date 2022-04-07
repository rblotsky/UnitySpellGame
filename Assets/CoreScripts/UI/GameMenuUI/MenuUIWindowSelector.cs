using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIWindowSelector : MonoBehaviour
{
    // WINDOW MANAGER DATA //
    public MenuUIWindow[] windows;
    public MenuUIWindow currentWindow;


    // FUNCTIONS //
    // Window Management Functions
    public void OpenWindow(MenuUIWindow windowToOpen)
    {
        if(windowToOpen == null)
        {
            Debug.LogError("[MenuUIWindowSelector] OpenWindow Passed Null Window!");
            return;
        }

        // Opens window
        windowToOpen.OpenWindow();
        currentWindow = windowToOpen;

        // Closes all other windows
        foreach (MenuUIWindow window in windows)
        {
            if (window != windowToOpen)
            {
                window.CloseWindow();
            }
        }
    }

    public void OpenWindow(string windowName)
    {
        // Opens window based off name
        OpenWindow(FindWindow(windowName));
    }

    public MenuUIWindow FindWindow(string name)
    {
        foreach(MenuUIWindow window in windows)
        {
            if(window.name.Equals(name))
            {
                return window;
            }
        }

        return null;
    }

    public void ResetCurrentWindow()
    {
        // When called, sets current window to default window.
        currentWindow = windows[0];
    }

    public void CloseWindows()
    {
        foreach(MenuUIWindow window in windows)
        {
            window.CloseWindow();
        }
    }
}
