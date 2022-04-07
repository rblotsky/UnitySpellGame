using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class MenuUIWindow : MonoBehaviour
{
    // UI DATA //
    // Management References
    public List<GameObject> itemsToManage;

    // Events
    public UnityEvent OnWindowOpen;
    public UnityEvent OnWindowClose;

    // Cached Data
    protected bool isOpen = false;


    // FUNCTIONS //
    // Open and Close UI (Run from whatever is opening/closing the UI)
    public virtual void OpenWindow()
    {
        // Enables everything in the list of items to manage
        foreach (GameObject item in itemsToManage)
        {
            item.SetActive(true);
        }

        isOpen = true;
        OnWindowOpen.Invoke();
    }

    public virtual void CloseWindow()
    {
        // Disables everything in the list of items to manage
        foreach (GameObject item in itemsToManage)
        {
            item.SetActive(false);
        }

        OnWindowClose.Invoke();
        isOpen = false;
    }
}
