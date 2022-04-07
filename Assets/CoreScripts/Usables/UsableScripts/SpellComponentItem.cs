using UnityEngine;

[CreateAssetMenu(fileName = "New SpellComponent Item", menuName = "Usables/SpellComponentItem")]
public class SpellComponentItem : Usable
{
    // DATA //
    // References
    public PlayerSpellComponent attachedSpellComponent;
    public GameObject usageEffect;


    // FUNCTIONS //
    // Basic Functions
    protected override void Awake()
    {
        itemType = UsableType.Spell_Tome;
    }


    // Item Management
    public override bool UseItem(PlayerComponent playerToUse)
    {
        // Only learns the new component if it's not already known
        if (PlayerSpells.main.LearnNewComponent(attachedSpellComponent))
        {
            // Plays usage effect, then learns new component
            if (usageEffect != null)
            {
                Instantiate(usageEffect, playerToUse.GetPlayerPosition(), usageEffect.transform.localRotation);
            }

            return true;
        }

        return false;
        
    }

    public override string GetItemDescription()
    {
        return attachedSpellComponent.GetDisplayDescription();
    }
}
