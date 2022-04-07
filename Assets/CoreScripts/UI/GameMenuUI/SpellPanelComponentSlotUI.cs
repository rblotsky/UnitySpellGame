using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellPanelComponentSlotUI : MonoBehaviour
{
    // STORED DATA //
    // References
    public SpellComponentType slotType;
    public Text textDisplay;
    public int slotIndex = 0;
    
    

    // FUNCTIONS //
    // Display Management
    public void DisplayComponent(PlayerSpellObject spellToDisplay)
    {
        // Makes sure there is a spell given
        if(spellToDisplay == null)
        {
            Debug.LogError("[SpellPanelComponentSlotUI] DisplayComponent given null spellToDisplay!");
            return;
        }

        // Displays component according to the type of slot it is
        if(slotType == SpellComponentType.Shape)
        {
            // Displays only if the component exists
            if (spellToDisplay.shapeComponent != null)
            {
                textDisplay.text = spellToDisplay.shapeComponent.DisplayName;
            }

            else
            {
                textDisplay.text = "Empty Slot";
            }
        }

        else
        {
            // Displays according to slot index, if the component exists
            if (slotIndex < spellToDisplay.spellModifiers.Count)
            {
                textDisplay.text = spellToDisplay.spellModifiers[slotIndex].DisplayName;
            }

            else
            {
                textDisplay.text = "Empty Slot";
            }
        }
    }
}
