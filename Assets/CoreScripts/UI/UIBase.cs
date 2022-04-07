using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class UIBase : MonoBehaviour
{
    // UI DATA //
    // Management References
    public List<GameObject> itemsToManage;

    // Instance Data
    public bool deleteOnClose = false;

    // Events
    public UnityEvent OnUIOpen;
    public UnityEvent OnUIClose;

    // Cached Data
    protected bool isOpen = false;


    // FUNCTIONS //
    // Open and Close UI (Run from whatever is opening/closing the UI)
    public virtual void OpenUI()
    {
        // Enables everything in the list of items to manage
        foreach(GameObject item in itemsToManage)
        {
            item.SetActive(true);
        }

        isOpen = true;
        OnUIOpen.Invoke();
    }

    public virtual void CloseUICustomDoDelete(bool doDeleteUI)
    {
        // Disables everything in the list of items to manage
        foreach (GameObject item in itemsToManage)
        {
            item.SetActive(false);

            // Destroys item if it has to be removed
            if (doDeleteUI)
            {
                Destroy(item);
            }
        }

        OnUIClose.Invoke();
        isOpen = false;

        // Destroys itself
        if (doDeleteUI)
        {
            Destroy(gameObject);
        }
    }

    public virtual void CloseUI()
    {
        // Uses deleteOnClose value for doDeleteUI on CloseUICustomDoDelete
        CloseUICustomDoDelete(deleteOnClose);
    }
}
