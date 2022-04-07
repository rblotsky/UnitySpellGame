using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellEditorManager : MonoBehaviour
{
    // This class manages the Spell Editor UI Window.
    /*
     * Stuff it does:
     * - Maintains references to other spell editor UI components
     * - Manages communications between other spell editor UI components
     * - Checks for spell editing actions and manages those actions (choosing a component to add, removing one, etc.)
     */

    // DATA //
    // References
    public SpellCollectionUI spellCollection;
    public SpellPanelComponentSlotUI[] editorComponentSlots;
    public Text[] spellButtons;

    // Cache Data
    private PlayerSpellComponent selectedComponent;
    private int spellNum = 1;
    public PlayerSpellObject selectedSpell = null;
    private PlayerComponent playerInScene;


    // FUNCTIONS //
    // Basic Functions
    private void Awake()
    {
        // Gets player reference
        playerInScene = FindObjectOfType<PlayerComponent>();
    }

    // Startup Function
    public void StartUp()
    {
        // Resets cache data
        selectedComponent = null;
        spellNum = 1;
        selectedSpell = PlayerSpells.main.spell1;

        // References this in collection
        spellCollection.editorManager = this;

        // Displays spell panel
        DisplaySpellPanel();
    }


    // Editing Spells Functions
    public void SelectSpellNum(int newSpell)
    {
        // If newSpell is a legal value, changes spellToEdit to newSpell. Otherwise, logs error.
        if(newSpell == 1 || newSpell == 2)
        {
            spellNum = newSpell;

            if(newSpell == 1)
            {
                selectedSpell = PlayerSpells.main.spell1;
            }

            else
            {
                selectedSpell = PlayerSpells.main.spell2;
            }

            DisplaySpellPanel();
            spellCollection.RefreshPage();
        }

        else
        {
            Debug.LogError("[SpellEditorManager] SelectSpell received illegal value: " + newSpell);
        }
    }

    public void SelectCollectionSlot(int slot)
    {
        selectedComponent = spellCollection.GetComponentAtSlot(slot);
    }

    public void ClearSelectedCollectionSlot()
    {
        selectedComponent = null;
    }

    public void AddComponentShapeSlot(SpellPanelComponentSlotUI editorSlotUsed)
    {
        // If no selected component, does nothing.
        if(selectedComponent == null)
        {
            return;
        }

        // If the component is a Shape component, adds it.
        if(selectedComponent is SpellShapeComponent)
        {
            PlayerSpells.main.AddComponent(selectedComponent, editorSlotUsed.slotType, spellNum, editorSlotUsed.slotIndex);
            DisplaySpellPanel();
        }

        spellCollection.RefreshPage();
    }

    public void AddComponentModifierSlot(SpellPanelComponentSlotUI editorSlotUsed)
    {
        // If no selected component, does nothing.
        if (selectedComponent == null)
        {
            return;
        }

        // If the component is a Modifier component, adds it.
        if (selectedComponent is SpellModifierComponent)
        {
            // Checks if the component is already added in this spell
            PlayerSpellObject editedSpell = null;

            if(spellNum == 1)
            {
                editedSpell = PlayerSpells.main.spell1;
            }

            if (spellNum == 2)
            {
                editedSpell = PlayerSpells.main.spell2;
            }

            if (!editedSpell.spellModifiers.Contains((SpellModifierComponent)selectedComponent))
            {
                PlayerSpells.main.AddComponent(selectedComponent, editorSlotUsed.slotType, spellNum, editorSlotUsed.slotIndex);
                DisplaySpellPanel();
            }
        }

        spellCollection.RefreshPage();
    }

    public void ClearComponentSlot(SpellPanelComponentSlotUI editorSlotUsed)
    {
        // Removes component from slot dependent on values in editorSlotUsed
        PlayerSpells.main.AddComponent(null, editorSlotUsed.slotType, spellNum, editorSlotUsed.slotIndex);

        // Displays UI
        DisplaySpellPanel();
        spellCollection.RefreshPage();
    }


    // Displaying Spell Panel Functions
    private void DisplaySpellPanel()
    {
        // Displays current spell being edited
        if(spellNum == 1)
        {
            spellButtons[0].fontStyle = FontStyle.Bold;
            spellButtons[1].fontStyle = FontStyle.Normal;
        }

        else
        {
            spellButtons[1].fontStyle = FontStyle.Bold;
            spellButtons[0].fontStyle = FontStyle.Normal;
        }

        // Displays each component slot
        foreach(SpellPanelComponentSlotUI slot in editorComponentSlots)
        {
            //TODO: Have this not use a separate class
            slot.DisplayComponent(selectedSpell);
        }
    }
}
