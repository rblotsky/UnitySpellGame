using UnityEngine;
using System.Collections;
using System.Text;
using System;

public class Usable : GameResource, IAttacker
{
    // STORED DATA //
    // Display Data
    [Header("Display Data")]
    [Tooltip("This will be overwritten if the derived class has a custom name function.")]
    public string displayName;
    [TextArea(1, 4)]
    [Tooltip("This will be overridden if the derived class has a custom description function.")]
    public string baseDescription;
    public Sprite displaySprite;

    // Use Modifiers
    [Header("Base Modifiers")]
    public bool isUsable = true;
    public bool canDrop = true;

    // Utility Instance Data
    [Header("Item Data")]
    [Tooltip("Merchants use this value to decide what to buy or sell.")]
    public UsableType itemType;
    public Vector3 dropOffset = new Vector3(1f, 0.5f, 1f);

    // References
    public GameObject dropItem;

    // Cached Data
    protected float effectInstanceStartTime;


    // FUNCTIONS //
    // Basic Functions
    protected virtual void Awake()
    {
        //Empty in base class
    }


    // Basic Item Functions
    public virtual bool UseItem(PlayerComponent playerToUse)
    {
        Debug.LogError("[Usable] " + name + " has no child class, running base UseItem");
        return true;
    }

    public virtual bool DropItem(Vector3 rootLocation)
    {
        // If the item can be dropped, drops it
        if (canDrop)
        {
            // Instantiates the item beside the player
            Vector3 locationToDropItem = rootLocation + dropOffset;
            Instantiate(dropItem, locationToDropItem, dropItem.transform.localRotation);

            return true;
        }

        return false;
    }

    public string GetTooltipText()
    {
        // Generates tooltip text in format: Name\nType\n\nDescription
        StringBuilder tooltipBuilder = new StringBuilder();
        tooltipBuilder.AppendLine("<b>" + GetItemName() + "</b>");
        tooltipBuilder.AppendLine(GameUtility.GenerateHTMLColouredText(itemType.ToString().Replace('_', ' '), UsableTypeColours.USABLE_TYPE_COLOURS[(int)itemType]));
        tooltipBuilder.AppendLine();
        tooltipBuilder.Append(GetItemDescription());

        return tooltipBuilder.ToString();
    }

    public virtual string GetItemDescription()
    {
        return baseDescription;
    }

    public virtual string GetItemName()
    {
        return displayName;
    }

    public virtual bool MechanicalComparison(Usable other)
    {
        // Default MechanicalComparison simply runs a true comparison
        return this == other;
    }


    // Instance Management Functions
    public virtual UsableInstance CreateItemInstance()
    {
        // TODO
        // This should create an instance of this usable, with copies of all its data.
        return null;
    }


    // Effect Instance Functions
    protected virtual IEnumerator DurationEffect()
    {
        Debug.LogError("[Usable] " + name + " is running base DurationEffect Coroutine.");
        yield return null;
    }
}
