using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class SpellCollectionUI : MonoBehaviour
{
    // This is a component added to the spell component 'collection' in the SpellEditor Menu window.
    // It manages the sorted list of all components, assigns components to different slots in the collection
    // depending on the current page and filters, and filters lists according to the player's selected filters.
    // It also acts as an intermediary between the SpellEditorManager and SpellCollectionSlot.

    // STORED DATA //
    // Basic Data
    [Header("Collection Data")]
    public Sprite emptySlotSprite;
    public float hoverDisplayTooltipTime = 1f;
    public Color cannotUseTint = Color.grey;
    public Color regularTint = Color.white;
    public string alreadyUsedText = "This component is already in this spell!";

    // Components
    private PageBasedObjectCollection<PlayerSpellComponent> componentCollection;

    // Referenced UI Elements
    [Header("References")]
    public GameObject[] collectionSlots;
    public UITooltip spellComponentTooltip;
    public SpellEditorManager editorManager;

    // Cached Data
    private PlayerComponent playerInScene;
    private int currentPage = 0;
    private List<(TextMeshProUGUI componentName, TextMeshProUGUI componentDescription, Image componentImage)> collectionSlotData = new List<(TextMeshProUGUI componentName, TextMeshProUGUI componentDescription, Image componentImage)>();
    private float hoverTime;
    private int currentHoverSlot = 0;


    // FUNCTIONS //
    // Basic Functions
    private void Update()
    {
        // If hover tooltip isnt to be displayed, closes it. If player is hovering, increments hover time until its hovering for over 2 seconds.
        if(hoverTime == 0)
        {
            if (spellComponentTooltip != null)
            {
                spellComponentTooltip.gameObject.SetActive(false);
            }
        }

        else
        {
            hoverTime += Time.unscaledDeltaTime;
        }

        // If hovering for over tooltip display time, displays and updates position
        if(hoverTime > hoverDisplayTooltipTime)
        {
            if (spellComponentTooltip != null)
            {
                spellComponentTooltip.gameObject.SetActive(true);
                spellComponentTooltip.UpdatePosition(Input.mousePosition);
                DisplayHoverPanelText();
            }
        }
    }

    private void Awake()
    {
        // Finds player reference
        playerInScene = FindObjectOfType<PlayerComponent>();
    }
    

    // Startup Function
    public void StartUp()
    {
        // Generates component collection from list of known components
        componentCollection = new PageBasedObjectCollection<PlayerSpellComponent>(PlayerSpells.main.availableComponents, collectionSlots.Length);

        // Generates collection slot data
        collectionSlotData.Clear();
        foreach(GameObject slot in collectionSlots)
        {
            TextMeshProUGUI nameText = slot.GetComponentsInChildren<TextMeshProUGUI>()[0];
            TextMeshProUGUI descriptionText = slot.GetComponentsInChildren<TextMeshProUGUI>()[1];
            Image image = slot.GetComponentInChildren<Image>();
            collectionSlotData.Add((nameText, descriptionText, image));
        }

        currentPage = 0;
        RefreshPage();
    }


    // Page Management Functions
    public void NextPage()
    {
        // Incremenets page, if below max page count
        if(currentPage < componentCollection.totalPages - 1)
        {
            currentPage++;
            RefreshPage();
        }
    }

    public void PreviousPage()
    {
        // Cannot go below 1
        if(currentPage > 0)
        {
            currentPage--;
            RefreshPage();
        }
    }

    public void ResetPage()
    {
        currentPage = 0;
        RefreshPage();
    }

    public void RefreshPage()
    {
        DisplayComponents(componentCollection.GetPageObjects(currentPage));
    }

    public PlayerSpellComponent GetComponentAtSlot(int slotIndex)
    {
        PlayerSpellComponent[] components = componentCollection.GetPageObjects(currentPage);

        if (slotIndex >= components.Length)
        {
            return null;
        }

        else
        {
            return components[slotIndex];
        }
    }


    // Page Displaying Functions
    public void DisplayComponents(PlayerSpellComponent[] pageComponents)
    {
        for(int i = 0; i < collectionSlots.Length; i++)
        {
            // If the index is a null pageComponent, adds null
            if (pageComponents.Length <= i)
            {
                DisplaySlot(null, i);
            }

            // Otherwise, adds the component at specified index
            else
            {
                DisplaySlot(pageComponents[i], i);
            }
        }
    }

    public void DisplaySlot(PlayerSpellComponent component, int slotIndex)
    {
        // Gets tuple of slot data
        (TextMeshProUGUI componentName, TextMeshProUGUI componentDescription, Image componentImage) slotData = collectionSlotData[slotIndex];

        // Displays component in it
        if(component == null)
        {
            slotData.componentName.SetText("");
            slotData.componentDescription.SetText("");
            slotData.componentImage.sprite = emptySlotSprite;
        }

        else
        {
            slotData.componentName.SetText(component.DisplayName);
            slotData.componentDescription.SetText(component.GetDisplayDescription());
            slotData.componentImage.sprite = component.spellManagerDisplaySprite;

            // Checks if the currently edited spell has this component already, if so adds a marker signifying that it can't be used again
            if (editorManager.selectedSpell.ContainsComponent(component))
            {
                slotData.componentImage.color = cannotUseTint;
            }

            else
            {
                slotData.componentImage.color = regularTint;
            }
        }
    }


    // Tooltip/Hovering Management Functions
    public void HoverStart(int slotIndex)
    {
        currentHoverSlot = slotIndex;
        hoverTime = 0.1f;
    }

    public void HoverFinish()
    {
        currentHoverSlot = -1;
        hoverTime = 0;
    }

    public void DisplayHoverPanelText()
    {
        // Checks if this slot has a component w/ keywords or if it's already used
        PlayerSpellComponent[] componentsOnPage = componentCollection.GetPageObjects(currentPage);
        string displayText = "";

        if (currentHoverSlot < componentsOnPage.Length)
        {
            PlayerSpellComponent component = componentsOnPage[currentHoverSlot];

            if (component != null)
            {
                if (component is SpellModifierComponent)
                {
                    SpellModifierComponent modComponent = (SpellModifierComponent)component;

                    if (modComponent.properties.keywords.Length > 0)
                    {
                        // Adds text for keywords
                        foreach (SpellKeyword keyword in modComponent.properties.keywords)
                        {
                            displayText += ("<b>" + keyword.displayName + "</b>\n" + keyword.functionalityDescription + "\n");
                        }
                    }
                }

                // Adds "This is already used" if its already used in the spell
                if (editorManager.selectedSpell.ContainsComponent(component))
                {
                    displayText += "<b>" + GameUtility.GenerateHTMLColouredText(alreadyUsedText, Color.red) + "</b>";
                }
            }
        }

        // If text isnt updated, closes the panel. Otherwise, displays text.
        if (string.IsNullOrEmpty(displayText)) 
        {
            spellComponentTooltip.UpdateText("");
            spellComponentTooltip.gameObject.SetActive(false);
        }

        else
        {
            spellComponentTooltip.gameObject.SetActive(true);
            spellComponentTooltip.UpdateText(displayText);
        }
    }
}
