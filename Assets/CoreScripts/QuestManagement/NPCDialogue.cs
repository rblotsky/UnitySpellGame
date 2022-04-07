using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New NPC Dialogue", menuName = "Dialogue/NPC Dialogue")]
public class NPCDialogue : DialogueBase
{
    // DATA //
    // Added Data
    public Usable[] itemsToGiveOnFinish;

    
    // OVERRIDES //
    public override void RunDialogueFinish()
    {
        // Gives items, then runs base RunDialogueFinish
        GiveItems();
        base.RunDialogueFinish();
    }


    // FUNCTIONS //
    // Giving items on finish
    public void GiveItems()
    {
        // If there are items to give, gives them
        if (itemsToGiveOnFinish.Length > 0)
        {
            PlayerEquipment inventory = PlayerEquipment.main;

            // Tries adding every item to player inventory, if can't then drops them on ground using item's offset and item prefab's rotation
            foreach(Usable item in itemsToGiveOnFinish)
            {
                if(inventory.AddItemToFirstEmptySlot(item, true))
                {
                    continue;
                }

                else
                {
                    item.DropItem(DataRef.playerReference.GetPlayerPosition());
                }

            }
        }
    }
}

