using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class MenuUIManager : UIBase
{
    // MENU DATA //
    public MenuUIWindowSelector windowManager;


    // FUNCTIONS
    // Basic functions
    private void Start()
    {
        // Resets window
        windowManager.ResetCurrentWindow();

        // Sets up all tooltips
        UITooltip[] tooltips = GetComponentsInChildren<UITooltip>();
        foreach (UITooltip tooltip in tooltips)
        {
            tooltip.Setup();
        }

        // Closes UI
        CloseUICustomDoDelete(false);
    }


    // Overrides
    public override void OpenUI()
    {
        // Opens Window Selector and current window
        windowManager.OpenWindow(windowManager.currentWindow);
        base.OpenUI();
    }

    public override void CloseUICustomDoDelete(bool doDeleteUI)
    {
        // Closes all windows and window selector
        windowManager.CloseWindows();
        base.CloseUICustomDoDelete(doDeleteUI);
    }
}
